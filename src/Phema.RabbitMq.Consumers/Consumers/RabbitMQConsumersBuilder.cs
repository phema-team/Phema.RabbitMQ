using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Impl;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConsumersBuilder
	{
		/// <summary>
		///   Register new consumer
		/// </summary>
		IRabbitMQConsumerBuilder AddConsumer<TPayload, TPayloadConsumer>(string queueName)
			where TPayloadConsumer : class, IRabbitMQConsumer<TPayload>;
	}

	internal sealed class RabbitMQConsumersBuilder : IRabbitMQConsumersBuilder
	{
		private readonly IConnection connection;
		private readonly IServiceCollection services;

		public RabbitMQConsumersBuilder(
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

			var metadata = new RabbitMQConsumerMetadata(queueName) as IRabbitMQConsumerMetadata;

			services.Configure<RabbitMQConsumersOptions>(options =>
			{
				options.ConsumerDispatchers.Add(provider =>
				{
					var channel = (IFullModel) connection.CreateModel();

					var queue = provider.GetRequiredService<IOptions<RabbitMQQueuesOptions>>()
						.Value
						.Queues
						.FirstOrDefault(q => q.Name == metadata.QueueName);

					if (queue == null)
					{
						// Ensure it declared in broker
						try
						{
							channel.QueueDeclarePassive(metadata.QueueName);
						}
						catch (OperationInterruptedException exception)
						{
							throw new RabbitMQConsumerException(
								$"Queue '{metadata.QueueName}' does not declared in broker",
								exception);
						}
					}
					else
					{
						if (queue.NoWait)
						{
							channel.QueueDeclareNoWait(
								queue: metadata.QueueName,
								durable: queue.Durable,
								exclusive: queue.Exclusive,
								autoDelete: queue.AutoDelete,
								arguments: queue.Arguments);
						}
						else
						{
							channel.QueueDeclare(
								queue: metadata.QueueName,
								durable: queue.Durable,
								exclusive: queue.Exclusive,
								autoDelete: queue.AutoDelete,
								arguments: queue.Arguments);
						}
					}

					// PrefetchSize != 0 is not implemented for now
					channel.BasicQos(0, metadata.PrefetchCount, metadata.Global);

					var factory = provider.GetRequiredService<IRabbitMQConsumerFactory>();

					for (var index = 0; index < metadata.Count; index++)
					{
						channel.BasicConsume(
							queue: metadata.QueueName,
							metadata.AutoAck,
							$"{metadata.Tag}_{index}",
							metadata.NoLocal,
							metadata.Exclusive,
							metadata.Arguments,
							factory.CreateConsumer<TPayload, TPayloadConsumer>(channel, metadata));
					}
				});
			});

			return new RabbitMQConsumerBuilder(metadata);
		}
	}
}