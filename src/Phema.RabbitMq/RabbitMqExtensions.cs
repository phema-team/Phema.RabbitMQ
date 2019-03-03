using System;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using RabbitMQ.Client;

namespace Phema.RabbitMq
{
	public static class RabbitMqExtensions
	{
		public static IRabbitMqConfiguration AddPhemaRabbitMq(
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

			return new RabbitMqConfiguration(services);
		}
	}
}