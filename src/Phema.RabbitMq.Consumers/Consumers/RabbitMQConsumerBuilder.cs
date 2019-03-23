namespace Phema.RabbitMQ
{
	public interface IRabbitMQConsumerBuilder
		: IRabbitMQDeclarationBuilder<IRabbitMQConsumerDeclaration>
	{
	}

	internal sealed class RabbitMQConsumerBuilder : IRabbitMQConsumerBuilder
	{
		public RabbitMQConsumerBuilder(IRabbitMQConsumerDeclaration declaration)
		{
			Declaration = declaration;
		}

		public IRabbitMQConsumerDeclaration Declaration { get; }
	}
}