using System;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Phema.RabbitMQ
{
	public static class RabbitMQProducersExtensions
	{
		/// <summary>
		///   Adds new producers group in separate connection
		/// </summary>
		public static IRabbitMQBuilder AddProducerGroup(
			this IRabbitMQBuilder builder,
			string groupName,
			Action<IRabbitMQProducerGroupBuilder> group)
		{
			if (group is null)
				throw new ArgumentNullException(nameof(group));

			if (groupName is null)
				throw new ArgumentNullException(nameof(groupName));

			var services = builder.Services;

			group.Invoke(new RabbitMQProducerGroupBuilder(services, groupName));

			services.TryAddSingleton<IRabbitMQProducerFactory, RabbitMQProducerFactory>();

			return builder;
		}

		/// <summary>
		///   Adds default producers group in separate connection
		/// </summary>
		public static IRabbitMQBuilder AddProducerGroup(
			this IRabbitMQBuilder builder,
			Action<IRabbitMQProducerGroupBuilder> options)
		{
			return builder.AddProducerGroup(RabbitMQDefaults.DefaultGroupName, options);
		}
	}
}