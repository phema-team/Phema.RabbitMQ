using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Impl;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConsumerGroupProvider
	{
		IRabbitMQConsumerGroupProvider Consumer<TPayload, TPayloadConsumer>(string queueName,
			Action<IRabbitMQConsumerProvider> consumer)
			where TPayloadConsumer : IRabbitMQConsumer<TPayload>;
	}

	internal sealed class RabbitMQConsumerGroupProvider : IRabbitMQConsumerGroupProvider
	{
		private readonly IConnection connection;
		private readonly IRabbitMQConsumerFactory factory;

		public RabbitMQConsumerGroupProvider(IConnection connection, IRabbitMQConsumerFactory factory)
		{
			this.connection = connection;
			this.factory = factory;
		}

		public IRabbitMQConsumerGroupProvider Consumer<TPayload, TPayloadConsumer>(string queueName,
			Action<IRabbitMQConsumerProvider> consumer)
			where TPayloadConsumer : IRabbitMQConsumer<TPayload>
		{
			if (queueName is null)
				throw new ArgumentNullException(nameof(queueName));

			var metadata = new RabbitMQConsumerMetadata(queueName);

			consumer.Invoke(new RabbitMQConsumerProvider(metadata));

			var channel = (IFullModel) connection.CreateModel();

			EnsurePrefetchCount(channel, metadata);

			if (metadata.Canceled)
			{
				channel.BasicCancel(metadata.Tag);
			}
			else
			{
				for (var index = 0; index < metadata.Count; index++)
				{
					channel.BasicConsume(
						queue: metadata.QueueName,
						autoAck: metadata.AutoAck,
						consumerTag: metadata.Tag,
						noLocal: metadata.NoLocal,
						exclusive: metadata.Exclusive,
						arguments: metadata.Arguments,
						consumer: factory.CreateConsumer<TPayload, TPayloadConsumer>(channel, metadata));
				}
			}

			return this;
		}

		private static void EnsurePrefetchCount(IModel channel, IRabbitMQConsumerMetadata consumer)
		{
			// PrefetchSize != 0 is not implemented for now
			channel.BasicQos(0, consumer.PrefetchCount, consumer.Global);
		}
	}
}