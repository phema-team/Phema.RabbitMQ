using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	public interface IProducersConfiguration<TRabbitExchange>
		where TRabbitExchange : RabbitExchange
	{
		IProducersConfiguration<TRabbitExchange> AddProducer<TPayload, TRabbitProducer, TRabbitQueue>()
			where TRabbitProducer : RabbitProducer<TPayload>
			where TRabbitQueue : RabbitQueue<TPayload>;
	}
	
	internal sealed class ProducersConfiguration<TRabbitExchange> : IProducersConfiguration<TRabbitExchange>
		where TRabbitExchange : RabbitExchange
	{
		private readonly IServiceCollection services;

		public ProducersConfiguration(IServiceCollection services)
		{
			this.services = services;
		}
		
		public IProducersConfiguration<TRabbitExchange> AddProducer<TPayload, TRabbitProducer, TRabbitQueue>() 
			where TRabbitProducer : RabbitProducer<TPayload>
			where TRabbitQueue : RabbitQueue<TPayload>
		{
			services.TryAddScoped(provider =>
			{
				var connection = provider.GetRequiredService<IConnection>();

				var channel = connection.CreateModel();

				var exchange = provider.GetRequiredService<TRabbitExchange>();

				channel.ExchangeDeclareNoWait(
					exchange: exchange.Name,
					type: exchange.Type,
					durable: exchange.Durable,
					autoDelete: exchange.AutoDelete,
					arguments: exchange.Arguments);

				var queue = provider.GetRequiredService<TRabbitQueue>();

				channel.QueueDeclareNoWait(
					queue: queue.Name,
					durable: queue.Durable,
					exclusive: queue.Exclusive,
					autoDelete: queue.AutoDelete,
					arguments: queue.Arguments);

				channel.QueueBindNoWait(
					queue: queue.Name,
					exchange: exchange.Name,
					routingKey: queue.Name,
					arguments: queue.Arguments);

				var rabbitOptions = provider.GetRequiredService<IOptions<RabbitOptions>>().Value;
				var producer = ActivatorUtilities.CreateInstance<TRabbitProducer>(provider);
				
				producer.Channel = channel;
				producer.ProduceAction = (model, payload) =>
					model.BasicPublish(
						exchange.Name,
						queue.Name,
						queue.Mandatory,
						queue.Properties,
						rabbitOptions.Encoding.GetBytes(JsonConvert.SerializeObject(payload, rabbitOptions.SerializerSettings)));

				return producer;
			});

			services.TryAddSingleton<TRabbitQueue>();
			services.TryAddSingleton<TRabbitExchange>();
			
			return this;
		}
	}
}