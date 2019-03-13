using System;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducerBuilder
		: IRabbitMQRoutingKeyBuilder<IRabbitMQProducerBuilder>,
			IRabbitMQWithArgumentBuilder<IRabbitMQProducerBuilder>
	{
		IRabbitMQProducerBuilder Mandatory();
		IRabbitMQProducerBuilder WithProperty(Action<IBasicProperties> property);
	}

	internal sealed class RabbitMQProducerBuilder : IRabbitMQProducerBuilder
	{
		private readonly RabbitMQProducerMetadata producer;

		public RabbitMQProducerBuilder(RabbitMQProducerMetadata producer)
		{
			this.producer = producer;
		}

		public IRabbitMQProducerBuilder WithRoutingKey(string routingKey)
		{
			producer.RoutingKey = routingKey;
			return this;
		}

		public IRabbitMQProducerBuilder WithArgument<TValue>(string argument, TValue value)
		{
			producer.Arguments.Add(argument, value);
			return this;
		}

		public IRabbitMQProducerBuilder Mandatory()
		{
			producer.Mandatory = true;
			return this;
		}

		public IRabbitMQProducerBuilder WithProperty(Action<IBasicProperties> property)
		{
			producer.Properties.Add(property);
			return this;
		}
	}
}