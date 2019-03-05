namespace Phema.RabbitMQ
{
	public static class RabbitMQConsumerConfigurationExtensions
	{
		public static IRabbitMQConsumerConfiguration WithPriority(
			this IRabbitMQConsumerConfiguration configuration,
			byte priority)
		{
			return configuration.WithArgument("x-priority", priority);
		}
	}
}