using System;
using Microsoft.Extensions.DependencyInjection;
using Phema.RabbitMQ.Internal;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangesExtensions
	{
		/// <summary>
		///   Adds new exchange group
		/// </summary>
		public static IRabbitMQBuilder AddExchangeGroup(
			this IRabbitMQBuilder builder,
			string groupName,
			Action<IRabbitMQExchangeGroupBuilder> options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));
			
			if (groupName is null)
				throw new ArgumentNullException(nameof(groupName));

			var services = builder.Services;
			
			options.Invoke(new RabbitMQExchangeGroupBuilder(services, groupName));

			services.AddHostedService<RabbitMQExchangeHostedService>();

			return builder;
		}

		/// <summary>
		///   Adds default exchange group
		/// </summary>
		public static IRabbitMQBuilder AddExchangeGroup(
			this IRabbitMQBuilder builder,
			Action<IRabbitMQExchangeGroupBuilder> options)
		{
			return builder.AddExchangeGroup(RabbitMQDefaults.DefaultGroupName, options);
		}
	}
}