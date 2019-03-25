namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducerBuilder
	{
		IRabbitMQProducerDeclaration Declaration { get; }
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