namespace Phema.RabbitMQ
{
	public interface IRabbitMQConsumerBuilder
	{
		IRabbitMQConsumerDeclaration Declaration { get; }
	}

	internal sealed class RabbitMQConsumerBuilder : IRabbitMQConsumerBuilder
	{
		public RabbitMQConsumerBuilder(IRabbitMQConsumerDeclaration declaration)
		{
			Declaration = declaration;
		}

		public IRabbitMQConsumerDeclaration Declaration { get; }
	}
}