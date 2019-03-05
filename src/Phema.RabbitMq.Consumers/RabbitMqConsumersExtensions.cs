using System;

using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public static class RabbitMQConsumersExtensions
	{
		public static IRabbitMQConfiguration AddConsumers(
			this IRabbitMQConfiguration configuration,
			Action<IRabbitMQConsumersConfiguration> options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));
			
			options.Invoke(new RabbitMQConsumersConfiguration(configuration.Services));

			configuration.Services.AddHostedService<RabbitMQConsumersHostedService>();

			return configuration;
		}
	}
}