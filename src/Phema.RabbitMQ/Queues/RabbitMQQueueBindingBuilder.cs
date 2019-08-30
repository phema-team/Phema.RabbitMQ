namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueueBindingBuilder
	{
		RabbitMQQueueBindingDeclaration Declaration { get; }
	}

	internal sealed class RabbitMQQueueBindingBuilder : IRabbitMQQueueBindingBuilder
	{
		public RabbitMQQueueBindingBuilder(RabbitMQQueueBindingDeclaration declaration)
		{
			Declaration = declaration;
		}

		public RabbitMQQueueBindingDeclaration Declaration { get; }
	}
}