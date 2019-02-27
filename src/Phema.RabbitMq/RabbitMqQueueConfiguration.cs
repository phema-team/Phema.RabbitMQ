using System.Collections.Generic;

namespace Phema.RabbitMq
{
	public interface IRabbitMqQueueConfiguration
	{
		IRabbitMqQueueConfiguration Durable();
		IRabbitMqQueueConfiguration Exclusive();
		IRabbitMqQueueConfiguration AutoDelete();
		IRabbitMqQueueConfiguration WithArgument(string argument, string value);
	}
	
	internal sealed class RabbitMqQueueConfiguration : IRabbitMqQueueConfiguration
	{
		private readonly RabbitMqQueue queue;

		public RabbitMqQueueConfiguration(RabbitMqQueue queue)
		{
			this.queue = queue;
		}

		public IRabbitMqQueueConfiguration Durable()
		{
			queue.Durable = true;
			return this;
		}

		public IRabbitMqQueueConfiguration Exclusive()
		{
			queue.Exclusive = true;
			return this;
		}

		public IRabbitMqQueueConfiguration AutoDelete()
		{
			queue.AutoDelete = true;
			return this;
		}

		public IRabbitMqQueueConfiguration WithArgument(string argument, string value)
		{
			queue.Arguments.Add(argument, value);
			return this;
		}
	}
}