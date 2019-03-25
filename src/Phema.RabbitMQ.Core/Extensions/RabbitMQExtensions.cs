using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExtensions
	{
		public static IRabbitMQBuilder AddRabbitMQ(this IServiceCollection services, Action<RabbitMQOptions> options)
		{
			services.Configure<RabbitMQOptions>(options.Invoke);

			services.TryAddSingleton<IRabbitMQConnectionFactory, RabbitMQConnectionFactory>();

			return new RabbitMQBuilder(services);
		}

		public static IRabbitMQBuilder AddRabbitMQ(
			this IServiceCollection services,
			string instanceName,
			Action<ConnectionFactory> factory)
		{
			return services.AddRabbitMQ(options =>
			{
				options.InstanceName = instanceName;
				factory.Invoke(options.ConnectionFactory);
			});
		}
		
		public static IRabbitMQBuilder AddRabbitMQ(
			this IServiceCollection services,
			string instanceName,
			string connectionString)
		{
			return services.AddRabbitMQ(options =>
			{
				options.InstanceName = instanceName;
				options.ConnectionFactory.Uri = new Uri(connectionString);
			});
		}
	}
}