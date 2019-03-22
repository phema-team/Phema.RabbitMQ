using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Impl;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducerGroupProvider
	{
		IRabbitMQProducerGroupProvider Producer<TPayload>(string exchangeName, string queueName, Action<IRabbitMQProducerProvider> producer);
	}
	
	internal sealed class RabbitMQProducerGroupProvider : IRabbitMQProducerGroupProvider
	{
		private readonly IConnection connection;
		private readonly RabbitMQProducerOptions options;

		public RabbitMQProducerGroupProvider(IConnection connection, RabbitMQProducerOptions options)
		{
			this.options = options;
			this.connection = connection;
		}

		public IRabbitMQProducerGroupProvider Producer<TPayload>(string exchangeName, string queueName, Action<IRabbitMQProducerProvider> producer)
		{
			if (queueName is null)
				throw new ArgumentNullException(nameof(queueName));

			var metadata = new RabbitMQProducerMetadata(exchangeName, queueName);

			producer.Invoke(new RabbitMQProducerProvider(metadata));

			var channel = (IFullModel) connection.CreateModel();
			
			if (metadata.QueueName != null)
			{
				// Should bind queue with exchange when not declared,
				// because of default or already declared
				BindQueue(channel, metadata);
			}

			EnsureRoutingKeyOrQueueName(metadata);

			if (metadata.WaitForConfirms)
			{
				channel.ConfirmSelect();
			}

			options.Producers[typeof(TPayload)] = new RabbitMQProducerEntry
			{
				Channel = channel,
				Metadata = metadata,
				Properties = CreateBasicProperties(channel, metadata)
			};

			return this;
		}
		
		private static void BindQueue(IModel channel, IRabbitMQProducerMetadata producer)
		{
			try
			{
				channel.QueueBind(
					queue: producer.QueueName,
					exchange: producer.ExchangeName,
					routingKey: producer.RoutingKey ?? producer.QueueName,
					arguments: producer.Arguments);
			}
			catch (OperationInterruptedException exception)
			{
				throw new RabbitMQProducerException(
					$"Exchange '{producer.ExchangeName}' or queue '{producer.QueueName}' does not declared in broker",
					exception);
			}
		}

		private static void EnsureRoutingKeyOrQueueName(IRabbitMQProducerMetadata producer)
		{
			if ((producer.RoutingKey ?? producer.QueueName) is null)
			{
				throw new RabbitMQProducerException(
					$"'{nameof(producer.RoutingKey)}' or '{nameof(producer.QueueName)}' for producer should be declared");
			}
		}

		private static IBasicProperties CreateBasicProperties(IModel channel, IRabbitMQProducerMetadata producer)
		{
			var properties = channel.CreateBasicProperties();
			
			foreach (var property in producer.Properties)
			{
				property(properties);
			}

			return properties;
		}
	}
}