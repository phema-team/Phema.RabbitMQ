using System.Linq;
using System.Runtime.Serialization;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using Phema.Serialization;

using RabbitMQ.Client;

namespace Phema.RabbitMq
{
	public interface IRabbitMqProducersConfiguration
	{
		IRabbitMqProducerConfiguration AddProducer<TPayload>(string exchangeName, string queueName);
	}

	internal sealed class RabbitMqProducersConfiguration : IRabbitMqProducersConfiguration
	{
		private readonly IServiceCollection services;

		public RabbitMqProducersConfiguration(IServiceCollection services)
		{
			this.services = services;
		}

		public IRabbitMqProducerConfiguration AddProducer<TPayload>(string exchangeName, string queueName)
		{
			var producer = new RabbitMqProducer(exchangeName, queueName);

			services.Configure<RabbitMqProducersOptions>(options =>
				options.Producers.Add(producer));

			services.TryAddScoped<IRabbitMqProducer<TPayload>>(provider =>
			{
				var channel = provider.GetRequiredService<IModel>();

				var exchange = provider.GetRequiredService<IOptions<RabbitMqExchangesOptions>>()
					.Value
					.Exchanges
					.FirstOrDefault(ex => ex.Name == producer.ExchangeName);

				if (exchange != null)
				{
					channel.ExchangeDeclareNoWait(
						exchange: exchange.Name,
						type: exchange.Type,
						durable: exchange.Durable,
						autoDelete: exchange.AutoDelete,
						arguments: exchange.Arguments);
				}

				var queue = provider.GetRequiredService<IOptions<RabbitMqQueuesOptions>>()
					.Value
					.Queues
					.FirstOrDefault(q => q.Name == producer.QueueName);

				if (queue != null)
				{
					channel.QueueDeclareNoWait(
						queue: queue.Name,
						durable: queue.Durable,
						exclusive: queue.Exclusive,
						autoDelete: queue.AutoDelete,
						arguments: queue.Arguments);
				}

				if (exchange != null && queue != null)
				{
					channel.QueueBindNoWait(
						queue: queue.Name,
						exchange: exchange.Name,
						routingKey: queue.Name,
						arguments: queue.Arguments);
				}

				var serializer = provider.GetRequiredService<ISerializer>();

				return new RabbitMqProducer<TPayload>(payload =>
				{
					var properties = channel.CreateBasicProperties();

					foreach (var property in producer.Properties)
					{
						property(properties);
					}

					channel.BasicPublish(
						exchange: producer.ExchangeName,
						routingKey: producer.QueueName,
						mandatory: producer.Mandatory,
						basicProperties: properties,
						body: serializer.Serialize(payload));
				});
			});

			return new RabbitMqProducerConfiguration(producer);
		}
	}
}