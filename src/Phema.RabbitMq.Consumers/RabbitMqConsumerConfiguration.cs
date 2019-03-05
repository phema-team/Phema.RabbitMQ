namespace Phema.RabbitMQ
{
	public interface IRabbitMQConsumerConfiguration
	{
		IRabbitMQConsumerConfiguration WithTag(string consumerTag);
		IRabbitMQConsumerConfiguration WithPrefetch(ushort prefetch);
		IRabbitMQConsumerConfiguration WithConsumers(int consumers);
		IRabbitMQConsumerConfiguration Exclusive();
		IRabbitMQConsumerConfiguration NoLocal();
		IRabbitMQConsumerConfiguration AutoAck();
		IRabbitMQConsumerConfiguration Requeue(bool multiple = false);
		IRabbitMQConsumerConfiguration WithArgument(string argument, string value);
	}

	internal sealed class RabbitMQConsumerConfiguration : IRabbitMQConsumerConfiguration
	{
		private readonly RabbitMQConsumer consumer;

		public RabbitMQConsumerConfiguration(RabbitMQConsumer consumer)
		{
			this.consumer = consumer;
		}

		public IRabbitMQConsumerConfiguration WithTag(string consumerTag)
		{
			consumer.Tag = consumerTag;
			return this;
		}

		public IRabbitMQConsumerConfiguration WithPrefetch(ushort prefetch)
		{
			consumer.Prefetch = prefetch;
			return this;
		}

		public IRabbitMQConsumerConfiguration WithConsumers(int consumers)
		{
			consumer.Consumers = consumers;
			return this;
		}

		public IRabbitMQConsumerConfiguration Exclusive()
		{
			consumer.Exclusive = true;
			return this;
		}

		public IRabbitMQConsumerConfiguration NoLocal()
		{
			consumer.NoLocal = true;
			return this;
		}

		public IRabbitMQConsumerConfiguration AutoAck()
		{
			consumer.AutoAck = true;
			return this;
		}

		public IRabbitMQConsumerConfiguration Requeue(bool multiple = false)
		{
			consumer.Requeue = true;
			consumer.Multiple = multiple;
			return this;
		}

		public IRabbitMQConsumerConfiguration WithArgument(string argument, string value)
		{
			consumer.Arguments.Add(argument, value);
			return this;
		}
	}
}