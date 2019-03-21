using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQProducerGroupExtensions
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

			var connection = builder.ConnectionFactory.CreateConnection(groupName);

			group.Invoke(new RabbitMQProducerGroupBuilder(builder.Services, connection));
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