using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
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
		private readonly string groupName;
		private readonly IServiceCollection services;

		public RabbitMQConsumersBuilder(
			IServiceCollection services,
			IConnection connection,
			string groupName)
		{
			this.services = services;
			this.connection = connection;
			this.groupName = groupName;
		}

		public IRabbitMQConsumerBuilder AddConsumer<TPayload, TPayloadConsumer>(string queueName)
			where TPayloadConsumer : class, IRabbitMQConsumer<TPayload>
		{
			if (queueName is null)
				throw new ArgumentNullException(nameof(queueName));

			services.TryAddScoped<TPayloadConsumer>();

			var consumer = new RabbitMQConsumerMetadata(queueName) as IRabbitMQConsumerMetadata;

			services.Configure<RabbitMQConsumersOptions>(options =>
			{
				options.ConsumerDispatchers.Add(provider =>
				{
					var channel = (IFullModel) connection.CreateModel();

					var queue = provider.GetRequiredService<IOptions<RabbitMQQueuesOptions>>()
						.Value
						.Queues
						.FirstOrDefault(q => q.Name == consumer.QueueName);

					// It can be already declared somewhere
					if (queue != null)
					{
						channel._Private_QueueDeclare(
							queue: queue.Name,
							passive: queue.Passive,
							durable: queue.Durable,
							exclusive: queue.Exclusive,
							autoDelete: queue.AutoDelete,
							nowait: queue.NoWait,
							arguments: queue.Arguments);
					}

					channel.BasicQos(consumer.PrefetchSize, consumer.PrefetchCount, consumer.Global);

					var factory = provider.GetRequiredService<IRabbitMQConsumerFactory>();

					for (var index = 0; index < consumer.Count; index++)
					{
						channel.BasicConsume(
							queue: groupName == null
								? consumer.QueueName
								: $"{groupName}_{consumer.QueueName}",
							consumer.AutoAck,
							$"{consumer.Tag}_{index}",
							consumer.NoLocal,
							consumer.Exclusive,
							consumer.Arguments,
							factory.CreateConsumer<TPayload, TPayloadConsumer>(channel, consumer));
					}
				});
			});

			return new RabbitMQConsumerBuilder(consumer);
		}
	}
}