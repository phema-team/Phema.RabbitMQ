namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBuilder
		: IRabbitMQMetadataBuilder<IRabbitMQExchangeMetadata>
	{
	}

	internal sealed class RabbitMQExchangeBuilder : IRabbitMQExchangeBuilder
	{
		public RabbitMQExchangeBuilder(IRabbitMQExchangeMetadata metadata)
		{
			Metadata = metadata;
		}

		public IRabbitMQExchangeMetadata Metadata { get; }
	}
}