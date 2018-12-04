using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	public interface IProducersConfiguration
	{
		IProducersConfiguration AddProducer<TPayload, TRabbitProducer, TRabbitQueue, TRabbitExchange>()
			where TRabbitProducer : RabbitProducer<TPayload>
			where TRabbitQueue : RabbitQueue<TPayload>
			where TRabbitExchange : RabbitExchange;
	}
	
	internal sealed class ProducersConfiguration : IProducersConfiguration
	{
		private readonly IServiceCollection services;

		public ProducersConfiguration(IServiceCollection services)
		{
			this.services = services;
		}
		
		public IProducersConfiguration AddProducer<TPayload, TRabbitProducer, TRabbitQueue, TRabbitExchange>() 
			where TRabbitProducer : RabbitProducer<TPayload>
			where TRabbitQueue : RabbitQueue<TPayload>
			where TRabbitExchange : RabbitExchange
		{
			services.TryAddSingleton<TRabbitProducer>();
			services.TryAddSingleton<TRabbitQueue>();
			services.TryAddSingleton<TRabbitExchange>();

			services.Configure<RabbitOptions>(rabbit =>
				rabbit.Actions.Add((provider, connection) =>
				{
					var model = connection.CreateModel();

					var exchange = provider.GetRequiredService<TRabbitExchange>();
					
					model.ExchangeDeclare(
						exchange: exchange.Name,
						type: exchange.Type,
						durable: exchange.Durable,
						autoDelete: exchange.AutoDelete,
						arguments: exchange.Arguments);
					
					var queue = provider.GetRequiredService<TRabbitQueue>();

					model.QueueDeclare(
						queue: queue.Name,
						durable: queue.Durable,
						exclusive: queue.Exclusive,
						autoDelete: queue.AutoDelete,
						arguments: queue.Arguments);
					
					model.QueueBind(
						queue: queue.Name,
						exchange: exchange.Name,
						routingKey: queue.Name,
						arguments: queue.Arguments);

					var producer = provider.GetRequiredService<TRabbitProducer>();
					var options = provider.GetRequiredService<IOptions<RabbitOptions>>().Value;

					producer.ProduceAction = payload =>
						model.BasicPublish(
							exchange.Name,
							queue.Name,
							producer.Properties,
							options.Encoding.GetBytes(JsonConvert.SerializeObject(payload)));
				}));
			
			return this;
		}
	}
}