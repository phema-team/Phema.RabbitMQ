using System;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangeGroupExtensions
	{
		/// <summary>
		///   Sets exchange type to direct
		/// </summary>
		public static IRabbitMQProvider DirectExchange(
			this IRabbitMQProvider configuration,
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
		public static IRabbitMQProvider FanoutExchange(
			this IRabbitMQProvider configuration,
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
		public static IRabbitMQProvider TopicExchange(
			this IRabbitMQProvider configuration,
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
		public static IRabbitMQProvider HeadersExchange(
			this IRabbitMQProvider configuration,
			string exchangeName,
			Action<IRabbitMQExchangeProvider> exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.Exchange(ExchangeType.Headers, exchangeName, exchange);
		}
	}
}