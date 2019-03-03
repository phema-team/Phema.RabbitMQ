using System;

using RabbitMQ.Client;

namespace Phema.RabbitMq
{
	public interface IRabbitMqProducerConfiguration
	{
		IRabbitMqProducerConfiguration Mandatory();
		IRabbitMqProducerConfiguration WithProperties(params Action<IBasicProperties>[] properties);
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

		public IRabbitMqProducerConfiguration WithProperties(params Action<IBasicProperties>[] properties)
		{
			foreach (var property in properties)
				producer.Properties.Add(property);

			return this;
		}
	}
}