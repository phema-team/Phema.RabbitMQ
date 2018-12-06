using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Phema.Rabbit
{
	public interface IConsumersConfiguration
	{
		IConsumersConfiguration AddConsumer<TPayload, TRabbitConsumer, TRabbitQueue>()
			where TRabbitConsumer : RabbitConsumer<TPayload>
			where TRabbitQueue : RabbitQueue<TPayload>;
	}

	internal class ConsumersConfiguration : IConsumersConfiguration
	{
		private readonly IServiceCollection services;

		public ConsumersConfiguration(IServiceCollection services)
		{
			this.services = services;
		}

		public IConsumersConfiguration AddConsumer<TPayload, TRabbitConsumer, TRabbitQueue>()
			where TRabbitConsumer : RabbitConsumer<TPayload>
			where TRabbitQueue : RabbitQueue<TPayload>
		{
			services.TryAddScoped<TRabbitConsumer>();
			services.TryAddSingleton<TRabbitQueue>();

			services.Configure<RabbitOptions>(options =>
				options.ConsumerActions.Add((provider, connection) =>
				{
					var consumer = provider.GetRequiredService<TRabbitConsumer>();

					var channel = connection.CreateModel();

					var queue = provider.GetRequiredService<TRabbitQueue>();
						
					channel.QueueDeclare(
						queue: queue.Name,
						durable: queue.Durable,
						exclusive: queue.Exclusive,
						autoDelete: queue.AutoDelete,
						arguments: queue.Arguments);

					if (consumer.Prefetch != null)
					{
						channel.BasicQos(0, consumer.Prefetch.Value, false);
					}
					
					for (var index = 0; index < consumer.Parallelism; index++)
					{
						channel.BasicConsume(
							queue: queue.Name,
							autoAck: consumer.AutoAck,
							consumerTag: $"{consumer.Name}_{index}",
							noLocal: consumer.NoLocal,
							exclusive: consumer.Exclusive,
							arguments: consumer.Arguments,
							consumer: new RabbitBasicConsumer<TPayload, TRabbitConsumer>(provider, channel));
					}
				}));

			return this;
		}
	}
}