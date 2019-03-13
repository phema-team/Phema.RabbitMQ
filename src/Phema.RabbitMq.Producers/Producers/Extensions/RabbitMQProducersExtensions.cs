using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQProducersExtensions
	{
		public static IRabbitMQBuilder AddProducers(
			this IRabbitMQBuilder builder,
			Action<IRabbitMQProducersBuilder> options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));

			options.Invoke(new RabbitMQProducersBuilder(builder.Services));
			return builder;
		}
	}
}