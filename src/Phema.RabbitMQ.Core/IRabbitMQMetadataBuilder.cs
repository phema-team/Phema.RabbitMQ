namespace Phema.RabbitMQ
{
	public interface IRabbitMQMetadataBuilder<TMetadata>
	{
		TMetadata Metadata { get; }
	}
}