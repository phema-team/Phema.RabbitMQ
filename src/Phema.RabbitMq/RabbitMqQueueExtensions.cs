using System;

namespace Phema.RabbitMq
{
	public static class RabbitMqQueueExtensions
	{
		public static IRabbitMqConfiguration AddQueues(
		this IRabbitMqConfiguration configuration,
		Action<IRabbitMqQueuesConfiguration> options = null)
		{
			options?.Invoke(new RabbitMqQueuesConfiguration(configuration.Services));
			return configuration;
		}
	}
}