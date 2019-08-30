namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBindingBuilder
	{
		RabbitMQExchangeBindingDeclaration Declaration { get; }
	}

	internal sealed class RabbitMQExchangeBindingBuilder : IRabbitMQExchangeBindingBuilder
	{
		public RabbitMQExchangeBindingBuilder(RabbitMQExchangeBindingDeclaration declaration)
		{
			Declaration = declaration;
		}

		public RabbitMQExchangeBindingDeclaration Declaration { get; }
	}
}