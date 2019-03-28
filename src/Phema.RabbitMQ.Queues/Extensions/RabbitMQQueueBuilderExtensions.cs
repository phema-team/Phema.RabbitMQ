using System;
using Phema.RabbitMQ.Internal;

namespace Phema.RabbitMQ
{
	public static class RabbitMQQueueBuilderExtensions
	{
		/// <summary>
		///   Declare queue as durable
		/// </summary>
		public static IRabbitMQQueueBuilder Durable(this IRabbitMQQueueBuilder builder)
		{
			builder.Declaration.Durable = true;

			return builder;
		}

		/// <summary>
		///   Declare queue as exclusive for consumers
		/// </summary>
		public static IRabbitMQQueueBuilder Exclusive(this IRabbitMQQueueBuilder builder)
		{
			builder.Declaration.Exclusive = true;

			return builder;
		}

		/// <summary>
		///   Nowait for queue declaration
		/// </summary>
		public static IRabbitMQQueueBuilder NoWait(this IRabbitMQQueueBuilder builder)
		{
			builder.Declaration.NoWait = true;
			return builder;
		}

		/// <summary>
		///   Delete queue declaration
		/// </summary>
		public static IRabbitMQQueueBuilder Deleted(
			this IRabbitMQQueueBuilder builder,
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
		public static IRabbitMQQueueBuilder AutoDelete(this IRabbitMQQueueBuilder builder)
		{
			builder.Declaration.AutoDelete = true;

			return builder;
		}

		/// <summary>
		///   Declare x-queue-mode argument to lazy (default "default")
		/// </summary>
		public static IRabbitMQQueueBuilder Lazy(this IRabbitMQQueueBuilder configuration)
		{
			return configuration.WithArgument("x-queue-mode", "lazy");
		}

		/// <summary>
		///   Declare x-max-length argument. When queue message limit reached, see
		///   <see cref="RejectPublish" />
		/// </summary>
		public static IRabbitMQQueueBuilder MaxMessageCount(
			this IRabbitMQQueueBuilder configuration,
			uint count)
		{
			// TODO: Hack, because does not uint table value support yet
			return configuration.WithArgument("x-max-length", (long)count);
		}

		/// <summary>
		///   Declare x-max-length-bytes argument. When size limit reached, message will be marked as dead
		/// </summary>
		public static IRabbitMQQueueBuilder MaxMessageSize(
			this IRabbitMQQueueBuilder configuration,
			uint bytes)
		{
			// TODO: Hack, because does not uint table value support yet
			return configuration.WithArgument("x-max-length-bytes", (long)bytes);
		}

		/// <summary>
		///   Declare x-dead-letter-exchange argument. When message is dead, send to x-dead-letter-exchange.
		///   Declare x-dead-letter-routing-key argument. When message is dead, send to x-dead-letter-exchange with routing key.
		/// </summary>
		public static IRabbitMQQueueBuilder DeadLetterExchange(
			this IRabbitMQQueueBuilder configuration,
			string exchangeName,
			string routingKey = null)
		{
			if (exchangeName is null)
				throw new ArgumentNullException(nameof(exchangeName));

			if (routingKey != null)
			{
				configuration.WithArgument("x-dead-letter-routing-key", routingKey);
			}

			return configuration.WithArgument("x-dead-letter-exchange", exchangeName);
		}

		/// <summary>
		///   Declare x-expires argument for queue. When expires queue will be deleted
		/// </summary>
		public static IRabbitMQQueueBuilder TimeToLive(
			this IRabbitMQQueueBuilder configuration,
			int milliseconds)
		{
			return configuration.WithArgument("x-expires", milliseconds);
		}
		
		/// <summary>
		///   Declare x-message-ttl argument. When expires message will be marked as dead
		/// </summary>
		public static IRabbitMQQueueBuilder MessageTimeToLive(
			this IRabbitMQQueueBuilder configuration,
			int timeToLive)
		{
			return configuration.WithArgument("x-message-ttl", timeToLive);
		}

		/// <summary>
		///   Declare x-max-priority argument for queue (default 0)
		/// </summary>
		public static IRabbitMQQueueBuilder MaxPriority(
			this IRabbitMQQueueBuilder configuration,
			byte priority)
		{
			// TODO: Hack, because RabbitMQ.Client has no conversion to byte
			return configuration.WithArgument("x-max-priority", (int) priority);
		}

		/// <summary>
		///   Declare x-overflow argument to reject-publish (default drop-head)
		/// </summary>
		public static IRabbitMQQueueBuilder RejectPublish(
			this IRabbitMQQueueBuilder configuration)
		{
			return configuration.WithArgument("x-overflow", "reject-publish");
		}

		/// <summary>
		///   Declare exchange to exchange binding
		/// </summary>
		public static IRabbitMQQueueBuilder BoundTo(
			this IRabbitMQQueueBuilder builder,
			string exchangeName,
			Action<IRabbitMQQueueBindingBuilder> binding = null)
		{
			if (exchangeName is null)
				throw new ArgumentNullException(nameof(exchangeName));

			var declaration = new RabbitMQQueueBindingDeclaration(exchangeName);

			binding?.Invoke(new RabbitMQQueueBindingBuilder(declaration));

			builder.Declaration.QueueBindings.Add(declaration);

			return builder;
		}
		
		/// <summary>
		///   Declare RabbitMQ arguments. Allow multiple
		/// </summary>
		public static IRabbitMQQueueBuilder WithArgument<TValue>(
			this IRabbitMQQueueBuilder builder,
			string argument,
			TValue value)
		{
			builder.Declaration.Arguments.Add(argument, value);

			return builder;
		}
	}
}