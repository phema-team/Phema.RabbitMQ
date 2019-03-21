using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQQueueGroupExtensions
	{
		/// <summary>
		///   Adds new queue group
		/// </summary>
		public static IRabbitMQBuilder AddQueueGroup(
			this IRabbitMQBuilder builder,
			string groupName,
			Action<IRabbitMQQueuesBuilder> options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));
			
			if (groupName is null)
				throw new ArgumentNullException(nameof(groupName));

			options.Invoke(new RabbitMQQueuesBuilder(builder.Services, groupName));
			return builder;
		}
		
		/// <summary>
		///   Adds default queue group
		/// </summary>
		public static IRabbitMQBuilder AddQueueGroup(
			this IRabbitMQBuilder builder,
			Action<IRabbitMQQueuesBuilder> options)
		{
			return builder.AddQueueGroup(RabbitMQDefaults.DefaultGroupName, options);
		}
	}
}