using System;
using System.Linq;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static class RabbitMQProducerBuilderExtensions
	{
		/// <summary>
		///   Sets routing key to queue name or queue binding routing key
		/// </summary>
		public static IRabbitMQProducerBuilder<TPayload> RoutedTo<TPayload>(
			this IRabbitMQProducerBuilder<TPayload> builder,
			IRabbitMQQueueBuilder<TPayload> queue)
		{
			var binding = queue.Declaration
				.BindingDeclarations
				.FirstOrDefault(b => b.ExchangeDeclaration == builder.ProducerDeclaration.ExchangeDeclaration);

			builder.ProducerDeclaration.RoutingKey = binding?.RoutingKey ?? queue.Declaration.Name;
			return builder;
		}

		/// <summary>
		///   Declare message routing key
		/// </summary>
		public static IRabbitMQProducerBuilder<TPayload> RoutedTo<TPayload>(
			this IRabbitMQProducerBuilder<TPayload> builder,
			string routingKey)
		{
			builder.ProducerDeclaration.RoutingKey = routingKey;
			return builder;
		}

		/// <summary>
		///   Produce messages as mandatory
		/// </summary>
		public static IRabbitMQProducerBuilder<TPayload> Mandatory<TPayload>(
			this IRabbitMQProducerBuilder<TPayload> builder)
		{
			builder.ProducerDeclaration.Mandatory = true;
			return builder;
		}

		/// <summary>
		///   Declare channel mode as 'transactional'. Producer will commit or rollback messages
		/// </summary>
		public static IRabbitMQProducerBuilder<TPayload> Transactional<TPayload>(
			this IRabbitMQProducerBuilder<TPayload> builder)
		{
			builder.ProducerDeclaration.Transactional = true;
			return builder;
		}

		/// <summary>
		///   Declare channel mode as 'confirm'. Producer will wait for delivery confirms
		/// </summary>
		public static IRabbitMQProducerBuilder<TPayload> WaitForConfirms<TPayload>(
			this IRabbitMQProducerBuilder<TPayload> builder,
			TimeSpan? timeout = null,
			bool die = true)
		{
			builder.ProducerDeclaration.WaitForConfirms = true;
			builder.ProducerDeclaration.Timeout = timeout;
			builder.ProducerDeclaration.Die = die;

			return builder;
		}

		/// <summary>
		///   Declare message persistence
		/// </summary>
		public static IRabbitMQProducerBuilder<TPayload> Persistent<TPayload>(
			this IRabbitMQProducerBuilder<TPayload> builder)
		{
			return builder.Property(x => x.Persistent = true);
		}

		/// <summary>
		///   Declare message priority. Means clamp(message-priority, 0, queue-max-priority)
		/// </summary>
		public static IRabbitMQProducerBuilder<TPayload> Priority<TPayload>(
			this IRabbitMQProducerBuilder<TPayload> builder,
			byte priority)
		{
			return builder.Property(x => x.Priority = priority);
		}

		/// <summary>
		///   Declare message time to live
		/// </summary>
		public static IRabbitMQProducerBuilder<TPayload> MessageTimeToLive<TPayload>(
			this IRabbitMQProducerBuilder<TPayload> builder,
			TimeSpan timeToLive)
		{
			return builder.Property(x => x.Expiration = timeToLive.Milliseconds.ToString());
		}

		/// <summary>
		///   Declare message app id
		/// </summary>
		public static IRabbitMQProducerBuilder<TPayload> AppId<TPayload>(
			this IRabbitMQProducerBuilder<TPayload> builder,
			string appId)
		{
			if (appId is null)
				throw new ArgumentNullException(nameof(appId));

			return builder.Property(p => p.AppId = appId);
		}

		/// <summary>
		///   Declare producer property
		/// </summary>
		public static IRabbitMQProducerBuilder<TPayload> Property<TPayload>(
			this IRabbitMQProducerBuilder<TPayload> builder,
			Action<IBasicProperties> property)
		{
			builder.ProducerDeclaration.Properties.Add(property);
			return builder;
		}

		/// <summary>
		///   Declare message header
		/// </summary>
		public static IRabbitMQProducerBuilder<TPayload> Header<TPayload>(
			this IRabbitMQProducerBuilder<TPayload> builder,
			string header,
			object value)
		{
			if (header is null)
				throw new ArgumentNullException(nameof(header));

			return builder.Property(x =>
			{
				if (x.Headers.ContainsKey(header))
					throw new ArgumentException($"Header '{header}' already registered", nameof(header));

				x.Headers.Add(header, value);
			});
		}

		/// <summary>
		///   Declare RabbitMQ arguments
		/// </summary>
		public static IRabbitMQProducerBuilder<TPayload> Argument<TPayload>(
			this IRabbitMQProducerBuilder<TPayload> builder,
			string argument,
			object value)
		{
			builder.ProducerDeclaration.Arguments.Add(argument, value);
			return builder;
		}
	}
}