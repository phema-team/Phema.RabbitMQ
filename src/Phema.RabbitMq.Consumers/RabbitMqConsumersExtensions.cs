using System;

using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMq
{
	public static class RabbitMqConsumersExtensions
	{
		public static IRabbitMqConfiguration AddConsumers(
			this IRabbitMqConfiguration configuration,
			Action<IRabbitMqConsumersConfiguration> options)
		{
			options(new RabbitMqConsumersConfiguration(configuration.Services));

			configuration.Services.AddHostedService<RabbitMqConsumersHostedService>();
			
			return configuration;
		}
	}
}