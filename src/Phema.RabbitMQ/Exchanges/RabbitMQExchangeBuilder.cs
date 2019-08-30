namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBuilder<in TPayload>
	{
		RabbitMQExchangeDeclaration Declaration { get; }
	}

	internal class RabbitMQExchangeBuilder<TPayload> : IRabbitMQExchangeBuilder<TPayload>
	{
		public RabbitMQExchangeBuilder(RabbitMQExchangeDeclaration declaration)
		{
			Declaration = declaration;
		}

		public RabbitMQExchangeDeclaration Declaration { get; }
	}
}