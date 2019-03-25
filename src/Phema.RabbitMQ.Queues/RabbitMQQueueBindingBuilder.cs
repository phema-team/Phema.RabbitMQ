namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueueBindingBuilder
	{
		IRabbitMQQueueBindingDeclaration Declaration { get; }
	}

	internal sealed class RabbitMQQueueBindingBuilder : IRabbitMQQueueBindingBuilder
	{
		public RabbitMQQueueBindingBuilder(IRabbitMQQueueBindingDeclaration declaration)
		{
			Declaration = declaration;
		}

		public IRabbitMQQueueBindingDeclaration Declaration { get; }
	}
}