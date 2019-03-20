using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQProducersExtensions
	{
		/// <summary>
		///   Adds new producers group in separate connection
		/// </summary>
		public static IRabbitMQBuilder AddProducers(
			this IRabbitMQBuilder builder,
			string groupName,
			Action<IRabbitMQProducersBuilder> options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));

			var connection = builder.ConnectionFactory.CreateConnection(groupName);

			options.Invoke(new RabbitMQProducersBuilder(builder.Services, connection));
			return builder;
		}

		/// <summary>
		///   Adds default producers group in separate connection
		/// </summary>
		public static IRabbitMQBuilder AddProducers(
			this IRabbitMQBuilder builder,
			Action<IRabbitMQProducersBuilder> options)
		{
			return builder.AddProducers(null, options);
		}
	}
}