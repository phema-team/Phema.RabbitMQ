namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducerProvider
	{
		IRabbitMQProducerMetadata Metadata { get; }
	}

	internal sealed class RabbitMQProducerProvider : IRabbitMQProducerProvider
	{
		public RabbitMQProducerProvider(IRabbitMQProducerMetadata metadata)
		{
			Metadata = metadata;
		}

		public IRabbitMQProducerMetadata Metadata { get; }
	}
}