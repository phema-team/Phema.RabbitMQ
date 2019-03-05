using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQProducerConfigurationExtensions
	{
		/// <summary>
		/// Sets message persistence
		/// </summary>
		public static IRabbitMQProducerConfiguration Persistent(this IRabbitMQProducerConfiguration configuration)
		{
			return configuration.WithProperties(x => x.Persistent = true);
		}
		
		/// <summary>
		/// Sets message priority. Means clamp(message-priority, 0, queue-max-priority)
		/// </summary>
		public static IRabbitMQProducerConfiguration WithPriority(
			this IRabbitMQProducerConfiguration configuration,
			byte priority)
		{
			return configuration.WithProperties(x => x.Priority = priority);
		}
		
		/// <summary>
		/// Sets message time to live
		/// </summary>
		public static IRabbitMQProducerConfiguration WithTimeToLive(
			this IRabbitMQProducerConfiguration configuration,
			int timeToLive)
		{
			return configuration.WithProperties(x => x.Expiration = timeToLive.ToString());
		}
		
		/// <summary>
		/// Sets message header
		/// </summary>
		public static IRabbitMQProducerConfiguration WithHeader<TValue>(
			this IRabbitMQProducerConfiguration configuration,
			string header,
			TValue value)
		{
			if (header is null)
				throw new ArgumentNullException(nameof(header));
			
			return configuration.WithProperties(x =>
			{
				if (x.Headers.ContainsKey(header))
					throw new ArgumentException($"Header {header} already registered", nameof(header));
				
				x.Headers.Add(header, value);
			});
		}
	}
}