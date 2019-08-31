using System;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static class ServiceCollectionExtensions
	{
		/// <summary>
		/// Adds RabbitMQ services
		/// </summary>
		public static IRabbitMQBuilder AddRabbitMQ(
			this IServiceCollection services,
			Action<RabbitMQOptions> options)
		{
			services.Configure(options)
				.AddSingleton<IRabbitMQProducer, RabbitMQProducer>()
				.AddSingleton<IRabbitMQChannelProvider, RabbitMQChannelProvider>()
				.AddSingleton<IRabbitMQConnectionProvider, RabbitMQConnectionProvider>()
				.AddHostedService<RabbitMQConnectionHostedService>()
				.AddHostedService<RabbitMQExchangeHostedService>()
				.AddHostedService<RabbitMQQueueHostedService>()
				.AddHostedService<RabbitMQConsumerHostedService>();

			return new RabbitMQBuilder(services);
		}

		/// <summary>
		/// Adds RabbitMQ services
		/// </summary>
		public static IRabbitMQBuilder AddRabbitMQ(
			this IServiceCollection services,
			string clientProvidedName,
			Action<ConnectionFactory> factory)
		{
			return services.AddRabbitMQ(options =>
			{
				options.ConnectionFactory.ClientProvidedName = clientProvidedName;
				factory?.Invoke(options.ConnectionFactory);
			});
		}

		/// <summary>
		/// Adds RabbitMQ services
		/// </summary>
		public static IRabbitMQBuilder AddRabbitMQ(
			this IServiceCollection services,
			string clientProvidedName,
			string url)
		{
			return services.AddRabbitMQ(clientProvidedName, factory =>
			{
				if (url != null)
					factory.Uri = new Uri(url);
			});
		}

		/// <summary>
		/// Adds RabbitMQ services
		/// </summary>
		public static IRabbitMQBuilder AddRabbitMQ(
			this IServiceCollection services,
			string clientProvidedName = null)
		{
			return services.AddRabbitMQ(clientProvidedName, factory => {});
		}
	}
}