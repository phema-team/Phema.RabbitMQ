namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBindingBuilder
		: IRabbitMQMetadataBuilder<IRabbitMQExchangeBindingMetadata>
	{
	}

	internal sealed class RabbitMQExchangeBindingBuilder : IRabbitMQExchangeBindingBuilder
	{
		public RabbitMQExchangeBindingBuilder(IRabbitMQExchangeBindingMetadata metadata)
		{
			Metadata = metadata;
		}

		public IRabbitMQExchangeBindingMetadata Metadata { get; }
	}
}