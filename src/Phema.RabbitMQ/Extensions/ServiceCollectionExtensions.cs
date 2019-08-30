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
				.AddSingleton<IRabbitMQChannelCache, RabbitMQChannelCache>()
				.AddSingleton<IRabbitMQConnectionCache, RabbitMQConnectionCache>()

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
			string instanceName,
			Action<ConnectionFactory> factory)
		{
			return services.AddRabbitMQ(options =>
			{
				options.InstanceName = instanceName;
				factory?.Invoke(options.ConnectionFactory);
			});
		}

		/// <summary>
		/// Adds RabbitMQ services
		/// </summary>
		public static IRabbitMQBuilder AddRabbitMQ(
			this IServiceCollection services,
			string instanceName,
			string url)
		{
			return services.AddRabbitMQ(instanceName, factory =>
			{
				if (url != null)
				{
					factory.Uri = new Uri(url);
				}
			});
		}

		/// <summary>
		/// Adds RabbitMQ services
		/// </summary>
		public static IRabbitMQBuilder AddRabbitMQ(
			this IServiceCollection services,
			string instanceName = null)
		{
			return services.AddRabbitMQ(instanceName, factory => {});
		}
	}
}