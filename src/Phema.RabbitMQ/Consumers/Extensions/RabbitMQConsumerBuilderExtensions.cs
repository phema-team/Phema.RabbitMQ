using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQConsumerBuilderExtensions
	{
		/// <summary>
		///   Declare consumer tag
		/// </summary>
		public static IRabbitMQConsumerBuilder<TPayload> Tagged<TPayload>(
			this IRabbitMQConsumerBuilder<TPayload> builder,
			string consumerTag)
		{
			if (consumerTag is null)
				throw new ArgumentNullException(nameof(consumerTag));

			builder.Declaration.Tag = consumerTag;

			return builder;
		}

		/// <summary>
		///   Declare message prefetch count
		/// </summary>
		public static IRabbitMQConsumerBuilder<TPayload> Prefetch<TPayload>(
			this IRabbitMQConsumerBuilder<TPayload> builder,
			ushort prefetchCount,
			bool global = false)
		{
			builder.Declaration.PrefetchCount = prefetchCount;
			builder.Declaration.Global = global;

			return builder;
		}

		/// <summary>
		///   Declare parallel consumers count
		/// </summary>
		public static IRabbitMQConsumerBuilder<TPayload> Count<TPayload>(
			this IRabbitMQConsumerBuilder<TPayload> builder,
			uint count)
		{
			builder.Declaration.Count = count;

			return builder;
		}

		/// <summary>
		///   Declare consumer as exclusive
		/// </summary>
		public static IRabbitMQConsumerBuilder<TPayload> Exclusive<TPayload>(
			this IRabbitMQConsumerBuilder<TPayload> builder)
		{
			builder.Declaration.Exclusive = true;
			return builder;
		}

		/// <summary>
		///   Declare no-local flag. Broker will not send messages to the connection that published them
		/// </summary>
		public static IRabbitMQConsumerBuilder<TPayload> NoLocal<TPayload>(this IRabbitMQConsumerBuilder<TPayload> builder)
		{
			builder.Declaration.NoLocal = true;

			return builder;
		}

		/// <summary>
		///   Declare auto-ack flag. Consumer will ack messages when received, not processed
		/// </summary>
		public static IRabbitMQConsumerBuilder<TPayload> AutoAck<TPayload>(this IRabbitMQConsumerBuilder<TPayload> builder)
		{
			builder.Declaration.AutoAck = true;

			return builder;
		}

		/// <summary>
		///   Requeue message when fail to consume
		/// </summary>
		public static IRabbitMQConsumerBuilder<TPayload> Requeue<TPayload>(
			this IRabbitMQConsumerBuilder<TPayload> builder,
			bool multiple = false)
		{
			builder.Declaration.Requeue = true;
			builder.Declaration.Multiple = multiple;

			return builder;
		}

		public static IRabbitMQConsumerBuilder<TPayload> Priority<TPayload>(
			this IRabbitMQConsumerBuilder<TPayload> configuration,
			byte priority)
		{
			// TODO: Hack, because RabbitMQ.Client has no conversion to byte
			return configuration.Argument("x-priority", (int)priority);
		}

		/// <summary>
		///   Declare RabbitMQ arguments. Allow multiple
		/// </summary>
		public static IRabbitMQConsumerBuilder<TPayload> Argument<TPayload>(
			this IRabbitMQConsumerBuilder<TPayload> builder,
			string argument,
			object value)
		{
			if (argument is null)
				throw new ArgumentNullException(nameof(argument));

			if (builder.Declaration.Arguments.ContainsKey(argument))
				throw new ArgumentException($"Argument {argument} already registered", nameof(argument));

			builder.Declaration.Arguments.Add(argument, value);

			return builder;
		}
	}
}