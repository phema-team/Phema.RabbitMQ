using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangeBuilderExtensions
	{
		/// <summary>
		/// Sets alternate-exchange argument. When message can't be routed in current exchange,
		/// instead of mark as dead, publish to specified exchange
		/// </summary>
		public static IRabbitMQExchangeBuilder WithAlternateExchange(
			this IRabbitMQExchangeBuilder configuration,
			string exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.WithArgument("alternate-exchange", exchange);
		}
	}
}