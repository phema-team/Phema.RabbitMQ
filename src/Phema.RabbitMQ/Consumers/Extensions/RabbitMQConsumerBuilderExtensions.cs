using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public static class RabbitMQConsumerBuilderExtensions
	{
		#region Subscribe

		/// <summary>
		///   Received payload handler with service scope support and cancellation support
		/// </summary>
		public static IRabbitMQConsumerBuilder<TPayload> Subscribe<TPayload>(
			this IRabbitMQConsumerBuilder<TPayload> builder,
			Func<IServiceScope, TPayload, CancellationToken, ValueTask> subscription)
		{
			builder.ConsumerDeclaration
				.Subscriptions
				.Add((scope, payload, cancellationToken) => subscription(scope, (TPayload)payload, cancellationToken));

			return builder;
		}

		/// <summary>
		///   Received payload handler with service scope support
		/// </summary>
		public static IRabbitMQConsumerBuilder<TPayload> Subscribe<TPayload>(
			this IRabbitMQConsumerBuilder<TPayload> builder,
			Func<IServiceScope, TPayload, ValueTask> subscription)
		{
			return builder.Subscribe((scope, payload, cancellationToken) => subscription(scope, payload));
		}

		/// <summary>
		///   Received payload handler
		/// </summary>
		public static IRabbitMQConsumerBuilder<TPayload> Subscribe<TPayload>(
			this IRabbitMQConsumerBuilder<TPayload> builder,
			Func<TPayload, ValueTask> subscription)
		{
			return builder.Subscribe((scope, payload, cancellationToken) => subscription(payload));
		}

		#endregion

		/// <summary>
		///   Declare consumer tag
		/// </summary>
		public static IRabbitMQConsumerBuilder<TPayload> Tagged<TPayload>(
			this IRabbitMQConsumerBuilder<TPayload> builder,
			string tag)
		{
			if (tag is null)
				throw new ArgumentNullException(nameof(tag));

			builder.ConsumerDeclaration.Tag = tag;

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
			builder.ConsumerDeclaration.PrefetchCount = prefetchCount;
			builder.ConsumerDeclaration.Global = global;

			return builder;
		}

		/// <summary>
		///   Declare parallel consumers count
		/// </summary>
		public static IRabbitMQConsumerBuilder<TPayload> Count<TPayload>(
			this IRabbitMQConsumerBuilder<TPayload> builder,
			uint count)
		{
			builder.ConsumerDeclaration.Count = count;

			return builder;
		}

		/// <summary>
		///   Declare consumer as exclusive
		/// </summary>
		public static IRabbitMQConsumerBuilder<TPayload> Exclusive<TPayload>(
			this IRabbitMQConsumerBuilder<TPayload> builder)
		{
			builder.ConsumerDeclaration.Exclusive = true;
			return builder;
		}

		/// <summary>
		///   Declare no-local flag. Broker will not send messages to the connection that published them
		/// </summary>
		public static IRabbitMQConsumerBuilder<TPayload> NoLocal<TPayload>(this IRabbitMQConsumerBuilder<TPayload> builder)
		{
			builder.ConsumerDeclaration.NoLocal = true;

			return builder;
		}

		/// <summary>
		///   Declare auto-ack flag. Consumer will ack messages when received, not processed
		/// </summary>
		public static IRabbitMQConsumerBuilder<TPayload> AutoAck<TPayload>(this IRabbitMQConsumerBuilder<TPayload> builder)
		{
			builder.ConsumerDeclaration.AutoAck = true;

			return builder;
		}

		/// <summary>
		///   Requeue message when fail to consume
		/// </summary>
		public static IRabbitMQConsumerBuilder<TPayload> Requeue<TPayload>(
			this IRabbitMQConsumerBuilder<TPayload> builder,
			bool multiple = false)
		{
			builder.ConsumerDeclaration.Requeue = true;
			builder.ConsumerDeclaration.Multiple = multiple;

			return builder;
		}

		public static IRabbitMQConsumerBuilder<TPayload> Priority<TPayload>(
			this IRabbitMQConsumerBuilder<TPayload> builder,
			byte priority)
		{
			// TODO: Hack, because RabbitMQ.Client has no conversion to byte
			return builder.Argument("x-priority", (int)priority);
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

			if (builder.ConsumerDeclaration.Arguments.ContainsKey(argument))
				throw new ArgumentException($"Argument {argument} already registered", nameof(argument));

			builder.ConsumerDeclaration.Arguments.Add(argument, value);

			return builder;
		}
	}
}