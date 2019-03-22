using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExtensions
	{
		public static IServiceCollection AddPhemaRabbitMQ(
			this IServiceCollection services,
			string instanceName,
			Action<ConnectionFactory> factory)
		{
			services.TryAddSingleton<IRabbitMQProvider, RabbitMQProvider>();
			services.TryAddSingleton<IRabbitMQConnectionFactory>(new RabbitMQConnectionFactory(instanceName, factory));

			services.TryAddSingleton<IRabbitMQConsumerFactory, RabbitMQConsumerFactory>();

			services.TryAddSingleton(typeof(IRabbitMQProducer<>), typeof(RabbitMQProducer<>));

			return services;
		}
	}
}