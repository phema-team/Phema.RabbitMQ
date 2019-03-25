namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueueBuilder
	{
		IRabbitMQQueueDeclaration Declaration { get; }
	}

	internal sealed class RabbitMQQueueBuilder : IRabbitMQQueueBuilder
	{
		public RabbitMQQueueBuilder(IRabbitMQQueueDeclaration declaration)
		{
			Declaration = declaration;
		}

		public IRabbitMQQueueDeclaration Declaration { get; }
	}
}