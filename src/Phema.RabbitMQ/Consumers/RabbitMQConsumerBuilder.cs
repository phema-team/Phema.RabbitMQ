namespace Phema.RabbitMQ
{
	public interface IRabbitMQConsumerBuilder<TPayload>
	{
		RabbitMQConsumerDeclaration Declaration { get; }
	}

	internal sealed class RabbitMQConsumerBuilder<TPayload> : IRabbitMQConsumerBuilder<TPayload>
	{
		public RabbitMQConsumerBuilder(RabbitMQConsumerDeclaration declaration)
		{
			Declaration = declaration;
		}

		public RabbitMQConsumerDeclaration Declaration { get; }
	}
}