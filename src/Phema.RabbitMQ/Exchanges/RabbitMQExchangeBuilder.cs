namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBuilderCore
	{
		RabbitMQExchangeDeclaration ExchangeDeclaration { get; }
	}

	public interface IRabbitMQExchangeBuilder : IRabbitMQExchangeBuilderCore
	{
	}

	internal sealed class RabbitMQExchangeBuilder : IRabbitMQExchangeBuilder
	{
		public RabbitMQExchangeBuilder(RabbitMQExchangeDeclaration exchangeDeclaration)
		{
			ExchangeDeclaration = exchangeDeclaration;
		}

		public RabbitMQExchangeDeclaration ExchangeDeclaration { get; }
	}

	public interface IRabbitMQExchangeBuilder<in TPayload> : IRabbitMQExchangeBuilderCore
	{
	}

	internal sealed class RabbitMQExchangeBuilder<TPayload> : IRabbitMQExchangeBuilder<TPayload>
	{
		public RabbitMQExchangeBuilder(RabbitMQExchangeDeclaration exchangeDeclaration)
		{
			ExchangeDeclaration = exchangeDeclaration;
		}

		public RabbitMQExchangeDeclaration ExchangeDeclaration { get; }
	}
}