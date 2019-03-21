using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Phema.RabbitMQ
{
	public static class RabbitMQConsumerGroupExtensions
	{
		/// <summary>
		///   Adds new consumers group in separate connection
		/// </summary>
		public static IRabbitMQBuilder AddConsumerGroup(
			this IRabbitMQBuilder builder,
			string groupName,
			Action<IRabbitMQConsumerGroupBuilder> group)
		{
			if (group is null)
				throw new ArgumentNullException(nameof(group));
			
			if (groupName is null)
				throw new ArgumentNullException(nameof(groupName));

			var services = builder.Services;

			var connection = builder.ConnectionFactory.CreateConnection(groupName);

			group.Invoke(new RabbitMQConsumerGroupBuilder(services, connection));

			services.AddHostedService<RabbitMQConsumersHostedService>();
			services.TryAddScoped<IRabbitMQConsumerFactory, RabbitMQConsumerFactory>();

			return builder;
		}

		/// <summary>
		///   Adds default consumers group in separate connection
		/// </summary>
		public static IRabbitMQBuilder AddConsumerGroup(
			this IRabbitMQBuilder builder,
			Action<IRabbitMQConsumerGroupBuilder> options)
		{
			return builder.AddConsumerGroup("", options);
		}
	}
}