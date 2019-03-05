using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExtensions
	{
		public static IRabbitMQConfiguration AddPhemaRabbitMQ(
			this IServiceCollection services,
			string instanceName,
			Action<ConnectionFactory> options = null)
		{
			services.TryAddSingleton(sp =>
			{
				var factory = new ConnectionFactory
				{
					DispatchConsumersAsync = true
				};

				options?.Invoke(factory);

				return factory.CreateConnection(instanceName);
			});

			return new RabbitMQConfiguration(services);
		}
	}
}