using System;
using System.Linq;

namespace Phema.RabbitMQ
{
	public static class RabbitMQQueueBuilderExtensions
	{
		/// <summary>
		///   Declare queue as durable
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> Durable<TPayload>(this IRabbitMQQueueBuilder<TPayload> builder)
		{
			builder.Declaration.Durable = true;

			return builder;
		}

		/// <summary>
		///   Declare queue as exclusive for consumers
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> Exclusive<TPayload>(this IRabbitMQQueueBuilder<TPayload> builder)
		{
			builder.Declaration.Exclusive = true;

			return builder;
		}

		/// <summary>
		///   Nowait for queue declaration
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> NoWait<TPayload>(this IRabbitMQQueueBuilder<TPayload> builder)
		{
			builder.Declaration.NoWait = true;
			return builder;
		}

		/// <summary>
		///   Delete queue declaration
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> Deleted<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder,
			bool unusedOnly = false,
			bool emptyOnly = false)
		{
			builder.Declaration.Deleted = true;
			builder.Declaration.UnusedOnly = unusedOnly;
			builder.Declaration.EmptyOnly = emptyOnly;

			return builder;
		}

		/// <summary>
		///   Declare auto-delete flag to queue
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> AutoDelete<TPayload>(this IRabbitMQQueueBuilder<TPayload> builder)
		{
			builder.Declaration.AutoDelete = true;

			return builder;
		}

		/// <summary>
		///   Declare x-queue-mode argument to lazy (default "default")
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> Lazy<TPayload>(this IRabbitMQQueueBuilder<TPayload> builder)
		{
			return builder.Argument("x-queue-mode", "lazy");
		}

		/// <summary>
		///   Declare x-max-length argument. When queue message limit reached, see
		///   <see cref="RejectPublish{TPayload}" />
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> MaxMessageCount<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder,
			uint count)
		{
			return builder.Argument("x-max-length", count);
		}

		/// <summary>
		///   Declare x-max-length-bytes argument. When size limit reached, message will be marked as dead
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> MaxMessageSize<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder,
			uint bytes)
		{
			return builder.Argument("x-max-length-bytes", bytes);
		}

		#region DeadLetterTo

		private static IRabbitMQQueueBuilder<TPayload> DeadLetterTo<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder,
			IRabbitMQExchangeBuilderCore exchange,
			string routingKey)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			if (routingKey != null)
			{
				builder.Argument("x-dead-letter-routing-key", routingKey);
			}

			return builder.Argument("x-dead-letter-exchange", exchange.ExchangeDeclaration.Name);
		}
		
		/// <summary>
		///   Declare x-dead-letter-exchange argument. When message is dead, send to x-dead-letter-exchange.
		///   Declare x-dead-letter-routing-key argument. When message is dead, send to x-dead-letter-exchange with routing key.
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> DeadLetterTo<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder,
			IRabbitMQExchangeBuilder<TPayload> exchange,
			string routingKey = null)
		{
			return builder.DeadLetterTo((IRabbitMQExchangeBuilderCore)exchange, routingKey);
		}

		/// <summary>
		///   Declare x-dead-letter-exchange argument. When message is dead, send to x-dead-letter-exchange.
		///   Declare x-dead-letter-routing-key argument. When message is dead, send to x-dead-letter-exchange with routing key.
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> DeadLetterTo<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder,
			IRabbitMQExchangeBuilder exchange,
			string routingKey = null)
		{
			return builder.DeadLetterTo((IRabbitMQExchangeBuilderCore)exchange, routingKey);
		}

		/// <summary>
		///   Declare x-dead-letter-exchange argument. When message is dead, send to x-dead-letter-exchange.
		///   Declare x-dead-letter-routing-key argument. When message is dead, send to x-dead-letter-exchange with routing key.
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> DeadLetterTo<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder,
			IRabbitMQExchangeBuilder<TPayload> exchange,
			IRabbitMQQueueBuilder<TPayload> queue)
		{
			var binding = queue.Declaration
				.BindingDeclarations
				.FirstOrDefault(b => b.ExchangeDeclaration == exchange.ExchangeDeclaration);

			return builder.DeadLetterTo(exchange, binding?.RoutingKey ?? queue.Declaration.Name);
		}

		/// <summary>
		///   Declare x-dead-letter-exchange argument. When message is dead, send to x-dead-letter-exchange.
		///   Declare x-dead-letter-routing-key argument. When message is dead, send to x-dead-letter-exchange with routing key.
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> DeadLetterTo<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder,
			IRabbitMQExchangeBuilder exchange,
			IRabbitMQQueueBuilder<TPayload> queue)
		{
			var binding = queue.Declaration
				.BindingDeclarations
				.FirstOrDefault(b => b.ExchangeDeclaration == exchange.ExchangeDeclaration);

			return builder.DeadLetterTo(exchange, binding?.RoutingKey ?? queue.Declaration.Name);
		}

		#endregion

		/// <summary>
		///   Declare x-expires argument for queue. When expires queue will be deleted
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> TimeToLive<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder,
			TimeSpan timeToLive)
		{
			return builder.Argument("x-expires", timeToLive.Milliseconds);
		}

		/// <summary>
		///   Declare x-message-ttl argument. When expires message will be marked as dead
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> MessageTimeToLive<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder,
			TimeSpan timeToLive)
		{
			return builder.Argument("x-message-ttl", timeToLive.Milliseconds);
		}

		/// <summary>
		///   Declare x-max-priority argument for queue (default 0)
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> MaxPriority<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder,
			byte priority)
		{
			return builder.Argument("x-max-priority", priority);
		}

		/// <summary>
		///   Declare x-overflow argument to reject-publish (default drop-head)
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> RejectPublish<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder)
		{
			return builder.Argument("x-overflow", "reject-publish");
		}

		#region BoundTo

		private static IRabbitMQQueueBuilder<TPayload> BoundTo<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder,
			IRabbitMQExchangeBuilderCore exchange,
			Action<IRabbitMQQueueBindingBuilder> binding)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			var declaration = new RabbitMQQueueBindingDeclaration(exchange.ExchangeDeclaration);

			binding?.Invoke(new RabbitMQQueueBindingBuilder(declaration));

			builder.Declaration.BindingDeclarations.Add(declaration);

			return builder;
		}

		/// <summary>
		///   Declare exchange to exchange binding
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> BoundTo<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder,
			IRabbitMQExchangeBuilder exchange,
			Action<IRabbitMQQueueBindingBuilder> binding = null)
		{
			return builder.BoundTo((IRabbitMQExchangeBuilderCore)exchange, binding);
		}

		/// <summary>
		///   Declare exchange to exchange binding
		/// </summary>
		public static IRabbitMQQueueBuilder<TPayload> BoundTo<TPayload>(
			this IRabbitMQQueueBuilder<TPayload> builder,
			IRabbitMQExchangeBuilder<TPayload> exchange,
			Action<IRabbitMQQueueBindingBuilder> binding = null)
		{
			return builder.BoundTo((IRabbitMQExchangeBuilderCore)exchange, binding);
		}

		#endregion

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