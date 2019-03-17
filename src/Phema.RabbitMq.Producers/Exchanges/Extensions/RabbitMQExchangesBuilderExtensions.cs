using System;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangesBuilderExtensions
	{
		/// <summary>
		///   Sets exchange type to direct
		/// </summary>
		public static IRabbitMQExchangeBuilder AddDirectExchange(
			this IRabbitMQExchangesBuilder configuration,
			string exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.AddExchange(ExchangeType.Direct, exchange);
		}

		/// <summary>
		///   Sets exchange type to fanout
		/// </summary>
		public static IRabbitMQExchangeBuilder AddFanoutExchange(
			this IRabbitMQExchangesBuilder configuration,
			string exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.AddExchange(ExchangeType.Fanout, exchange);
		}

		/// <summary>
		///   Sets exchange type to topic
		/// </summary>
		public static IRabbitMQExchangeBuilder AddTopicExchange(
			this IRabbitMQExchangesBuilder configuration,
			string exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.AddExchange(ExchangeType.Topic, exchange);
		}

		/// <summary>
		///   Sets exchange type to headers
		/// </summary>
		public static IRabbitMQExchangeBuilder AddHeadersExchange(
			this IRabbitMQExchangesBuilder configuration,
			string exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.AddExchange(ExchangeType.Headers, exchange);
		}
	}
}