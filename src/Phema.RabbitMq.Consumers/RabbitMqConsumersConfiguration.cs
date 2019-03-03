using System.Linq;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;

namespace Phema.RabbitMq
{
	public interface IRabbitMqConsumersConfiguration
	{
		IRabbitMqConsumerConfiguration AddConsumer<TPayload, TPayloadConsumer>(string queueName)
			where TPayloadConsumer : class, IRabbitMqConsumer<TPayload>;
	}

	internal sealed class RabbitMqConsumersConfiguration : IRabbitMqConsumersConfiguration
	{
		private readonly IServiceCollection services;

		public RabbitMqConsumersConfiguration(IServiceCollection services)
		{
			this.services = services;
		}

		public IRabbitMqConsumerConfiguration AddConsumer<TPayload, TPayloadConsumer>(string queueName)
			where TPayloadConsumer : class, IRabbitMqConsumer<TPayload>
		{
			services.TryAddScoped<TPayloadConsumer>();

			var consumer = new RabbitMqConsumer<TPayload, TPayloadConsumer>(queueName);

			services.Configure<RabbitMqConsumersOptions>(options =>
			{
				options.ConsumerDispatchers.Add(provider =>
				{
					var queue = provider.GetRequiredService<IOptions<RabbitMqQueuesOptions>>()
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

					for (var index = 0; index < consumer.Consumers; index++)
						channel.BasicConsume(
							consumer.QueueName,
							consumer.AutoAck,
							$"{consumer.Tag}_{index}",
							consumer.NoLocal,
							consumer.Exclusive,
							consumer.Arguments,
							new RabbitMqBasicConsumer<TPayload, TPayloadConsumer>(provider, channel, consumer));
				});
			});

			return new RabbitMqConsumerConsiguration(consumer);
		}
	}
}