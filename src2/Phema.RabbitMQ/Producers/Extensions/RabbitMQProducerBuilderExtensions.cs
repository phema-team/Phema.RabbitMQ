using System;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static class RabbitMQProducerBuilderExtensions
	{
		/// <summary>
		///   Sets message routing key
		/// </summary>
		public static IRabbitMQProducerProvider WithRoutingKey(
			this IRabbitMQProducerProvider builder,
			string routingKey)
		{
			builder.Metadata.RoutingKey = routingKey;
			return builder;
		}

		/// <summary>
		///   Sets RabbitMQ arguments. Used for topic exchange
		/// </summary>
		public static IRabbitMQProducerProvider WithArgument<TValue>(
			this IRabbitMQProducerProvider builder,
			string argument,
			TValue value)
		{
			builder.Metadata.Arguments.Add(argument, value);
			return builder;
		}

		/// <summary>
		///   Sets messages as 'must be routed or return back'
		/// </summary>
		public static IRabbitMQProducerProvider Mandatory(this IRabbitMQProducerProvider builder)
		{
			builder.Metadata.Mandatory = true;
			return builder;
		}

		/// <summary>
		///   Sets channel mode to 'transactional'. Producer will commit or rollback messages
		/// </summary>
		public static IRabbitMQProducerProvider Transactional(this IRabbitMQProducerProvider builder)
		{
			builder.Metadata.Transactional = true;
			return builder;
		}
		
		/// <summary>
		///   Sets channel mode to 'confirm'. Producer will wait for delivery confirms
		/// </summary>
		public static IRabbitMQProducerProvider WaitForConfirms(this IRabbitMQProducerProvider builder,
			TimeSpan? timeout = null,
			bool die = true)
		{
			builder.Metadata.WaitForConfirms = true;
			builder.Metadata.Timeout = timeout;
			builder.Metadata.Die = die;
			return builder;
		}

		/// <summary>
		///   Sets producer property
		/// </summary>
		public static IRabbitMQProducerProvider WithProperty(
			this IRabbitMQProducerProvider builder,
			Action<IBasicProperties> property)
		{
			builder.Metadata.Properties.Add(property);
			return builder;
		}

		/// <summary>
		///   Sets message persistence
		/// </summary>
		public static IRabbitMQProducerProvider Persistent(this IRabbitMQProducerProvider builder)
		{
			return builder.WithProperty(x => x.Persistent = true);
		}

		/// <summary>
		///   Sets message priority. Means clamp(message-priority, 0, queue-max-priority)
		/// </summary>
		public static IRabbitMQProducerProvider WithPriority(
			this IRabbitMQProducerProvider builder,
			byte priority)
		{
			return builder.WithProperty(x => x.Priority = priority);
		}

		/// <summary>
		///   Sets message time to live
		/// </summary>
		public static IRabbitMQProducerProvider WithMessageTimeToLive(
			this IRabbitMQProducerProvider builder,
			int timeToLive)
		{
			return builder.WithProperty(x => x.Expiration = timeToLive.ToString());
		}

		/// <summary>
		///   Sets message header
		/// </summary>
		public static IRabbitMQProducerProvider WithHeader<TValue>(
			this IRabbitMQProducerProvider builder,
			string header,
			TValue value)
		{
			if (header is null)
				throw new ArgumentNullException(nameof(header));

			return builder.WithProperty(x =>
			{
				if (x.Headers.ContainsKey(header))
					throw new ArgumentException($"Header {header} already registered", nameof(header));

				x.Headers.Add(header, value);
			});
		}
	}
}