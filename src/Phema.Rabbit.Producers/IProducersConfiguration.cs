using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	/// <summary>
	/// Used to configuring <see cref="RabbitProducer{TPayload,TRabbitExchange}"/> for <see cref="FanoutRabbitExchange{TPayload}"/>
	/// </summary>
	public interface IProducersConfiguration<TPayload, TRabbitExchange>
		where TRabbitExchange : FanoutRabbitExchange<TPayload>
	{
		/// <summary>
		/// Used to add <see cref="TRabbitProducer"/> service
		/// </summary>
		IProducersConfiguration<TRabbitExchange> AddProducer<TRabbitProducer, TRabbitQueue>()
			where TRabbitProducer : RabbitProducer<TPayload, TRabbitExchange>
			where TRabbitQueue : RabbitQueue<TPayload>;
	}
	
	/// <summary>
	/// Used for configuring <see cref="RabbitProducer{TPayload,TRabbitExchange}"/>
	/// </summary>
	public interface IProducersConfiguration<TRabbitExchange>
		where TRabbitExchange : RabbitExchange
	{
		/// <summary>
		/// Used to add <see cref="TRabbitProducer"/> service
		/// </summary>
		IProducersConfiguration<TRabbitExchange> AddProducer<TPayload, TRabbitProducer, TRabbitQueue>()
			where TRabbitProducer : RabbitProducer<TPayload, TRabbitExchange>
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
			where TRabbitProducer : RabbitProducer<TPayload, TFanoutRabbitExchange>
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
			where TRabbitProducer : RabbitProducer<TPayload, TRabbitExchange>
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