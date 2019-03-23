namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBuilder
		: IRabbitMQDeclarationBuilder<IRabbitMQExchangeDeclaration>
	{
	}

	internal sealed class RabbitMQExchangeBuilder : IRabbitMQExchangeBuilder
	{
		public RabbitMQExchangeBuilder(IRabbitMQExchangeDeclaration declaration)
		{
			Declaration = declaration;
		}

		public IRabbitMQExchangeDeclaration Declaration { get; }
	}
}