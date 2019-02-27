using System.Collections.Generic;

namespace Phema.RabbitMq
{
	public interface IRabbitMqConsumerConfiguration
	{
		IRabbitMqConsumerConfiguration WithConsumerTag(string consumerTag);
		IRabbitMqConsumerConfiguration WithPrefetch(ushort prefetch);
		IRabbitMqConsumerConfiguration WithConsumers(int consumers);
		IRabbitMqConsumerConfiguration Exclusive();
		IRabbitMqConsumerConfiguration NoLocal();
		IRabbitMqConsumerConfiguration AutoAck();
		IRabbitMqConsumerConfiguration Requeue(bool multiple = false);
		IRabbitMqConsumerConfiguration WithArgument(string argument, string value);
	}
	
	internal sealed class RabbitMqConsumerConsiguration : IRabbitMqConsumerConfiguration
	{
		private readonly RabbitMqConsumer consumer;

		public RabbitMqConsumerConsiguration(RabbitMqConsumer consumer)
		{
			this.consumer = consumer;
		}

		public IRabbitMqConsumerConfiguration WithConsumerTag(string consumerTag)
		{
			consumer.ConsumerTag = consumerTag;
			return this;
		}

		public IRabbitMqConsumerConfiguration WithPrefetch(ushort prefetch)
		{
			consumer.Prefetch = prefetch;
			return this;
		}

		public IRabbitMqConsumerConfiguration WithConsumers(int consumers)
		{
			consumer.Consumers = consumers;
			return this;
		}

		public IRabbitMqConsumerConfiguration Exclusive()
		{
			consumer.Exclusive = true;
			return this;
		}
		
		public IRabbitMqConsumerConfiguration NoLocal()
		{
			consumer.NoLocal = true;
			return this;
		}

		public IRabbitMqConsumerConfiguration AutoAck()
		{
			consumer.AutoAck = true;
			return this;
		}

		public IRabbitMqConsumerConfiguration Requeue(bool multiple = false)
		{
			consumer.Requeue = true;
			consumer.Multiple = multiple;
			return this;
		}

		public IRabbitMqConsumerConfiguration WithArgument(string argument, string value)
		{
			consumer.Arguments.Add(argument, value);
			return this;
		}
	}
}