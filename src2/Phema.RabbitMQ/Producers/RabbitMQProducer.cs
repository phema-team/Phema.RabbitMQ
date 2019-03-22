using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Phema.Serialization;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducer<TPayload>
	{
		Task<bool> Produce(TPayload payload);

		Task<bool> BatchProduce(IEnumerable<TPayload> payloads);
	}

	internal sealed class RabbitMQProducer<TPayload> : IRabbitMQProducer<TPayload>
	{
		// Each generic type has unique semaphore, because unique channel
		private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1);

		private readonly ISerializer serializer;
		private readonly IOptions<RabbitMQProducerOptions> options;

		public RabbitMQProducer(IOptions<RabbitMQProducerOptions> options, ISerializer serializer)
		{
			this.options = options;
			this.serializer = serializer;
		}

		public async Task<bool> Produce(TPayload payload)
		{
			await Semaphore.WaitAsync().ConfigureAwait(false);

			var entry = options.Value.Producers[typeof(TPayload)];

			var metadata = entry.Metadata;
			var channel = entry.Channel;
			var properties = entry.Properties;

			try
			{
				if (metadata.Transactional)
				{
					channel.TxSelect();
				}

				channel.BasicPublish(
					metadata.ExchangeName,
					metadata.RoutingKey ?? metadata.QueueName,
					metadata.Mandatory,
					properties,
					serializer.Serialize(payload));

				return !metadata.WaitForConfirms || WaitForConfirms(channel, metadata);
			}
			catch
			{
				if (metadata.Transactional)
				{
					channel.TxRollback();
				}

				throw;
			}
			finally
			{
				if (metadata.Transactional)
				{
					channel.TxCommit();
				}

				Semaphore.Release();
			}
		}

		public async Task<bool> BatchProduce(IEnumerable<TPayload> payloads)
		{
			var entry = options.Value.Producers[typeof(TPayload)];

			var metadata = entry.Metadata;
			var channel = entry.Channel;
			var properties = entry.Properties;

			var batch = channel.CreateBasicPublishBatch();

			foreach (var payload in payloads)
			{
				batch.Add(
					metadata.ExchangeName,
					metadata.RoutingKey ?? metadata.QueueName,
					metadata.Mandatory,
					properties,
					serializer.Serialize(payload));
			}

			await Semaphore.WaitAsync().ConfigureAwait(false);

			try
			{
				if (metadata.Transactional)
				{
					channel.TxSelect();
				}

				batch.Publish();

				return !metadata.WaitForConfirms || WaitForConfirms(channel, metadata);
			}
			catch
			{
				if (metadata.Transactional)
				{
					channel.TxRollback();
				}

				throw;
			}
			finally
			{
				if (metadata.Transactional)
				{
					channel.TxCommit();
				}

				Semaphore.Release();
			}
		}

		private static bool WaitForConfirms(IModel channel, IRabbitMQProducerMetadata metadata)
		{
			if (metadata.Die)
			{
				if (metadata.Timeout is null)
				{
					channel.WaitForConfirmsOrDie();
				}
				else
				{
					channel.WaitForConfirmsOrDie(metadata.Timeout.Value);
				}

				return true;
			}

			return metadata.Timeout is null
				? channel.WaitForConfirms()
				: channel.WaitForConfirms(metadata.Timeout.Value);
		}
	}
}