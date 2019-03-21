using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangeGroupExtensions
	{
		/// <summary>
		///   Adds new exchange group
		/// </summary>
		public static IRabbitMQBuilder AddExchangeGroup(
			this IRabbitMQBuilder builder,
			string groupName,
			Action<IRabbitMQExchangesBuilder> options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));
			
			if (groupName is null)
				throw new ArgumentNullException(nameof(groupName));

			options.Invoke(new RabbitMQExchangesBuilder(builder.Services, groupName));
			return builder;
		}

		/// <summary>
		///   Adds default exchange group
		/// </summary>
		public static IRabbitMQBuilder AddExchangeGroup(
			this IRabbitMQBuilder builder,
			Action<IRabbitMQExchangesBuilder> options)
		{
			return builder.AddExchangeGroup(RabbitMQDefaults.DefaultGroupName, options);
		}
	}
}