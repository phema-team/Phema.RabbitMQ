namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueueProvider
	{
		IRabbitMQQueueMetadata Metadata { get; }
	}

	internal sealed class RabbitMQQueueProvider : IRabbitMQQueueProvider
	{
		public RabbitMQQueueProvider(IRabbitMQQueueMetadata metadata)
		{
			Metadata = metadata;
		}
		
		public IRabbitMQQueueMetadata Metadata { get; }
	}
}