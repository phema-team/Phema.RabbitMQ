namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueueBuilder
	{
		IRabbitMQQueueDeclaration Declaration { get; }
	}
}

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQQueueBuilder : IRabbitMQQueueBuilder
	{
		public RabbitMQQueueBuilder(IRabbitMQQueueDeclaration declaration)
		{
			Declaration = declaration;
		}

		public IRabbitMQQueueDeclaration Declaration { get; }
	}
}