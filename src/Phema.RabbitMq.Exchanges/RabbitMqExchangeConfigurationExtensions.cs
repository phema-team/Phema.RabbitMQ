using RabbitMQ.Client;

namespace Phema.RabbitMq
{
	public static class RabbitMqExchangeConfigurationExtensions
	{
		public static IRabbitMqExchangeConfiguration AddDirectExchange(
			this IRabbitMqExchangesConfiguration configuration,
			string exchangeName)
		{
			return configuration.AddExchange(ExchangeType.Direct, exchangeName);
		}

		public static IRabbitMqExchangeConfiguration AddFanoutExchange(
			this IRabbitMqExchangesConfiguration configuration,
			string exchangeName)
		{
			return configuration.AddExchange(ExchangeType.Fanout, exchangeName);
		}

		public static IRabbitMqExchangeConfiguration AddTopicExchange(
			this IRabbitMqExchangesConfiguration configuration,
			string exchangeName)
		{
			return configuration.AddExchange(ExchangeType.Topic, exchangeName);
		}

		public static IRabbitMqExchangeConfiguration AddHeadersExchange(
			this IRabbitMqExchangesConfiguration configuration,
			string exchangeName)
		{
			return configuration.AddExchange(ExchangeType.Headers, exchangeName);
		}
	}
}