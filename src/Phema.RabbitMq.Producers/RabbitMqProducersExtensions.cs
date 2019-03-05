using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQProducersExtensions
	{
		public static IRabbitMQConfiguration AddProducers(
			this IRabbitMQConfiguration configuration,
			Action<IRabbitMQProducersConfiguration> options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));
			
			options.Invoke(new RabbitMQProducersConfiguration(configuration.Services));
			return configuration;
		}
	}
}