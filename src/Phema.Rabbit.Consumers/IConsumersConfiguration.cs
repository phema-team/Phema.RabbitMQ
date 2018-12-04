using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

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
			services.TryAddSingleton<TRabbitConsumer>();
			services.TryAddSingleton<TRabbitQueue>();

			services.Configure<RabbitOptions>(options =>
				options.Actions.Add((provider, connection) =>
				{
					var consumer = provider.GetRequiredService<TRabbitConsumer>();

					for (var index = 0; index < consumer.Parallelism; index++)
					{
						var model = connection.CreateModel();

						var queue = provider.GetRequiredService<TRabbitQueue>();
						model.QueueDeclare(
							queue: queue.Name,
							durable: queue.Durable,
							exclusive: queue.Exclusive,
							autoDelete: queue.AutoDelete,
							arguments: queue.Arguments);
						
						var handler = new AsyncEventingBasicConsumer(model);

						handler.Received += (sender, args) =>
						{
							using (var scope = provider.CreateScope())
							{
								return BasicConsume<TPayload, TRabbitConsumer>(scope.ServiceProvider, args);
							}
						};

						model.BasicConsume(
							queue: queue.Name,
							autoAck: consumer.AutoAck,
							consumerTag: $"{consumer.Tag}_{index}",
							noLocal: consumer.NoLocal,
							exclusive: consumer.Exclusive,
							arguments: consumer.Arguments,
							consumer: handler);
					}
				}));

			return this;
		}

		private static Task BasicConsume<TPayload, TRabbitConsumer>(IServiceProvider provider, BasicDeliverEventArgs args)
			where TRabbitConsumer : RabbitConsumer<TPayload>
		{
			var consumer = provider.GetRequiredService<TRabbitConsumer>();
			var options = provider.GetRequiredService<IOptions<RabbitOptions>>().Value;

			var model = JsonConvert.DeserializeObject<TPayload>(options.Encoding.GetString(args.Body));

			return consumer.Consume(model);
		}
	}
}