using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	public interface IProducersConfiguration<TPayload, TRabbitExchange>
		where TRabbitExchange : FanoutRabbitExchange<TPayload>
	{
		IProducersConfiguration<TRabbitExchange> AddProducer<TRabbitProducer, TRabbitQueue>()
			where TRabbitProducer : RabbitProducer<TPayload>
			where TRabbitQueue : RabbitQueue<TPayload>;
	}
	
	public interface IProducersConfiguration<TRabbitExchange>
		where TRabbitExchange : RabbitExchange
	{
		IProducersConfiguration<TRabbitExchange> AddProducer<TPayload, TRabbitProducer, TRabbitQueue>()
			where TRabbitProducer : RabbitProducer<TPayload>
			where TRabbitQueue : RabbitQueue<TPayload>;
	}
	
	internal sealed class ProducersConfiguration<TPayload, TFanoutRabbitExchange> 
		: IProducersConfiguration<TPayload, TFanoutRabbitExchange>
		where TFanoutRabbitExchange : FanoutRabbitExchange<TPayload>
	{
		private readonly IProducersConfiguration<TFanoutRabbitExchange> configuration;

		public ProducersConfiguration(IProducersConfiguration<TFanoutRabbitExchange> configuration)
		{
			this.configuration = configuration;
		}

		public IProducersConfiguration<TFanoutRabbitExchange> AddProducer<TRabbitProducer, TRabbitQueue>()
			where TRabbitProducer : RabbitProducer<TPayload>
			where TRabbitQueue : RabbitQueue<TPayload>
		{
			return configuration.AddProducer<TPayload, TRabbitProducer, TRabbitQueue>();
		}
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

				// TODO: Do i need to cache IModel for TRabbitProducer?
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

				var options = provider.GetRequiredService<IOptions<RabbitOptions>>().Value;
				var producer = ActivatorUtilities.CreateInstance<TRabbitProducer>(provider);
				
				producer.Channel = channel;
				producer.Exchange = exchange;
				producer.Queue = queue;
				producer.Options = options;

				return producer;
			});

			services.TryAddSingleton<TRabbitQueue>();
			
			return this;
		}
	}
}