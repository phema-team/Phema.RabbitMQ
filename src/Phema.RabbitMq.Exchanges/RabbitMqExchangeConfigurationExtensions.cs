using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangeConfigurationExtensions
	{
		/// <summary>
		/// Sets alternate-exchange argument. When message can't be routed in current exchange,
		/// instead of mark as dead, publish to specified exchange
		/// </summary>
		public static IRabbitMQExchangeConfiguration WithAlternateExchange(
			this IRabbitMQExchangeConfiguration configuration,
			string exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.WithArgument("alternate-exchange", exchange);
		}
	}
}