using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQProducerBuilderExtensions
	{
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
		public static IRabbitMQProducerBuilder WithTimeToLive(
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