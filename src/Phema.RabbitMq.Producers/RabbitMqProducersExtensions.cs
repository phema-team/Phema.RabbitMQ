using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQProducersExtensions
	{
		public static IRabbitMQConfiguration AddProducers(
			this IRabbitMQConfiguration configuration,
			Action<IRabbitMQProducersConfiguration> options)
		{
			options(new RabbitMQProducersConfiguration(configuration.Services));
			return configuration;
		}
	}
}