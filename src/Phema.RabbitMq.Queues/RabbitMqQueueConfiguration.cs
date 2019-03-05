using System;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueueConfiguration
	{
		IRabbitMQQueueConfiguration Durable();
		IRabbitMQQueueConfiguration Exclusive();
		IRabbitMQQueueConfiguration AutoDelete();
		IRabbitMQQueueConfiguration WithArgument<TValue>(string argument, TValue value);
	}

	internal sealed class RabbitMQQueueConfiguration : IRabbitMQQueueConfiguration
	{
		private readonly RabbitMQQueue queue;

		public RabbitMQQueueConfiguration(RabbitMQQueue queue)
		{
			this.queue = queue;
		}

		/// <summary>
		/// Sets durable to true. Queue won't be deleted on restart
		/// </summary>
		/// <returns></returns>
		public IRabbitMQQueueConfiguration Durable()
		{
			queue.Durable = true;
			return this;
		}

		/// <summary>
		/// Sets exclusive flag
		/// </summary>
		public IRabbitMQQueueConfiguration Exclusive()
		{
			queue.Exclusive = true;
			return this;
		}

		/// <summary>
		/// Sets auto-delete flag. Queue will be deleted when no more bound exchanges or consumers
		/// </summary>
		public IRabbitMQQueueConfiguration AutoDelete()
		{
			queue.AutoDelete = true;
			return this;
		}

		/// <summary>
		/// Sets rabbitmq arguments
		/// </summary>
		public IRabbitMQQueueConfiguration WithArgument<TValue>(string argument, TValue value)
		{
			if (argument is null)
				throw new ArgumentNullException(nameof(argument));
			
			if (queue.Arguments.ContainsKey(argument))
				throw new ArgumentException($"Argument {argument} already registered", nameof(argument));
			
			queue.Arguments.Add(argument, value);
			return this;
		}
	}
}