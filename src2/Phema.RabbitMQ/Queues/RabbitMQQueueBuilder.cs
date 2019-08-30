namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueueBuilder<TPayload>
	{
		RabbitMQQueueDeclaration Declaration { get; }
	}
	
	internal sealed class RabbitMQQueueBuilder<TPayload> : IRabbitMQQueueBuilder<TPayload>
	{
		public RabbitMQQueueBuilder(RabbitMQQueueDeclaration declaration)
		{
			Declaration = declaration;
		}

		public RabbitMQQueueDeclaration Declaration { get; }
	}
}