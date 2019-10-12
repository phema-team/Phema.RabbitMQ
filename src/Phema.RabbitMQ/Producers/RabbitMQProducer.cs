using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducer
	{
		ValueTask<bool> Publish<TPayload>(
			TPayload payload,
			Action<IRabbitMQProducerBuilder<TPayload>> overrides = null);

		ValueTask<bool> PublishBatch<TPayload>(
			IEnumerable<TPayload> payloads,
			Action<IRabbitMQProducerBuilder<TPayload>> overrides = null);
	}

	internal sealed class RabbitMQProducer : IRabbitMQProducer
	{
		private readonly RabbitMQOptions options;
		private readonly IRabbitMQChannelProvider channelProvider;

		public RabbitMQProducer(
			IOptions<RabbitMQOptions> options,
			IRabbitMQChannelProvider channelProvider)
		{
			this.options = options.Value;
			this.channelProvider = channelProvider;
		}

		public async ValueTask<bool> Publish<TPayload>(
			TPayload payload,
			Action<IRabbitMQProducerBuilder<TPayload>> overrides = null)
		{
			var declaration = GetDeclaration(overrides);
			var channel = await channelProvider.FromDeclaration(declaration);

			try
			{
				channel.BasicPublish(declaration, options.Serializer(payload));

				if (declaration.Transactional)
				{
					channel.TxCommit();
				}

				if (declaration.WaitForConfirms)
				{
					return channel.WaitForConfirms(declaration);
				}

				return true;
			}
			catch
			{
				if (declaration.Transactional)
				{
					channel.TxRollback();
				}

				throw;
			}
		}

		public async ValueTask<bool> PublishBatch<TPayload>(
			IEnumerable<TPayload> payloads,
			Action<IRabbitMQProducerBuilder<TPayload>> overrides = null)
		{
			var declaration = GetDeclaration(overrides);
			var channel = await channelProvider.FromDeclaration(declaration);

			var batch = channel.CreateBasicPublishBatch(
				declaration,
				payloads.Select(payload => options.Serializer(payload)));

			try
			{
				batch.Publish();

				if (declaration.Transactional)
				{
					channel.TxCommit();
				}

				if (declaration.WaitForConfirms)
				{
					return channel.WaitForConfirms(declaration);
				}

				return true;
			}
			catch
			{
				if (declaration.Transactional)
				{
					channel.TxRollback();
				}

				throw;
			}
		}

		private RabbitMQProducerDeclaration GetDeclaration<TPayload>(Action<IRabbitMQProducerBuilder<TPayload>> overrides)
		{
			var producerDeclaration = options.ProducerDeclarations.TryGetValue(typeof(TPayload), out var declaration)
				? declaration
				: throw new RabbitMQMissingDeclarationException(typeof(TPayload));

			if (overrides != null)
			{
				producerDeclaration = RabbitMQProducerDeclaration.FromDeclaration(producerDeclaration);
				overrides(new RabbitMQProducerBuilder<TPayload>(producerDeclaration));
			}

			return producerDeclaration;
		}
	}
}