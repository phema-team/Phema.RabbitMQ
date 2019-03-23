namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueueBuilder
		: IRabbitMQDeclarationBuilder<IRabbitMQQueueDeclaration>
	{
	}

	internal sealed class RabbitMQQueueBuilder : IRabbitMQQueueBuilder
	{
		public RabbitMQQueueBuilder(IRabbitMQQueueDeclaration declaration)
		{
			Declaration = declaration;
		}

		public IRabbitMQQueueDeclaration Declaration { get; }
	}
}