using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Phema.RabbitMQ
{
	public static class RabbitMQConsumersExtensions
	{
		/// <summary>
		/// Adds new consumers group in separate connection
		/// </summary>
		public static IRabbitMQBuilder AddConsumers(
			this IRabbitMQBuilder builder,
			string groupName,
			Action<IRabbitMQConsumersBuilder> options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));

			var services = builder.Services;

			var connection = builder.ConnectionFactory.CreateConnection(groupName);
			
			options.Invoke(new RabbitMQConsumersBuilder(services, connection, groupName));

			services.AddHostedService<RabbitMQConsumersHostedService>();
			services.TryAddScoped<IRabbitMQConsumerFactory, RabbitMQConsumerFactory>();

			return builder;
		}

		/// <summary>
		/// Adds default consumers group in separate connection
		/// </summary>
		public static IRabbitMQBuilder AddConsumers(
			this IRabbitMQBuilder builder,
			Action<IRabbitMQConsumersBuilder> options)
		{
			return builder.AddConsumers(null, options);
		}
	}
}