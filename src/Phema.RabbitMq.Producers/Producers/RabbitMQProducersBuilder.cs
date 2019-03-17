using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Phema.Serialization;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducersBuilder
	{
		/// <summary>
		///   Add new producer
		/// </summary>
		IRabbitMQProducerBuilder AddProducer<TPayload>(string exchangeName, string queueName = null);
	}

	internal sealed class RabbitMQProducersBuilder : IRabbitMQProducersBuilder
	{
		private readonly IServiceCollection services;

		public RabbitMQProducersBuilder(IServiceCollection services)
		{
			this.services = services;
		}

		public IRabbitMQProducerBuilder AddProducer<TPayload>(string exchangeName, string queueName)
		{
			if (exchangeName is null)
				throw new ArgumentNullException(nameof(exchangeName));

			if (queueName is null)
				throw new ArgumentNullException(nameof(queueName));

			var producer = new RabbitMQProducerMetadata(exchangeName, queueName);

			services.TryAddSingleton<IRabbitMQProducer<TPayload>>(provider =>
			{
				var channel = provider.GetRequiredService<IConnection>().CreateModel();

				var exchange = provider.GetRequiredService<IOptions<RabbitMQExchangesOptions>>()
					.Value
					.Exchanges
					.FirstOrDefault(ex => ex.Name == producer.ExchangeName);

				if (exchange != null)
				{
					channel.ExchangeDeclareNoWait(
						exchange.Name,
						exchange.Type,
						exchange.Durable,
						exchange.AutoDelete,
						exchange.Arguments);

					foreach (var binding in exchange.ExchangeBindings)
						channel.ExchangeBindNoWait(
							binding.ExchangeName,
							exchange.Name,
							binding.RoutingKey ?? binding.ExchangeName,
							binding.Arguments);
				}

				channel.QueueBindNoWait(
					producer.QueueName,
					producer.ExchangeName,
					producer.RoutingKey ?? producer.QueueName,
					// Queue arguments for topic exchange
					producer.Arguments);

				var serializer = provider.GetRequiredService<ISerializer>();

				var properties = channel.CreateBasicProperties();
				foreach (var property in producer.Properties)
					property(properties);

				return new RabbitMQProducer<TPayload>(channel, serializer, producer, properties);
			});

			return new RabbitMQProducerBuilder(producer);
		}
	}
}