using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQQueueExtensions
	{
		public static IRabbitMQConfiguration AddQueues(
			this IRabbitMQConfiguration configuration,
			Action<IRabbitMQQueuesConfiguration> options)
		{
			options.Invoke(new RabbitMQQueuesConfiguration(configuration.Services));
			return configuration;
		}
	}
}