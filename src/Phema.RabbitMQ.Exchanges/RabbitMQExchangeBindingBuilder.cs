namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBindingBuilder
	{
		IRabbitMQExchangeBindingDeclaration Declaration { get; }
	}
}

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQExchangeBindingBuilder : IRabbitMQExchangeBindingBuilder
	{
		public RabbitMQExchangeBindingBuilder(IRabbitMQExchangeBindingDeclaration declaration)
		{
			Declaration = declaration;
		}

		public IRabbitMQExchangeBindingDeclaration Declaration { get; }
	}
}