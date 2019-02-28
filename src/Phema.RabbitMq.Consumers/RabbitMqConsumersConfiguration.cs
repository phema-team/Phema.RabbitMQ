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
				options.Consumers.Add(consumer);
				
				options.ConsumerDispatchers.Add(provider =>
				{
					var queue = provider.GetRequiredService<IOptions<RabbitMqQueuesOptions>>()
						.Value
						.Queues
						.First(q => q.Name == consumer.QueueName);
					
					var channel = provider.GetRequiredService<IModel>();
					
					channel.QueueDeclareNoWait(
						queue: queue.Name,
						durable: queue.Durable,
						exclusive: queue.Exclusive,
						autoDelete: queue.AutoDelete,
						arguments: queue.Arguments);

					channel.BasicQos(0, consumer.Prefetch, global: false);
					
					for (var index = 0; index < consumer.Consumers; index++)
					{
						channel.BasicConsume(
							queue: queue.Name,
							autoAck: consumer.AutoAck,
							consumerTag: $"{consumer.Tag}_{index}",
							noLocal: consumer.NoLocal,
							exclusive: consumer.Exclusive,
							arguments: consumer.Arguments,
							consumer: new RabbitMqBasicConsumer<TPayload, TPayloadConsumer>(provider, channel, consumer));
					}
				});
			});

			return new RabbitMqConsumerConsiguration(consumer);
		}
	}
}