using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public sealed class RabbitMQOptions
	{
		public RabbitMQOptions()
		{
			ConnectionFactory = new ConnectionFactory
			{
				DispatchConsumersAsync = true
			};
		}

		public string InstanceName { get; set; }
		public ConnectionFactory ConnectionFactory { get; }

		internal void Deconstruct(out string instanceName, out ConnectionFactory connectionFactory)
		{
			instanceName = InstanceName;
			connectionFactory = ConnectionFactory;
		}
	}

	public static class RabbitMQExtensions
	{
		public static IRabbitMQBuilder AddPhemaRabbitMQ(
			this IServiceCollection services,
			Action<RabbitMQOptions> options = null)
		{
			services.Configure(options ?? (o => { }));

			services.TryAddSingleton(provider =>
			{
				var (instanceName, connectionFactory) = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value;
				return connectionFactory.CreateConnection(instanceName);
			});

			return new RabbitMQBuilder(services);
		}
		
		public static IRabbitMQBuilder AddPhemaRabbitMQ(
			this IServiceCollection services,
			string instanceName,
			Action<ConnectionFactory> factory = null)
		{
			return services.AddPhemaRabbitMQ(options =>
			{
				options.InstanceName = instanceName;
				factory?.Invoke(options.ConnectionFactory);
			});
		}
	}
}