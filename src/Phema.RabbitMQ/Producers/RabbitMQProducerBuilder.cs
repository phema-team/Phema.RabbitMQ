namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducerBuilder<TPayload>
	{
		RabbitMQProducerDeclaration ProducerDeclaration { get; }
	}

	internal sealed class RabbitMQProducerBuilder<TPayload> : IRabbitMQProducerBuilder<TPayload>
	{
		public RabbitMQProducerBuilder(RabbitMQProducerDeclaration producerDeclaration)
		{
			ProducerDeclaration = producerDeclaration;
		}

		public RabbitMQProducerDeclaration ProducerDeclaration { get; }
	}
}