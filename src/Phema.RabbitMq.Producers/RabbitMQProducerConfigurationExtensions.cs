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
	}
}