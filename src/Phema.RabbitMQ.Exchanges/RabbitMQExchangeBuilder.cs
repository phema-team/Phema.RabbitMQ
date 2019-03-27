namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBuilder
	{
		IRabbitMQExchangeDeclaration Declaration { get; }
	}
}

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQExchangeBuilder : IRabbitMQExchangeBuilder
	{
		public RabbitMQExchangeBuilder(IRabbitMQExchangeDeclaration declaration)
		{
			Declaration = declaration;
		}

		public IRabbitMQExchangeDeclaration Declaration { get; }
	}
}