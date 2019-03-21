using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQQueueBuilderExtensions
	{
		/// <summary>
		///   Marks queue as durable
		/// </summary>
		public static IRabbitMQQueueBuilder Durable(this IRabbitMQQueueBuilder builder)
		{
			builder.Metadata.Durable = true;

			return builder;
		}

		/// <summary>
		///   Sets queue as exclusive for consumers
		/// </summary>
		public static IRabbitMQQueueBuilder Exclusive(this IRabbitMQQueueBuilder builder)
		{
			builder.Metadata.Exclusive = true;

			return builder;
		}

		/// <summary>
		///   Sets nowait for queue declaration
		/// </summary>
		public static IRabbitMQQueueBuilder NoWait(this IRabbitMQQueueBuilder builder)
		{
			builder.Metadata.NoWait = true;
			return builder;
		}

		/// <summary>
		///   Sets purged for queue declaration. Queue will be purged before use
		/// </summary>
		public static IRabbitMQQueueBuilder Purged(this IRabbitMQQueueBuilder builder)
		{
			builder.Metadata.Purged = true;
			return builder;
		}

		/// <summary>
		///   Sets deleted for queue declaration. Queue will be deleted before use
		/// </summary>
		public static IRabbitMQQueueBuilder Deleted(
			this IRabbitMQQueueBuilder builder,
			bool ifUnused = false,
			bool ifEmpty = false)
		{
			builder.Metadata.Deleted = true;
			builder.Metadata.IfUnused = ifUnused;
			builder.Metadata.IfEmpty = ifEmpty;

			return builder;
		}

		/// <summary>
		///   Sets auto-delete flag to queue
		/// </summary>
		public static IRabbitMQQueueBuilder AutoDelete(this IRabbitMQQueueBuilder builder)
		{
			builder.Metadata.AutoDelete = true;

			return builder;
		}

		/// <summary>
		///   Sets RabbitMQ arguments. Allow multiple
		/// </summary>
		public static IRabbitMQQueueBuilder WithArgument<TValue>(
			this IRabbitMQQueueBuilder builder,
			string argument,
			TValue value)
		{
			builder.Metadata.Arguments.Add(argument, value);

			return builder;
		}

		/// <summary>
		///   Sets x-queue-mode argument to lazy (default "default")
		/// </summary>
		public static IRabbitMQQueueBuilder Lazy(this IRabbitMQQueueBuilder configuration)
		{
			return configuration.WithArgument("x-queue-mode", "lazy");
		}

		/// <summary>
		///   Sets x-max-length argument. When queue message limit reached, see
		///   <see cref="RabbitMQQueueBuilderExtensions.WithRejectPublishOnOverflow" />
		/// </summary>
		public static IRabbitMQQueueBuilder WithMaxMessageCount(
			this IRabbitMQQueueBuilder configuration,
			uint count)
		{
			// Hack, because does not uint table value support yet
			return configuration.WithArgument("x-max-length", (long)count);
		}

		/// <summary>
		///   Sets x-max-length-bytes argument. When size limit reached, message will be marked as dead
		/// </summary>
		public static IRabbitMQQueueBuilder WithMaxMessageSize(
			this IRabbitMQQueueBuilder configuration,
			uint bytes)
		{
			// Hack, because does not uint table value support yet
			return configuration.WithArgument("x-max-length-bytes", (long)bytes);
		}

		/// <summary>
		///   Sets x-dead-letter-exchange argument. When message is dead, send to x-dead-letter-exchange
		/// </summary>
		public static IRabbitMQQueueBuilder WithDeadLetterExchange(
			this IRabbitMQQueueBuilder configuration,
			string exchangeName)
		{
			if (exchangeName is null)
				throw new ArgumentNullException(nameof(exchangeName));

			return configuration.WithArgument("x-dead-letter-exchange", exchangeName);
		}


		/// <summary>
		///   Sets x-dead-letter-routing-key argument. When message is dead, send to x-dead-letter-exchange with routing key
		/// </summary>
		public static IRabbitMQQueueBuilder WithDeadLetterRoutingKey(
			this IRabbitMQQueueBuilder configuration,
			string routingKey)
		{
			if (routingKey is null)
				throw new ArgumentNullException(nameof(routingKey));

			return configuration.WithArgument("x-dead-letter-routing-key", routingKey);
		}

		/// <summary>
		///   Sets x-message-ttl argument. When expires message will be marked as dead
		/// </summary>
		public static IRabbitMQQueueBuilder WithMessageTimeToLive(
			this IRabbitMQQueueBuilder configuration,
			int timeToLive)
		{
			return configuration.WithArgument("x-message-ttl", timeToLive);
		}

		/// <summary>
		///   Sets x-expires argument for queue. When expires queue will be deleted
		/// </summary>
		public static IRabbitMQQueueBuilder WithTimeToLive(
			this IRabbitMQQueueBuilder configuration,
			int milliseconds)
		{
			return configuration.WithArgument("x-expires", milliseconds);
		}

		/// <summary>
		///   Sets x-max-priority argument for queue (default 0)
		/// </summary>
		public static IRabbitMQQueueBuilder WithMaxPriority(
			this IRabbitMQQueueBuilder configuration,
			byte priority)
		{
			// Hack, because RabbitMQ.Client has no conversion to byte
			return configuration.WithArgument("x-max-priority", (int) priority);
		}

		/// <summary>
		///   Sets x-overflow argument to reject-publish (default drop-head)
		/// </summary>
		public static IRabbitMQQueueBuilder WithRejectPublishOnOverflow(
			this IRabbitMQQueueBuilder configuration)
		{
			return configuration.WithArgument("x-overflow", "reject-publish");
		}
	}
}