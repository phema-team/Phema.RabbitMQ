using System;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConsumerBuilder
		: IRabbitMQExclusiveBuilder<IRabbitMQConsumerBuilder>,
			IRabbitMQWithArgumentBuilder<IRabbitMQConsumerBuilder>
	{
		/// <summary>
		/// Sets consumer tag used in queue consumers
		/// </summary>
		IRabbitMQConsumerBuilder WithTag(string tag);

		/// <summary>
		/// Sets message count prefetch
		/// </summary>
		IRabbitMQConsumerBuilder WithPrefetch(ushort prefetch);

		/// <summary>
		/// Sets count parallel consumers
		/// </summary>
		IRabbitMQConsumerBuilder WithCount(int count);

		/// <summary>
		/// Sets no-local flag. If true, rabbitmq will not send messages to the connection that published them
		/// </summary>
		IRabbitMQConsumerBuilder NoLocal();

		/// <summary>
		/// Sets auto-ack flag. If true, consumer will ack messages when received
		/// </summary>
		IRabbitMQConsumerBuilder AutoAck();

		/// <summary>
		/// Requeue message when fail to consume.
		/// </summary>
		IRabbitMQConsumerBuilder Requeue(bool multiple = false);
	}

	internal sealed class RabbitMQConsumerBuilder : IRabbitMQConsumerBuilder
	{
		private readonly RabbitMQConsumerMetadata consumer;

		public RabbitMQConsumerBuilder(RabbitMQConsumerMetadata consumer)
		{
			this.consumer = consumer;
		}

		public IRabbitMQConsumerBuilder WithTag(string consumerTag)
		{
			if (consumerTag is null)
				throw new ArgumentNullException(nameof(consumerTag));

			consumer.Tag = consumerTag;
			return this;
		}

		public IRabbitMQConsumerBuilder WithPrefetch(ushort prefetch)
		{
			consumer.Prefetch = prefetch;
			return this;
		}

		public IRabbitMQConsumerBuilder WithCount(int count)
		{
			consumer.Count = count;
			return this;
		}

		public IRabbitMQConsumerBuilder Exclusive()
		{
			consumer.Exclusive = true;
			return this;
		}

		public IRabbitMQConsumerBuilder NoLocal()
		{
			consumer.NoLocal = true;
			return this;
		}

		public IRabbitMQConsumerBuilder AutoAck()
		{
			consumer.AutoAck = true;
			return this;
		}

		public IRabbitMQConsumerBuilder Requeue(bool multiple = false)
		{
			consumer.Requeue = true;
			consumer.Multiple = multiple;
			return this;
		}

		public IRabbitMQConsumerBuilder WithArgument<TValue>(string argument, TValue value)
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