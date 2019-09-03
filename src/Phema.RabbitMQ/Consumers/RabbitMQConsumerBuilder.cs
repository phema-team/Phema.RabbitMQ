namespace Phema.RabbitMQ
{
	public interface IRabbitMQConsumerBuilder<TPayload>
	{
		RabbitMQConsumerDeclaration ConsumerDeclaration { get; }
	}

	internal sealed class RabbitMQConsumerBuilder<TPayload> : IRabbitMQConsumerBuilder<TPayload>
	{
		public RabbitMQConsumerBuilder(RabbitMQConsumerDeclaration consumerDeclaration)
		{
			ConsumerDeclaration = consumerDeclaration;
		}

		public RabbitMQConsumerDeclaration ConsumerDeclaration { get; }
	}
}