using System;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangeGroupBuilderExtensions
	{
		/// <summary>
		///   Declare direct exchange
		/// </summary>
		public static IRabbitMQExchangeBuilder AddDirectExchange(
			this IRabbitMQExchangeGroupBuilder configuration,
			string exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.AddExchange(ExchangeType.Direct, exchange);
		}

		/// <summary>
		///   Declare fanout exchange
		/// </summary>
		public static IRabbitMQExchangeBuilder AddFanoutExchange(
			this IRabbitMQExchangeGroupBuilder configuration,
			string exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.AddExchange(ExchangeType.Fanout, exchange);
		}

		/// <summary>
		///   Declare topic exchange
		/// </summary>
		public static IRabbitMQExchangeBuilder AddTopicExchange(
			this IRabbitMQExchangeGroupBuilder configuration,
			string exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.AddExchange(ExchangeType.Topic, exchange);
		}

		/// <summary>
		///   Declare headers exchange
		/// </summary>
		public static IRabbitMQExchangeBuilder AddHeadersExchange(
			this IRabbitMQExchangeGroupBuilder configuration,
			string exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.AddExchange(ExchangeType.Headers, exchange);
		}
	}
}