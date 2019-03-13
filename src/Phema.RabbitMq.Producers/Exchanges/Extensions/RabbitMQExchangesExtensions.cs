using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangesExtensions
	{
		public static IRabbitMQBuilder AddExchanges(
			this IRabbitMQBuilder builder,
			Action<IRabbitMQExchangesBuilder> options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));

			options.Invoke(new RabbitMQExchangesBuilder(builder.Services));
			return builder;
		}
	}
}