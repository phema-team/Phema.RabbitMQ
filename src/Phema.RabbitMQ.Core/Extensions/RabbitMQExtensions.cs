using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExtensions
	{
		public static IRabbitMQBuilder AddPhemaRabbitMQ(
			this IServiceCollection services,
			string instanceName,
			Action<IConnectionFactory> options = null)
		{
			var factory = new ConnectionFactory
			{
				DispatchConsumersAsync = true
			};
			
			options?.Invoke(factory);

			var connectionFactory = new RabbitMQConnectionFactory(instanceName, factory);
			
			services.TryAddSingleton<IRabbitMQConnectionFactory>(connectionFactory);
			
			return new RabbitMQBuilder(services, connectionFactory);
		}

		public static IRabbitMQBuilder AddPhemaRabbitMQ(
			this IServiceCollection services,
			string instanceName,
			string connectionString)
		{
			if (connectionString is null)
				throw new ArgumentNullException(nameof(connectionString));

			return services.AddPhemaRabbitMQ(
				instanceName,
				factory => factory.Uri = new Uri(connectionString));
		}
	}
}