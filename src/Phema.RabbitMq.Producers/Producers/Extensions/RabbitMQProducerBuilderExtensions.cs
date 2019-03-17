using System;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static class RabbitMQProducerBuilderExtensions
	{
		/// <summary>
		/// Sets message routing key
		/// </summary>
		public static IRabbitMQProducerBuilder WithRoutingKey(
			this IRabbitMQProducerBuilder builder,
			string routingKey)
		{
			builder.Metadata.RoutingKey = routingKey;
			return builder;
		}

		/// <summary>
		/// Sets RabbitMQ arguments. Allow multiple
		/// </summary>
		public static IRabbitMQProducerBuilder WithArgument<TValue>(
			this IRabbitMQProducerBuilder builder,
			string argument,
			TValue value)
		{
			builder.Metadata.Arguments.Add(argument, value);
			return builder;
		}

		/// <summary>
		/// Sets messages as 'must be routed'
		/// </summary>
		public static IRabbitMQProducerBuilder Mandatory(this IRabbitMQProducerBuilder builder)
		{
			builder.Metadata.Mandatory = true;
			return builder;
		}

		/// <summary>
		/// Sets producer property
		/// </summary>
		public static IRabbitMQProducerBuilder WithProperty(
			this IRabbitMQProducerBuilder builder,
			Action<IBasicProperties> property)
		{
			builder.Metadata.Properties.Add(property);
			return builder;
		}

		/// <summary>
		/// Sets message persistence
		/// </summary>
		public static IRabbitMQProducerBuilder Persistent(this IRabbitMQProducerBuilder builder)
		{
			return builder.WithProperty(x => x.Persistent = true);
		}

		/// <summary>
		/// Sets message priority. Means clamp(message-priority, 0, queue-max-priority)
		/// </summary>
		public static IRabbitMQProducerBuilder WithPriority(
			this IRabbitMQProducerBuilder builder,
			byte priority)
		{
			return builder.WithProperty(x => x.Priority = priority);
		}

		/// <summary>
		/// Sets message time to live
		/// </summary>
		public static IRabbitMQProducerBuilder WithMessageTimeToLive(
			this IRabbitMQProducerBuilder builder,
			int timeToLive)
		{
			return builder.WithProperty(x => x.Expiration = timeToLive.ToString());
		}

		/// <summary>
		/// Sets message header
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
	}
}