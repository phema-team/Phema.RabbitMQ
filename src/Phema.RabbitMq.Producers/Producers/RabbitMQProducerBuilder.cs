namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducerBuilder
		: IRabbitMQMetadataBuilder<IRabbitMQProducerMetadata>
	{
	}

	internal sealed class RabbitMQProducerBuilder : IRabbitMQProducerBuilder
	{
		public RabbitMQProducerBuilder(IRabbitMQProducerMetadata metadata)
		{
			Metadata = metadata;
		}

		public IRabbitMQProducerMetadata Metadata { get; }
	}
}