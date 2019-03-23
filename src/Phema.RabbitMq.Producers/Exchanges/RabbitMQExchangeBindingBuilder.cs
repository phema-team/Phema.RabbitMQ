namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBindingBuilder
		: IRabbitMQDeclarationBuilder<IRabbitMQExchangeBindingDeclaration>
	{
	}

	internal sealed class RabbitMQExchangeBindingBuilder : IRabbitMQExchangeBindingBuilder
	{
		public RabbitMQExchangeBindingBuilder(IRabbitMQExchangeBindingDeclaration declaration)
		{
			Declaration = declaration;
		}

		public IRabbitMQExchangeBindingDeclaration Declaration { get; }
	}
}