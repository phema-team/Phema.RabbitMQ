using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Phema.RabbitMQ.Internal;

namespace Phema.RabbitMQ
{
	public static class RabbitMQConsumersExtensions
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

			group.Invoke(new RabbitMQConsumerGroupBuilder(services, groupName));

			services.TryAddSingleton<IRabbitMQConsumerFactory, RabbitMQConsumerFactory>();

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