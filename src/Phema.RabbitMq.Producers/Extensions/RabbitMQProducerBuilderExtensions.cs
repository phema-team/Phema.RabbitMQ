using System;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static class RabbitMQProducerBuilderExtensions
	{
		/// <summary>
		///   Declare message routing key
		/// </summary>
		public static IRabbitMQProducerBuilder RoutingKey(
			this IRabbitMQProducerBuilder builder,
			string routingKey)
		{
			builder.Declaration.RoutingKey = routingKey;
			return builder;
		}

		/// <summary>
		///   Produce messages as mandatory
		/// </summary>
		public static IRabbitMQProducerBuilder Mandatory(this IRabbitMQProducerBuilder builder)
		{
			builder.Declaration.Mandatory = true;
			return builder;
		}

		/// <summary>
		///   Declare channel mode as 'transactional'. Producer will commit or rollback messages
		/// </summary>
		public static IRabbitMQProducerBuilder Transactional(this IRabbitMQProducerBuilder builder)
		{
			builder.Declaration.Transactional = true;
			return builder;
		}
		
		/// <summary>
		///   Declare channel mode as 'confirm'. Producer will wait for delivery confirms
		/// </summary>
		public static IRabbitMQProducerBuilder WaitForConfirms(this IRabbitMQProducerBuilder builder,
			TimeSpan? timeout = null,
			bool die = true)
		{
			builder.Declaration.WaitForConfirms = true;
			builder.Declaration.Timeout = timeout;
			builder.Declaration.Die = die;
			return builder;
		}

		/// <summary>
		///   Declare message persistence
		/// </summary>
		public static IRabbitMQProducerBuilder Persistent(this IRabbitMQProducerBuilder builder)
		{
			return builder.WithProperty(x => x.Persistent = true);
		}

		/// <summary>
		///   Declare message priority. Means clamp(message-priority, 0, queue-max-priority)
		/// </summary>
		public static IRabbitMQProducerBuilder Priority(
			this IRabbitMQProducerBuilder builder,
			byte priority)
		{
			return builder.WithProperty(x => x.Priority = priority);
		}

		/// <summary>
		///   Declare message time to live
		/// </summary>
		public static IRabbitMQProducerBuilder MessageTimeToLive(
			this IRabbitMQProducerBuilder builder,
			int timeToLive)
		{
			return builder.WithProperty(x => x.Expiration = timeToLive.ToString());
		}

		/// <summary>
		///   Declare producer property
		/// </summary>
		public static IRabbitMQProducerBuilder WithProperty(
			this IRabbitMQProducerBuilder builder,
			Action<IBasicProperties> property)
		{
			builder.Declaration.Properties.Add(property);
			return builder;
		}

		/// <summary>
		///   Declare message header
		/// </summary>
		public static IRabbitMQProducerBuilder WithHeader<TValue>(
			this IRabbitMQProducerBuilder builder,
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
		
		/// <summary>
		///   Declare RabbitMQ arguments. Used for topic exchange
		/// </summary>
		public static IRabbitMQProducerBuilder WithArgument<TValue>(
			this IRabbitMQProducerBuilder builder,
			string argument,
			TValue value)
		{
			builder.Declaration.Arguments.Add(argument, value);
			return builder;
		}
	}
}