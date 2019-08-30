namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducerBuilder<TPayload>
	{
		RabbitMQProducerDeclaration Declaration { get; }
	}

	internal sealed class RabbitMQProducerBuilder<TPayload> : IRabbitMQProducerBuilder<TPayload>
	{
		public RabbitMQProducerBuilder(RabbitMQProducerDeclaration declaration)
		{
			Declaration = declaration;
		}

		public RabbitMQProducerDeclaration Declaration { get; }
	}
}