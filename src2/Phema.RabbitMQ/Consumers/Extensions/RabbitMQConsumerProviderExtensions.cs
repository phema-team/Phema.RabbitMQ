using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQConsumerProviderExtensions
	{
		/// <summary>
		///   Sets consumer tag used in queue consumers
		/// </summary>
		public static IRabbitMQConsumerProvider WithTag(
			this IRabbitMQConsumerProvider builder,
			string consumerTag)
		{
			if (consumerTag is null)
				throw new ArgumentNullException(nameof(consumerTag));

			builder.Metadata.Tag = consumerTag;

			return builder;
		}

		/// <summary>
		///   Sets message prefetch count
		/// </summary>
		public static IRabbitMQConsumerProvider WithPrefetchCount(
			this IRabbitMQConsumerProvider builder,
			ushort prefetch,
			bool global = true)
		{
			builder.Metadata.PrefetchCount = prefetch;
			builder.Metadata.Global = global;

			return builder;
		}


		/// <summary>
		///   Sets count parallel consumers
		/// </summary>
		public static IRabbitMQConsumerProvider WithCount(
			this IRabbitMQConsumerProvider builder,
			uint count)
		{
			builder.Metadata.Count = count;

			return builder;
		}

		/// <summary>
		///   Sets exclusive consumer for queue
		/// </summary>
		public static IRabbitMQConsumerProvider Exclusive(this IRabbitMQConsumerProvider builder)
		{
			builder.Metadata.Exclusive = true;
			return builder;
		}

		/// <summary>
		///   Sets no-local flag. If true, rabbitmq will not send messages to the connection that published them
		/// </summary>
		public static IRabbitMQConsumerProvider NoLocal(this IRabbitMQConsumerProvider builder)
		{
			builder.Metadata.NoLocal = true;

			return builder;
		}

		/// <summary>
		///   Sets auto-ack flag. If true, consumer will ack messages when received
		/// </summary>
		public static IRabbitMQConsumerProvider AutoAck(this IRabbitMQConsumerProvider builder)
		{
			builder.Metadata.AutoAck = true;

			return builder;
		}

		/// <summary>
		///   Requeue message when fail to consume
		/// </summary>
		public static IRabbitMQConsumerProvider Requeue(
			this IRabbitMQConsumerProvider builder,
			bool multiple = false)
		{
			builder.Metadata.Requeue = true;
			builder.Metadata.Multiple = multiple;

			return builder;
		}

		/// <summary>
		///   Sets RabbitMQ arguments. Allow multiple
		/// </summary>
		public static IRabbitMQConsumerProvider WithArgument<TValue>(
			this IRabbitMQConsumerProvider builder,
			string argument,
			TValue value)
		{
			if (argument is null)
				throw new ArgumentNullException(nameof(argument));

			if (builder.Metadata.Arguments.ContainsKey(argument))
				throw new ArgumentException($"Argument {argument} already registered", nameof(argument));

			builder.Metadata.Arguments.Add(argument, value);

			return builder;
		}

		public static IRabbitMQConsumerProvider WithPriority(
			this IRabbitMQConsumerProvider configuration,
			byte priority)
		{
			// Hack, because RabbitMQ.Client has no conversion to byte
			return configuration.WithArgument("x-priority", (int) priority);
		}
	}
}