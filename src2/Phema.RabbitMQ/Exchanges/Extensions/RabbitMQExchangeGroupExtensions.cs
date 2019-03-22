using System;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangeGroupExtensions
	{
		/// <summary>
		///   Sets exchange type to direct
		/// </summary>
		public static IRabbitMQExchangeGroupProvider DirectExchange(
			this IRabbitMQExchangeGroupProvider configuration,
			string exchangeName,
			Action<IRabbitMQExchangeProvider> exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.Exchange(ExchangeType.Direct, exchangeName, exchange);
		}

		/// <summary>
		///   Sets exchange type to fanout
		/// </summary>
		public static IRabbitMQExchangeGroupProvider FanoutExchange(
			this IRabbitMQExchangeGroupProvider configuration,
			string exchangeName,
			Action<IRabbitMQExchangeProvider> exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.Exchange(ExchangeType.Fanout, exchangeName, exchange);
		}

		/// <summary>
		///   Sets exchange type to topic
		/// </summary>
		public static IRabbitMQExchangeGroupProvider TopicExchange(
			this IRabbitMQExchangeGroupProvider configuration,
			string exchangeName,
			Action<IRabbitMQExchangeProvider> exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.Exchange(ExchangeType.Topic, exchangeName, exchange);
		}

		/// <summary>
		///   Sets exchange type to headers
		/// </summary>
		public static IRabbitMQExchangeGroupProvider HeadersExchange(
			this IRabbitMQExchangeGroupProvider configuration,
			string exchangeName,
			Action<IRabbitMQExchangeProvider> exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.Exchange(ExchangeType.Headers, exchangeName, exchange);
		}
	}
}