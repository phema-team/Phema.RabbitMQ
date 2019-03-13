namespace Phema.RabbitMQ
{
	public static class RabbitMQConsumerBuilderExtensions
	{
		public static IRabbitMQConsumerBuilder WithPriority(
			this IRabbitMQConsumerBuilder configuration,
			byte priority)
		{
			return configuration.WithArgument("x-priority", priority);
		}
	}
}