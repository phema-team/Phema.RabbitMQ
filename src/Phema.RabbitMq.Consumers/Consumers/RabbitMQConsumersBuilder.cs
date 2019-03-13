using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConsumersBuilder
	{
		/// <summary>
		/// Add new consumer
		/// </summary>
		IRabbitMQConsumerBuilder AddConsumer<TPayload, TPayloadConsumer>(string queueName)
			where TPayloadConsumer : class, IRabbitMQConsumer<TPayload>;
	}

	internal sealed class RabbitMQConsumersBuilder : IRabbitMQConsumersBuilder
	{
		private readonly IServiceCollection services;

		public RabbitMQConsumersBuilder(IServiceCollection services)
		{
			this.services = services;
		}

		public IRabbitMQConsumerBuilder AddConsumer<TPayload, TPayloadConsumer>(string queueName)
			where TPayloadConsumer : class, IRabbitMQConsumer<TPayload>
		{
			if (queueName is null)
				throw new ArgumentNullException(nameof(queueName));

			services.TryAddScoped<TPayloadConsumer>();

			var consumer = new RabbitMQConsumerMetadata(queueName);

			services.Configure<RabbitMQConsumersOptions>(options =>
			{
				options.ConsumerDispatchers.Add(provider =>
				{
					var queue = provider.GetRequiredService<IOptions<RabbitMQQueuesOptions>>()
						.Value
						.Queues
						.FirstOrDefault(q => q.Name == consumer.QueueName);

					var channel = provider.GetRequiredService<IConnection>().CreateModel();

					if (queue != null)
						channel.QueueDeclareNoWait(
							queue.Name,
							queue.Durable,
							queue.Exclusive,
							queue.AutoDelete,
							queue.Arguments);

					channel.BasicQos(0, consumer.Prefetch, false);

					for (var index = 0; index < consumer.Count; index++)
						channel.BasicConsume(
							consumer.QueueName,
							consumer.AutoAck,
							$"{consumer.Tag}_{index}",
							consumer.NoLocal,
							consumer.Exclusive,
							consumer.Arguments,
							new RabbitMQBasicConsumer<TPayload, TPayloadConsumer>(provider, channel, consumer));
				});
			});

			return new RabbitMQConsumerBuilder(consumer);
		}
	}
}