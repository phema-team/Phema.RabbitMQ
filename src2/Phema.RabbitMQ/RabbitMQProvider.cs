using System;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Impl;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProvider
	{
		IRabbitMQProvider Consumer<TPayload, TPayloadConsumer>(string queueName,
			Action<IRabbitMQConsumerProvider> consumer = null)
			where TPayloadConsumer : IRabbitMQConsumer<TPayload>;

		IRabbitMQProvider Exchange(string type, string exchangeName, Action<IRabbitMQExchangeProvider> exchange = null);

		IRabbitMQProvider Producer<TPayload>(string exchangeName,
			string queueName,
			Action<IRabbitMQProducerProvider> producer = null);

		IRabbitMQProvider Queue(string queueName, Action<IRabbitMQQueueProvider> queue = null);

		// IRabbitMQProvider ConsumerGroup(string groupName, Action<IRabbitMQConsumerGroupProvider> group);
		//IRabbitMQProvider ExchangeGroup(string groupName, Action<IRabbitMQExchangeGroupProvider> group);
		//IRabbitMQProvider ProducerGroup(string groupName, Action<IRabbitMQProducerGroupProvider> group);
		//IRabbitMQProvider QueueGroup(string groupName, Action<IRabbitMQQueueGroupProvider> group);
	}

	internal sealed class RabbitMQProvider : IRabbitMQProvider
	{
		private readonly IRabbitMQConnectionFactory connectionFactory;
		private readonly IRabbitMQConsumerFactory consumerFactory;
		private readonly RabbitMQProducerOptions options;

		public RabbitMQProvider(
			IRabbitMQConnectionFactory connectionFactory,
			IRabbitMQConsumerFactory consumerFactory,
			IOptions<RabbitMQProducerOptions> options)
		{
			this.connectionFactory = connectionFactory;
			this.consumerFactory = consumerFactory;
			this.options = options.Value;
		}


		public IRabbitMQProvider Consumer<TPayload, TPayloadConsumer>(string queueName,
			Action<IRabbitMQConsumerProvider> consumer) where TPayloadConsumer : IRabbitMQConsumer<TPayload>
		{
			if (queueName is null)
				throw new ArgumentNullException(nameof(queueName));

			var metadata = new RabbitMQConsumerMetadata(queueName);

			consumer?.Invoke(new RabbitMQConsumerProvider(metadata));

			var channel = (IFullModel) connectionFactory.CreateConnection(queueName).CreateModel();

			EnsurePrefetchCount(channel, metadata);

			for (var index = 0; index < metadata.Count; index++)
			{
				channel.BasicConsume(
					queue: metadata.QueueName,
					autoAck: metadata.AutoAck,
					consumerTag: metadata.Tag,
					noLocal: metadata.NoLocal,
					exclusive: metadata.Exclusive,
					arguments: metadata.Arguments,
					consumer: consumerFactory.CreateConsumer<TPayload, TPayloadConsumer>(channel, metadata));
			}

			return this;

			static void EnsurePrefetchCount(IModel channel, IRabbitMQConsumerMetadata metadata)
			{
				// PrefetchSize != 0 is not implemented for now
				channel.BasicQos(0, metadata.PrefetchCount, metadata.Global);
			}
		}

		public IRabbitMQProvider Exchange(string type, string exchangeName, Action<IRabbitMQExchangeProvider> exchange)
		{
			if (exchangeName is null)
				throw new ArgumentNullException(nameof(exchangeName));

			var metadata = new RabbitMQExchangeMetadata(type, exchangeName);

			exchange?.Invoke(new RabbitMQExchangeProvider(metadata));

			using (var channel = (IFullModel) connectionFactory.CreateConnection(exchangeName).CreateModel())
			{
				if (metadata.Deleted)
				{
					EnsureExchangeDeleted(channel, metadata);
				}
				else
				{
					DeclareExchange(channel, metadata);

					// Should i move bindings to bindinds provider?
					foreach (var binding in metadata.ExchangeBindings)
					{
						DeclareExchangeBinding(channel, metadata, binding);
					}
				}
			}

			return this;

			static void EnsureExchangeDeleted(IFullModel channel, IRabbitMQExchangeMetadata exchange)
			{
				channel._Private_ExchangeDelete(exchange.Name, exchange.IfUnused, exchange.NoWait);
			}

			static void DeclareExchange(IFullModel channel, IRabbitMQExchangeMetadata exchange)
			{
				channel._Private_ExchangeDeclare(
					exchange: exchange.Name,
					type: exchange.Type,
					passive: false,
					durable: exchange.Durable,
					autoDelete: exchange.AutoDelete,
					@internal: exchange.Internal,
					nowait: exchange.NoWait,
					arguments: exchange.Arguments);
			}

			static void DeclareExchangeBinding(
				IFullModel channel,
				IRabbitMQExchangeMetadata exchange,
				IRabbitMQExchangeBindingMetadata binding)
			{
				channel._Private_ExchangeBind(
					destination: binding.ExchangeName,
					source: exchange.Name,
					routingKey: binding.RoutingKey ?? binding.ExchangeName,
					nowait: binding.NoWait,
					arguments: binding.Arguments);
			}
		}

		public IRabbitMQProvider Producer<TPayload>(string exchangeName,
			string queueName,
			Action<IRabbitMQProducerProvider> producer)
		{
			if (queueName is null)
				throw new ArgumentNullException(nameof(queueName));

			var metadata = new RabbitMQProducerMetadata(exchangeName, queueName);

			producer?.Invoke(new RabbitMQProducerProvider(metadata));

			var channel = (IFullModel) connectionFactory.CreateConnection(exchangeName).CreateModel();

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

			static void BindQueue(IModel channel, IRabbitMQProducerMetadata producer)
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

			static void EnsureRoutingKeyOrQueueName(IRabbitMQProducerMetadata producer)
			{
				if ((producer.RoutingKey ?? producer.QueueName) is null)
				{
					throw new RabbitMQProducerException(
						$"'{nameof(producer.RoutingKey)}' or '{nameof(producer.QueueName)}' for producer should be declared");
				}
			}

			static IBasicProperties CreateBasicProperties(IModel channel, IRabbitMQProducerMetadata producer)
			{
				var properties = channel.CreateBasicProperties();

				foreach (var property in producer.Properties)
				{
					property(properties);
				}

				return properties;
			}
		}

		public IRabbitMQProvider Queue(string queueName, Action<IRabbitMQQueueProvider> queue)
		{
			if (queueName is null)
				throw new ArgumentNullException(nameof(queueName));

			var metadata = new RabbitMQQueueMetadata(queueName);

			queue?.Invoke(new RabbitMQQueueProvider(metadata));

			using (var channel = (IFullModel) connectionFactory.CreateConnection(queueName).CreateModel())
			{
				if (metadata.Purged)
				{
					EnsureQueuePurged(channel, metadata);
				}

				if (metadata.Deleted)
				{
					EnsureQueueDeleted(channel, metadata);
				}
				else
				{
					DeclareQueue(channel, metadata);
				}
			}

			return this;

			static void EnsureQueuePurged(IFullModel channel, IRabbitMQQueueMetadata queue)
			{
				try
				{
					channel._Private_QueuePurge(queue.Name, queue.NoWait);
				}
				catch (OperationInterruptedException)
				{
					// Means that queue does not declared, so just ignore exception
				}
			}

			static void EnsureQueueDeleted(IFullModel channel, IRabbitMQQueueMetadata metadata)
			{
				try
				{
					channel._Private_QueueDelete(metadata.Name, metadata.IfUnused, metadata.IfEmpty, metadata.NoWait);
				}
				catch (OperationInterruptedException) when (metadata.IfUnused || metadata.IfEmpty)
				{
					// RabbitMQ.Client does not ignore PRECONDITION_FAILED
					// Means that queue is used or not empty, so just ignore exception
				}
			}

			static void DeclareQueue(IModel channel, IRabbitMQQueueMetadata metadata)
			{
				if (metadata.NoWait)
				{
					channel.QueueDeclareNoWait(
						queue: metadata.Name,
						durable: metadata.Durable,
						exclusive: metadata.Exclusive,
						autoDelete: metadata.AutoDelete,
						arguments: metadata.Arguments);
				}
				else
				{
					channel.QueueDeclare(
						queue: metadata.Name,
						durable: metadata.Durable,
						exclusive: metadata.Exclusive,
						autoDelete: metadata.AutoDelete,
						arguments: metadata.Arguments);
				}
			}
		}
	}
}