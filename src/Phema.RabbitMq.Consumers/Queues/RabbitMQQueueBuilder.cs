namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueueBuilder
		: IRabbitMQDurableBuilder<IRabbitMQQueueBuilder>,
			IRabbitMQAutoDeleteBuilder<IRabbitMQQueueBuilder>,
			IRabbitMQWithArgumentBuilder<IRabbitMQQueueBuilder>,
			IRabbitMQExclusiveBuilder<IRabbitMQQueueBuilder>
	{
	}

	internal sealed class RabbitMQQueueBuilder : IRabbitMQQueueBuilder
	{
		private readonly RabbitMQQueueMetadata queue;

		public RabbitMQQueueBuilder(RabbitMQQueueMetadata queue)
		{
			this.queue = queue;
		}

		public IRabbitMQQueueBuilder Durable()
		{
			queue.Durable = true;
			return this;
		}

		public IRabbitMQQueueBuilder AutoDelete()
		{
			queue.AutoDelete = true;
			return this;
		}

		public IRabbitMQQueueBuilder WithArgument<TValue>(string argument, TValue value)
		{
			queue.Arguments.Add(argument, value);
			return this;
		}

		public IRabbitMQQueueBuilder Exclusive()
		{
			queue.Exclusive = true;
			return this;
		}
	}
}