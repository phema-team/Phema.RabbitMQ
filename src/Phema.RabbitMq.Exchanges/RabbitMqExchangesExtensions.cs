using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangesExtensions
	{
		public static IRabbitMQConfiguration AddExchanges(
			this IRabbitMQConfiguration configuration,
			Action<IRabbitMQExchangesConfiguration> options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));
			
			options.Invoke(new RabbitMQExchangesConfiguration(configuration.Services));
			return configuration;
		}
	}
}