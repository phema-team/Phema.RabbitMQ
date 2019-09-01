using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static partial class RabbitMQConnectionBuilderExtensions
	{
		public static IRabbitMQExchangeBuilder AddExchange(
			this IRabbitMQConnectionBuilder connection,
			string type,
			string name)
		{
			var declaration = new RabbitMQExchangeDeclaration(connection.Declaration, type, name);

			connection.Services
				.Configure<RabbitMQOptions>(options => options.ExchangeDeclarations.Add(declaration));

			return new RabbitMQExchangeBuilder(declaration);
		}

		public static IRabbitMQExchangeBuilder<TPayload> AddExchange<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			string type,
			string name)
		{
			var declaration = new RabbitMQExchangeDeclaration(connection.Declaration, type, name);

			connection.Services
				.Configure<RabbitMQOptions>(options => options.ExchangeDeclarations.Add(declaration));

			return new RabbitMQExchangeBuilder<TPayload>(declaration);
		}

		public static IRabbitMQExchangeBuilder AddDirectExchange(
			this IRabbitMQConnectionBuilder connection,
			string name)
		{
			return connection.AddExchange(ExchangeType.Direct, name);
		}

		public static IRabbitMQExchangeBuilder<TPayload> AddDirectExchange<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			string name)
		{
			return connection.AddExchange<TPayload>(ExchangeType.Direct, name);
		}

		public static IRabbitMQExchangeBuilder<TPayload> AddFanoutExchange<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			string name)
		{
			return connection.AddExchange<TPayload>(ExchangeType.Fanout, name);
		}

		public static IRabbitMQExchangeBuilder AddTopicExchange(
			this IRabbitMQConnectionBuilder connection,
			string name)
		{
			return connection.AddExchange(ExchangeType.Topic, name);
		}

		public static IRabbitMQExchangeBuilder<TPayload> AddTopicExchange<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			string name)
		{
			return connection.AddExchange<TPayload>(ExchangeType.Topic, name);
		}

		public static IRabbitMQExchangeBuilder AddHeadersExchange(
			this IRabbitMQConnectionBuilder connection,
			string name)
		{
			return connection.AddExchange(ExchangeType.Headers, name);
		}

		public static IRabbitMQExchangeBuilder<TPayload> AddHeadersExchange<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			string name)
		{
			return connection.AddExchange<TPayload>(ExchangeType.Headers, name);
		}
	}
}