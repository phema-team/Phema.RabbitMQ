using System;

using RabbitMQ.Client;

namespace Phema.RabbitMq
{
	public interface IRabbitMqProducerConfiguration
	{
		IRabbitMqProducerConfiguration Mandatory();
		IRabbitMqProducerConfiguration WithProperty(Action<IBasicProperties> property);
	}
	
	internal sealed class RabbitMqProducerConfiguration : IRabbitMqProducerConfiguration
	{
		private readonly RabbitMqProducer producer;

		public RabbitMqProducerConfiguration(RabbitMqProducer producer)
		{
			this.producer = producer;
		}

		public IRabbitMqProducerConfiguration Mandatory()
		{
			producer.Mandatory = true;
			return this;
		}

		public IRabbitMqProducerConfiguration WithProperty(Action<IBasicProperties> property)
		{
			producer.Properties.Add(property);
			return this;
		}
	}
}