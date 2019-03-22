namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBindingProvider
	{
		IRabbitMQExchangeBindingMetadata Metadata { get; }
	}

	internal sealed class RabbitMQExchangeBindingProvider : IRabbitMQExchangeBindingProvider
	{
		public RabbitMQExchangeBindingProvider(IRabbitMQExchangeBindingMetadata metadata)
		{
			Metadata = metadata;
		}

		public IRabbitMQExchangeBindingMetadata Metadata { get; }
	}
}