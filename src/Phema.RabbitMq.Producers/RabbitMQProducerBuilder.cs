namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducerBuilder
	{
		IRabbitMQProducerDeclaration Declaration { get; }
	}
}

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQProducerBuilder : IRabbitMQProducerBuilder
	{
		public RabbitMQProducerBuilder(IRabbitMQProducerDeclaration declaration)
		{
			Declaration = declaration;
		}

		public IRabbitMQProducerDeclaration Declaration { get; }
	}
}