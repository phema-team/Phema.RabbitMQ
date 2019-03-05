using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangesExtensions
	{
		public static IRabbitMQConfiguration AddExchanges(
			this IRabbitMQConfiguration configuration,
			Action<IRabbitMQExchangesConfiguration> options)
		{
			options(new RabbitMQExchangesConfiguration(configuration.Services));
			return configuration;
		}
	}
}