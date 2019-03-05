using System;

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
		IRabbitMQConsumerConfiguration WithArgument<TValue>(string argument, TValue value);
	}

	internal sealed class RabbitMQConsumerConfiguration : IRabbitMQConsumerConfiguration
	{
		private readonly RabbitMQConsumer consumer;

		public RabbitMQConsumerConfiguration(RabbitMQConsumer consumer)
		{
			this.consumer = consumer;
		}

		/// <summary>
		/// Sets consumer tag used in queue consumers
		/// </summary>
		public IRabbitMQConsumerConfiguration WithTag(string consumerTag)
		{
			if (consumerTag is null)
				throw new ArgumentNullException(nameof(consumerTag));
			
			consumer.Tag = consumerTag;
			return this;
		}

		/// <summary>
		/// Sets message count prefetch
		/// </summary>
		public IRabbitMQConsumerConfiguration WithPrefetch(ushort prefetch)
		{
			consumer.Prefetch = prefetch;
			return this;
		}

		/// <summary>
		/// Sets count parallel consumers
		/// </summary>
		public IRabbitMQConsumerConfiguration WithConsumers(int consumers)
		{
			consumer.Consumers = consumers;
			return this;
		}

		/// <summary>
		/// Sets exclusive flag
		/// </summary>
		public IRabbitMQConsumerConfiguration Exclusive()
		{
			consumer.Exclusive = true;
			return this;
		}

		/// <summary>
		/// Sets no-local flag. If true, rabbitmq will not send messages to the connection that published them
		/// </summary>
		public IRabbitMQConsumerConfiguration NoLocal()
		{
			consumer.NoLocal = true;
			return this;
		}

		/// <summary>
		/// Sets auto-ack flag. If true, consumer will ack messages when received
		/// </summary>
		public IRabbitMQConsumerConfiguration AutoAck()
		{
			consumer.AutoAck = true;
			return this;
		}

		/// <summary>
		/// Requeue message when fail to consume.
		/// </summary>
		public IRabbitMQConsumerConfiguration Requeue(bool multiple = false)
		{
			consumer.Requeue = true;
			consumer.Multiple = multiple;
			return this;
		}

		/// <summary>
		/// Sets rabbitmq arguments
		/// </summary>
		public IRabbitMQConsumerConfiguration WithArgument<TValue>(string argument, TValue value)
		{
			if (argument is null)
				throw new ArgumentNullException(nameof(argument));
			
			if (consumer.Arguments.ContainsKey(argument))
				throw new ArgumentException($"Argument {argument} already registered", nameof(argument));
			
			consumer.Arguments.Add(argument, value);
			return this;
		}
	}
}