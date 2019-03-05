using System;

using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangesConfigurationExtensions
	{
		/// <summary>
		/// Sets exchange type to direct
		/// </summary>
		public static IRabbitMQExchangeConfiguration AddDirectExchange(
			this IRabbitMQExchangesConfiguration configuration,
			string exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));
			
			return configuration.AddExchange(ExchangeType.Direct, exchange);
		}

		/// <summary>
		/// Sets exchange type to fanout
		/// </summary>
		public static IRabbitMQExchangeConfiguration AddFanoutExchange(
			this IRabbitMQExchangesConfiguration configuration,
			string exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));
			
			return configuration.AddExchange(ExchangeType.Fanout, exchange);
		}

		/// <summary>
		/// Sets exchange type to topic
		/// </summary>
		public static IRabbitMQExchangeConfiguration AddTopicExchange(
			this IRabbitMQExchangesConfiguration configuration,
			string exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));
			
			return configuration.AddExchange(ExchangeType.Topic, exchange);
		}

		/// <summary>
		/// Sets exchange type to headers
		/// </summary>
		public static IRabbitMQExchangeConfiguration AddHeadersExchange(
			this IRabbitMQExchangesConfiguration configuration,
			string exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));
			
			return configuration.AddExchange(ExchangeType.Headers, exchange);
		}
	}
}