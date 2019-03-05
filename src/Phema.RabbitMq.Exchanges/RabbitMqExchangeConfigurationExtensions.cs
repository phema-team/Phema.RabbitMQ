using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangeConfigurationExtensions
	{
		public static IRabbitMQExchangeConfiguration AddDirectExchange(
			this IRabbitMQExchangesConfiguration configuration,
			string exchangeName)
		{
			return configuration.AddExchange(ExchangeType.Direct, exchangeName);
		}

		public static IRabbitMQExchangeConfiguration AddFanoutExchange(
			this IRabbitMQExchangesConfiguration configuration,
			string exchangeName)
		{
			return configuration.AddExchange(ExchangeType.Fanout, exchangeName);
		}

		public static IRabbitMQExchangeConfiguration AddTopicExchange(
			this IRabbitMQExchangesConfiguration configuration,
			string exchangeName)
		{
			return configuration.AddExchange(ExchangeType.Topic, exchangeName);
		}

		public static IRabbitMQExchangeConfiguration AddHeadersExchange(
			this IRabbitMQExchangesConfiguration configuration,
			string exchangeName)
		{
			return configuration.AddExchange(ExchangeType.Headers, exchangeName);
		}
	}
}