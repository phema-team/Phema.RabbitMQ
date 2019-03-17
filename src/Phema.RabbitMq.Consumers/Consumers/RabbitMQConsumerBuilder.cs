namespace Phema.RabbitMQ
{
	public interface IRabbitMQConsumerBuilder
		: IRabbitMQMetadataBuilder<IRabbitMQConsumerMetadata>
	{
	}

	internal sealed class RabbitMQConsumerBuilder : IRabbitMQConsumerBuilder
	{
		public RabbitMQConsumerBuilder(IRabbitMQConsumerMetadata metadata)
		{
			Metadata = metadata;
		}

		public IRabbitMQConsumerMetadata Metadata { get; }
	}
}