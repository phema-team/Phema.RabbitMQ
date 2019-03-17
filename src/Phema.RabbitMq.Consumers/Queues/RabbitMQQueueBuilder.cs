namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueueBuilder
		: IRabbitMQMetadataBuilder<IRabbitMQQueueMetadata>
	{
	}

	internal sealed class RabbitMQQueueBuilder : IRabbitMQQueueBuilder
	{
		public RabbitMQQueueBuilder(IRabbitMQQueueMetadata metadata)
		{
			Metadata = metadata;
		}

		public IRabbitMQQueueMetadata Metadata { get; }
	}
}