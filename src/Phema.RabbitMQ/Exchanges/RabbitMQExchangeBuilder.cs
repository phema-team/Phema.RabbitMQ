namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBuilderCore
	{
		RabbitMQExchangeDeclaration Declaration { get; }
	}

	public interface IRabbitMQExchangeBuilder : IRabbitMQExchangeBuilderCore
	{
	}

	internal sealed class RabbitMQExchangeBuilder : IRabbitMQExchangeBuilder
	{
		public RabbitMQExchangeBuilder(RabbitMQExchangeDeclaration declaration)
		{
			Declaration = declaration;
		}

		public RabbitMQExchangeDeclaration Declaration { get; }
	}

	public interface IRabbitMQExchangeBuilder<in TPayload> : IRabbitMQExchangeBuilderCore
	{
	}

	internal sealed class RabbitMQExchangeBuilder<TPayload> : IRabbitMQExchangeBuilder<TPayload>
	{
		public RabbitMQExchangeBuilder(RabbitMQExchangeDeclaration declaration)
		{
			Declaration = declaration;
		}

		public RabbitMQExchangeDeclaration Declaration { get; }
	}
}