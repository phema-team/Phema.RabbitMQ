namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeProvider
	{
		IRabbitMQExchangeMetadata Metadata { get; }
	}

	internal sealed class RabbitMQExchangeProvider : IRabbitMQExchangeProvider
	{
		public RabbitMQExchangeProvider(IRabbitMQExchangeMetadata metadata)
		{
			Metadata = metadata;
		}

		public IRabbitMQExchangeMetadata Metadata { get; }
	}
}