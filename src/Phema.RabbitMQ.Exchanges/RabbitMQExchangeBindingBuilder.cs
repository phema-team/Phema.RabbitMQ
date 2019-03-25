namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBindingBuilder
	{
		IRabbitMQExchangeBindingDeclaration Declaration { get; }
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