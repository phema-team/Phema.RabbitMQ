using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQQueueConfigurationExtensions
	{
		/// <summary>
		/// Sets x-max-length argument. When queue message limit reached, see <see cref="RabbitMQQueueConfigurationExtensions.WithRejectPublishOnOverflow"/>
		/// </summary>
		public static IRabbitMQQueueConfiguration WithMaxMessageCount(
			this IRabbitMQQueueConfiguration configuration,
			int length)
		{
			return configuration.WithArgument("x-max-length", length);
		}
		
		/// <summary>
		/// Sets x-max-length-bytes argument. When size limit reached, message will be marked as dead
		/// </summary>
		public static IRabbitMQQueueConfiguration WithMaxMessageSize(
			this IRabbitMQQueueConfiguration configuration,
			int bytes)
		{
			return configuration.WithArgument("x-max-length-bytes", bytes);
		}
		
		/// <summary>
		/// Sets x-dead-letter-exchange argument. When message is dead, send to x-dead-letter-exchange
		/// </summary>
		public static IRabbitMQQueueConfiguration WithDeadLetterExchange(
			this IRabbitMQQueueConfiguration configuration,
			string exchangeName)
		{
			if (exchangeName is null)
				throw new ArgumentNullException(nameof(exchangeName));
			
			return configuration.WithArgument("x-dead-letter-exchange", exchangeName);
		}
		
		
		/// <summary>
		/// Sets x-dead-letter-routing-key argument. When message is dead, send to x-dead-letter-exchange with routing key
		/// </summary>
		public static IRabbitMQQueueConfiguration WithDeadLetterRoutingKey(
			this IRabbitMQQueueConfiguration configuration,
			string routingKey)
		{
			if (routingKey is null)
				throw new ArgumentNullException(nameof(routingKey));
			
			return configuration.WithArgument("x-dead-letter-routing-key", routingKey);
		}
		
		/// <summary>
		/// Sets x-message-ttl argument. When expires message will be marked as dead
		/// </summary>
		public static IRabbitMQQueueConfiguration WithMessageTimeToLive(
			this IRabbitMQQueueConfiguration configuration,
			int timeToLive)
		{
			return configuration.WithArgument("x-message-ttl", timeToLive);
		}
		
		/// <summary>
		/// Sets x-expires argument for queue. When expires queue will be deleted
		/// </summary>
		public static IRabbitMQQueueConfiguration WithTimeToLive(
			this IRabbitMQQueueConfiguration configuration,
			int milliseconds)
		{
			return configuration.WithArgument("x-expires", milliseconds);
		}
		
		/// <summary>
		/// Sets x-max-priority argument for queue (default 0)
		/// </summary>
		public static IRabbitMQQueueConfiguration WithMaxPriority(
			this IRabbitMQQueueConfiguration configuration,
			byte priority)
		{
			return configuration.WithArgument("x-max-priority", priority);
		}

		/// <summary>
		/// Sets x-overflow argument to reject-publish (default drop-head)
		/// </summary>
		public static IRabbitMQQueueConfiguration WithRejectPublishOnOverflow(
			this IRabbitMQQueueConfiguration configuration)
		{
			return configuration.WithArgument("x-overflow", "reject-publish");
		}
	}
}