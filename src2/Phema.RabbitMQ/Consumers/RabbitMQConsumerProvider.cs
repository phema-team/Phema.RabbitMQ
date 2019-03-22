namespace Phema.RabbitMQ
{
	public interface IRabbitMQConsumerProvider
	{
		IRabbitMQConsumerMetadata Metadata { get; }
	}
	
	internal sealed class RabbitMQConsumerProvider : IRabbitMQConsumerProvider
	{
		public RabbitMQConsumerProvider(IRabbitMQConsumerMetadata metadata)
		{
			Metadata = metadata;
		}

		public IRabbitMQConsumerMetadata Metadata { get; }
	}
}