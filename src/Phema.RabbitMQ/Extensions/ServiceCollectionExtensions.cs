using System;
using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds RabbitMQ services
		/// </summary>
		public static IRabbitMQBuilder AddRabbitMQ(
			this IServiceCollection services,
			Action<RabbitMQOptions> options = null)
		{
			services.Configure(options ?? (o => {}))
				.AddSingleton<IRabbitMQProducer, RabbitMQProducer>()
				.AddSingleton<IRabbitMQChannelProvider, RabbitMQChannelProvider>()
				.AddSingleton<IRabbitMQConnectionProvider, RabbitMQConnectionProvider>()
				.AddHostedService<RabbitMQExchangeHostedService>()
				.AddHostedService<RabbitMQQueueHostedService>()
				.AddHostedService<RabbitMQConsumerHostedService>();

			return new RabbitMQBuilder(services);
		}
	}
}