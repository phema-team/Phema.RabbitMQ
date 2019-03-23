using System;
using System.Linq;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;
using RabbitMQ.Client.Impl;
using RabbitMQ.Client.Exceptions;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConsumerGroupBuilder
	{
		/// <summary>
		///   Register new consumer
		/// </summary>
		IRabbitMQConsumerBuilder AddConsumer<TPayload, TPayloadConsumer>(string queueName)
			where TPayloadConsumer : class, IRabbitMQConsumer<TPayload>;
	}

	internal sealed class RabbitMQConsumerGroupBuilder : IRabbitMQConsumerGroupBuilder
	{
		private readonly IConnection connection;
		private readonly IServiceCollection services;

		public RabbitMQConsumerGroupBuilder(
			IServiceCollection services,
			IConnection connection)
		{
			this.services = services;
			this.connection = connection;
		}

		public IRabbitMQConsumerBuilder AddConsumer<TPayload, TPayloadConsumer>(string queueName)
			where TPayloadConsumer : class, IRabbitMQConsumer<TPayload>
		{
			if (queueName is null)
				throw new ArgumentNullException(nameof(queueName));

			services.TryAddScoped<TPayloadConsumer>();

			var declaration = new RabbitMQConsumerDeclaration(queueName) as IRabbitMQConsumerDeclaration;

			services.Configure<RabbitMQConsumersOptions>(options =>
			{
				options.ConsumerDispatchers.Add(provider =>
				{
					var channel = (IFullModel) connection.CreateModel();

					var queue = provider.GetRequiredService<IOptions<RabbitMQQueuesOptions>>()
						.Value
						.Queues
						.FirstOrDefault(q => q.Name == declaration.QueueName);

					if (queue is null)
					{
						EnsureQueueDeclared(channel, declaration.QueueName);
					}
					else
					{
						if (queue.Purged)
						{
							EnsureQueuePurged(channel, queue);
						}

						if (queue.Deleted)
						{
							EnsureQueueDeleted(channel, queue);
						}

						else
						{
							DeclareQueue(channel, queue);
						}
					}

					EnsurePrefetchCount(channel, declaration);

					var factory = provider.GetRequiredService<IRabbitMQConsumerFactory>();

					for (var index = 0; index < declaration.Count; index++)
					{
						channel.BasicConsume(
							queue: declaration.QueueName,
							autoAck: declaration.AutoAck,
							consumerTag: $"{declaration.Tag}_{index}",
							noLocal: declaration.NoLocal,
							exclusive: declaration.Exclusive,
							arguments: declaration.Arguments,
							consumer: factory.CreateConsumer<TPayload, TPayloadConsumer>(channel, declaration));
					}
				});
			});

			return new RabbitMQConsumerBuilder(declaration);
		}

		private static void EnsureQueueDeclared(IModel channel, string queueName)
		{
			try
			{
				channel.QueueDeclarePassive(queueName);
			}
			catch (OperationInterruptedException exception)
			{
				throw new RabbitMQConsumerException(
					$"Queue '{queueName}' does not declared in broker",
					exception);
			}
		}

		private static void EnsureQueueDeleted(IFullModel channel, IRabbitMQQueueDeclaration queue)
		{
			try
			{
				channel._Private_QueueDelete(queue.Name, queue.IfUnused, queue.IfEmpty, queue.NoWait);
			}
			catch (OperationInterruptedException) when (queue.IfUnused || queue.IfEmpty)
			{
				// RabbitMQ.Client does not ignore PRECONDITION_FAILED
				// Means that queue is used or not empty, so just ignore exception
			}
		}

		private static void DeclareQueue(IModel channel, IRabbitMQQueueDeclaration queue)
		{
			if (queue.NoWait)
			{
				channel.QueueDeclareNoWait(
					queue: queue.Name,
					durable: queue.Durable,
					exclusive: queue.Exclusive,
					autoDelete: queue.AutoDelete,
					arguments: queue.Arguments);
			}
			else
			{
				channel.QueueDeclare(
					queue: queue.Name,
					durable: queue.Durable,
					exclusive: queue.Exclusive,
					autoDelete: queue.AutoDelete,
					arguments: queue.Arguments);
			}
		}

		private static void EnsureQueuePurged(IFullModel channel, IRabbitMQQueueDeclaration queue)
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

		private static void EnsurePrefetchCount(IModel channel, IRabbitMQConsumerDeclaration consumer)
		{
			// PrefetchSize != 0 is not implemented for now
			channel.BasicQos(0, consumer.PrefetchCount, consumer.Global);
		}
	}
}