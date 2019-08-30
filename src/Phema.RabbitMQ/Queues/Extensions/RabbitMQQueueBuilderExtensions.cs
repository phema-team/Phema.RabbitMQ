using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQQueueBuilderExtensions
	{
		/// <summary>
		///   Declare queue as durable
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> Durable<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder)
		{
			builder.Declaration.Durable = true;

			return builder;
		}

		/// <summary>
		///   Declare queue as exclusive for consumers
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> Exclusive<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder)
		{
			builder.Declaration.Exclusive = true;

			return builder;
		}

		/// <summary>
		///   Nowait for queue declaration
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> NoWait<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder)
		{
			builder.Declaration.NoWait = true;
			return builder;
		}

		/// <summary>
		///   Delete queue declaration
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> Deleted<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder,
			bool ifUnused = false,
			bool ifEmpty = false)
		{
			builder.Declaration.Deleted = true;
			builder.Declaration.IfUnused = ifUnused;
			builder.Declaration.IfEmpty = ifEmpty;

			return builder;
		}

		/// <summary>
		///   Declare auto-delete flag to queue
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> AutoDelete<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder)
		{
			builder.Declaration.AutoDelete = true;

			return builder;
		}

		/// <summary>
		///   Declare x-queue-mode argument to lazy (default "default")
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> Lazy<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> configuration)
		{
			return configuration.Argument("x-queue-mode", "lazy");
		}

		/// <summary>
		///   Declare x-max-length argument. When queue message limit reached, see
		///   <see cref="RejectPublish{TPayload}" />
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> MaxMessageCount<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> configuration,
			uint count)
		{
			// TODO: Hack, because does not uint table value support yet
			return configuration.Argument("x-max-length", (long)count);
		}

		/// <summary>
		///   Declare x-max-length-bytes argument. When size limit reached, message will be marked as dead
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> MaxMessageSize<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> configuration,
			uint bytes)
		{
			// TODO: Hack, because does not uint table value support yet
			return configuration.Argument("x-max-length-bytes", (long)bytes);
		}

		/// <summary>
		///   Declare x-dead-letter-exchange argument. When message is dead, send to x-dead-letter-exchange.
		///   Declare x-dead-letter-routing-key argument. When message is dead, send to x-dead-letter-exchange with routing key.
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> DeadLetterTo<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> configuration,
			IRabbitMQExchangeBuilder<TPayload> exchange,
			string routingKey = null)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			if (routingKey != null)
			{
				configuration.Argument("x-dead-letter-routing-key", routingKey);
			}

			return configuration.Argument("x-dead-letter-exchange", exchange.Declaration.Name);
		}

		/// <summary>
		///   Declare x-expires argument for queue. When expires queue will be deleted
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> TimeToLive<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> configuration,
			int milliseconds)
		{
			return configuration.Argument("x-expires", milliseconds);
		}
		
		/// <summary>
		///   Declare x-message-ttl argument. When expires message will be marked as dead
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> MessageTimeToLive<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> configuration,
			int timeToLive)
		{
			return configuration.Argument("x-message-ttl", timeToLive);
		}

		/// <summary>
		///   Declare x-max-priority argument for queue (default 0)
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> MaxPriority<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> configuration,
			byte priority)
		{
			// TODO: Hack, because RabbitMQ.Client has no conversion to byte
			return configuration.Argument("x-max-priority", (int) priority);
		}

		/// <summary>
		///   Declare x-overflow argument to reject-publish (default drop-head)
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> RejectPublish<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> configuration)
		{
			return configuration.Argument("x-overflow", "reject-publish");
		}

		/// <summary>
		///   Declare exchange to exchange binding
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> BoundTo<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder,
			IRabbitMQExchangeBuilder<TPayload> exchange,
			Action<IRabbitMQQueueBindingBuilder> binding = null)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			var declaration = new RabbitMQQueueBindingDeclaration(exchange.Declaration);

			binding?.Invoke(new RabbitMQQueueBindingBuilder(declaration));

			builder.Declaration.Bindings.Add(declaration);

			return builder;
		}

		/// <summary>
		///   Declare RabbitMQ arguments. Allow multiple
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> Argument<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder,
			string argument,
			object value)
		{
			builder.Declaration.Arguments.Add(argument, value);

			return builder;
		}
	}
}