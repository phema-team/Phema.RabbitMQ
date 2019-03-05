using System;

using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducerConfiguration
	{
		IRabbitMQProducerConfiguration Mandatory();
		IRabbitMQProducerConfiguration WithProperties(params Action<IBasicProperties>[] properties);
	}

	internal sealed class RabbitMQProducerConfiguration : IRabbitMQProducerConfiguration
	{
		private readonly RabbitMQProducer producer;

		public RabbitMQProducerConfiguration(RabbitMQProducer producer)
		{
			this.producer = producer;
		}

		public IRabbitMQProducerConfiguration WithRoutingKey(string routingKey)
		{
			producer.RoutingKey = routingKey;
			return this;
		}

		public IRabbitMQProducerConfiguration Mandatory()
		{
			producer.Mandatory = true;
			return this;
		}

		public IRabbitMQProducerConfiguration WithProperties(params Action<IBasicProperties>[] properties)
		{
			foreach (var property in properties)
				producer.Properties.Add(property);

			return this;
		}
		
		public IRabbitMQProducerConfiguration WithArgument(string argument, string value)
		{
			producer.Arguments.Add(argument, value);
			return this;
		}
	}
}