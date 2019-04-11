using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using ISerializer = Phema.Serialization.ISerializer;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducer<TPayload>
	{
		Task<bool> Produce(TPayload payload);


		Task<bool> BatchProduce(IEnumerable<TPayload> payloads);
	}
}

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQProducer<TPayload> : IRabbitMQProducer<TPayload>
	{
		// Each generic type has unique semaphore, because unique channel
		private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1);

		private readonly IModel channel;
		private readonly ISerializer serializer;
		private readonly IBasicProperties properties;
		private readonly IRabbitMQProducerDeclaration declaration;

		public RabbitMQProducer(
			IModel channel,
			ISerializer serializer,
			IRabbitMQProducerDeclaration declaration,
			IBasicProperties properties)
		{
			this.channel = channel;
			this.declaration = declaration;
			this.serializer = serializer;
			this.properties = properties;

			if (declaration.WaitForConfirms)
			{
				channel.ConfirmSelect();
			}

			if (declaration.Transactional)
			{
				channel.TxSelect();
			}
		}

		public async Task<bool> Produce(TPayload payload)
		{
			try
			{
				await Semaphore.WaitAsync().ConfigureAwait(false);

				channel.BasicPublish(
					declaration.ExchangeName,
					declaration.RoutingKey ?? declaration.ExchangeName,
					declaration.Mandatory,
					properties,
					serializer.Serialize(payload));

				if (declaration.Transactional)
				{
					channel.TxCommit();
				}

				return !declaration.WaitForConfirms || WaitForConfirms();
			}
			catch
			{
				if (declaration.Transactional)
				{
					channel.TxRollback();
				}

				throw;
			}
			finally
			{
				Semaphore.Release();
			}
		}

		public async Task<bool> BatchProduce(IEnumerable<TPayload> payloads)
		{
			var batch = CreateBasicPublishBatch(payloads);

			try
			{
				await Semaphore.WaitAsync().ConfigureAwait(false);

				if (declaration.Transactional)
				{
					channel.TxSelect();
				}

				batch.Publish();

				if (declaration.Transactional)
				{
					channel.TxCommit();
				}

				return !declaration.WaitForConfirms || WaitForConfirms();
			}
			catch
			{
				if (declaration.Transactional)
				{
					channel.TxRollback();
				}

				throw;
			}
			finally
			{
				Semaphore.Release();
			}
		}

		private IBasicPublishBatch CreateBasicPublishBatch(IEnumerable<TPayload> payloads)
		{
			var batch = channel.CreateBasicPublishBatch();

			foreach (var payload in payloads)
			{
				batch.Add(
					declaration.ExchangeName,
					declaration.RoutingKey ?? declaration.ExchangeName,
					declaration.Mandatory,
					properties,
					serializer.Serialize(payload));
			}

			return batch;
		}

		private bool WaitForConfirms()
		{
			if (declaration.Die)
			{
				if (declaration.Timeout is null)
				{
					channel.WaitForConfirmsOrDie();
				}
				else
				{
					channel.WaitForConfirmsOrDie(declaration.Timeout.Value);
				}

				return true;
			}

			return declaration.Timeout is null
				? channel.WaitForConfirms()
				: channel.WaitForConfirms(declaration.Timeout.Value);
		}
	}
}