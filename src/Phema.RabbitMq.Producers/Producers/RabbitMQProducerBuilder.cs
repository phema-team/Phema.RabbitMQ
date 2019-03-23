namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducerBuilder
		: IRabbitMQDeclarationBuilder<IRabbitMQProducerDeclaration>
	{
	}

	internal sealed class RabbitMQProducerBuilder : IRabbitMQProducerBuilder
	{
		public RabbitMQProducerBuilder(IRabbitMQProducerDeclaration declaration)
		{
			Declaration = declaration;
		}

		public IRabbitMQProducerDeclaration Declaration { get; }
	}
}