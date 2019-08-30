using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static partial class RabbitMQConnectionBuilderExtensions
	{
		public static IRabbitMQExchangeBuilder<object> AddExchange(
			this IRabbitMQConnectionBuilder connection,
			string exchangeType,
			string exchangeName)
		{
			return connection.AddExchange<object>(exchangeType, exchangeName);
		}

		public static IRabbitMQExchangeBuilder<TPayload> AddExchange<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			string exchangeType,
			string exchangeName)
		{
			var declaration = new RabbitMQExchangeDeclaration(connection.Declaration, exchangeType, exchangeName);

			connection.Services
				.Configure<RabbitMQOptions>(options => options.ExchangeDeclarations.Add(declaration));

			return new RabbitMQExchangeBuilder<TPayload>(declaration);
		}

		public static IRabbitMQExchangeBuilder<object> AddDirectExchange(
			this IRabbitMQConnectionBuilder connection,
			string exchangeName)
		{
			return connection.AddExchange(ExchangeType.Direct, exchangeName);
		}

		public static IRabbitMQExchangeBuilder<TPayload> AddDirectExchange<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			string exchangeName)
		{
			return connection.AddExchange<TPayload>(ExchangeType.Direct, exchangeName);
		}

		public static IRabbitMQExchangeBuilder<TPayload> AddFanoutExchange<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			string exchangeName)
		{
			return connection.AddExchange<TPayload>(ExchangeType.Fanout, exchangeName);
		}

		public static IRabbitMQExchangeBuilder<object> AddTopicExchange(
			this IRabbitMQConnectionBuilder connection,
			string exchangeName)
		{
			return connection.AddExchange(ExchangeType.Topic, exchangeName);
		}

		public static IRabbitMQExchangeBuilder<TPayload> AddTopicExchange<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			string exchangeName)
		{
			return connection.AddExchange<TPayload>(ExchangeType.Topic, exchangeName);
		}

		public static IRabbitMQExchangeBuilder<object> AddHeadersExchange(
			this IRabbitMQConnectionBuilder connection,
			string exchangeName)
		{
			return connection.AddExchange(ExchangeType.Headers, exchangeName);
		}

		public static IRabbitMQExchangeBuilder<TPayload> AddHeadersExchange<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			string exchangeName)
		{
			return connection.AddExchange<TPayload>(ExchangeType.Headers, exchangeName);
		}
	}
}