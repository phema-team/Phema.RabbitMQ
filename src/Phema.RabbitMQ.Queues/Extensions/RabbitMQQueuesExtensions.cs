using System;
using Microsoft.Extensions.DependencyInjection;
using Phema.RabbitMQ.Internal;

namespace Phema.RabbitMQ
{
	public static class RabbitMQQueuesExtensions
	{
		/// <summary>
		///   Adds new queue group
		/// </summary>
		public static IRabbitMQBuilder AddQueueGroup(
			this IRabbitMQBuilder builder,
			string groupName,
			Action<IRabbitMQQueueGroupBuilder> options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));
			
			if (groupName is null)
				throw new ArgumentNullException(nameof(groupName));

			var services = builder.Services;
			
			options.Invoke(new RabbitMQQueueGroupBuilder(services, groupName));

			services.AddHostedService<RabbitMQQueueHostedService>();

			return builder;
		}
		
		/// <summary>
		///   Adds default queue group
		/// </summary>
		public static IRabbitMQBuilder AddQueueGroup(
			this IRabbitMQBuilder builder,
			Action<IRabbitMQQueueGroupBuilder> options)
		{
			return builder.AddQueueGroup(RabbitMQDefaults.DefaultGroupName, options);
		}
	}
}