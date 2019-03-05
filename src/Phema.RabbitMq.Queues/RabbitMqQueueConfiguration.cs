namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueueConfiguration
	{
		IRabbitMQQueueConfiguration Durable();
		IRabbitMQQueueConfiguration Exclusive();
		IRabbitMQQueueConfiguration AutoDelete();
		IRabbitMQQueueConfiguration WithArgument(string argument, string value);
	}

	internal sealed class RabbitMQQueueConfiguration : IRabbitMQQueueConfiguration
	{
		private readonly RabbitMQQueue queue;

		public RabbitMQQueueConfiguration(RabbitMQQueue queue)
		{
			this.queue = queue;
		}

		public IRabbitMQQueueConfiguration Durable()
		{
			queue.Durable = true;
			return this;
		}

		public IRabbitMQQueueConfiguration Exclusive()
		{
			queue.Exclusive = true;
			return this;
		}

		public IRabbitMQQueueConfiguration AutoDelete()
		{
			queue.AutoDelete = true;
			return this;
		}

		public IRabbitMQQueueConfiguration WithArgument(string argument, string value)
		{
			queue.Arguments.Add(argument, value);
			return this;
		}
	}
}