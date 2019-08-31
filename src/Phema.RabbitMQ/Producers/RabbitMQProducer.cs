using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

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

		public ValueTask<bool> Publish<TPayload>(
			TPayload payload,
			Action<IRabbitMQProducerBuilder<TPayload>> overrides = null)
		{
			var declaration = GetDeclaration(overrides);
			var channel = FromDeclaration(declaration);
			var properties = CreateBasicProperties(channel, declaration);

			var routingKey = declaration.RoutingKey ?? declaration.Exchange.Name;

			try
			{
				channel.BasicPublish(
					declaration.Exchange.Name,
					routingKey,
					declaration.Mandatory,
					properties,
					options.Serializer(payload));

				if (declaration.Transactional)
				{
					channel.TxCommit();
				}

				return new ValueTask<bool>(!declaration.WaitForConfirms || WaitForConfirms(channel, declaration));
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

		public ValueTask<bool> PublishBatch<TPayload>(
			IEnumerable<TPayload> payloads,
			Action<IRabbitMQProducerBuilder<TPayload>> overrides = null)
		{
			var declaration = GetDeclaration(overrides);
			var channel = FromDeclaration(declaration);
			var batch = CreateBasicPublishBatch(channel, declaration, payloads);

			try
			{
				batch.Publish();

				if (declaration.Transactional)
				{
					channel.TxCommit();
				}

				return new ValueTask<bool>(!declaration.WaitForConfirms || WaitForConfirms(channel, declaration));
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

		private static bool WaitForConfirms(IModel channel, RabbitMQProducerDeclaration declaration)
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

		private RabbitMQProducerDeclaration GetDeclaration<TPayload>(Action<IRabbitMQProducerBuilder<TPayload>> overrides)
		{
			// TODO: Dictionary?
			var declaration = options.ProducerDeclarations.First(d => d.Type == typeof(TPayload));

			if (overrides != null)
			{
				declaration = RabbitMQProducerDeclaration.FromDeclaration(declaration);
				overrides(new RabbitMQProducerBuilder<TPayload>(declaration));
			}

			return declaration;
		}

		private IModel FromDeclaration(RabbitMQProducerDeclaration declaration)
		{
			var channel = channelProvider.FromDeclaration(declaration);

			if (declaration.WaitForConfirms)
			{
				channel.ConfirmSelect();
			}

			if (declaration.Transactional)
			{
				channel.TxSelect();
			}

			return channel;
		}

		private IBasicPublishBatch CreateBasicPublishBatch<TPayload>(
			IModel channel,
			RabbitMQProducerDeclaration declaration,
			IEnumerable<TPayload> payloads)
		{
			var batch = channel.CreateBasicPublishBatch();
			var properties = CreateBasicProperties(channel, declaration);

			var routingKey = declaration.RoutingKey ?? declaration.Exchange.Name;

			foreach (var payload in payloads)
			{
				batch.Add(
					declaration.Exchange.Name,
					routingKey,
					declaration.Mandatory,
					properties,
					options.Serializer(payload));
			}

			return batch;
		}

		private static IBasicProperties CreateBasicProperties(IModel channel, RabbitMQProducerDeclaration declaration)
		{
			var properties = channel.CreateBasicProperties();

			foreach (var property in declaration.Properties)
			{
				property(properties);
			}

			return properties;
		}
	}
}