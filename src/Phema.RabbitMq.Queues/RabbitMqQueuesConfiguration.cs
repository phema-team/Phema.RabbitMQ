using System;

using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueuesConfiguration
	{
		IRabbitMQQueueConfiguration AddQueue(string queueName);
	}

	internal sealed class RabbitMQQueuesConfiguration : IRabbitMQQueuesConfiguration
	{
		private readonly IServiceCollection services;

		public RabbitMQQueuesConfiguration(IServiceCollection services)
		{
			this.services = services;
		}

		public IRabbitMQQueueConfiguration AddQueue(string queueName)
		{
			if (queueName is null)
				throw new ArgumentNullException(nameof(queueName));
			
			var queue = new RabbitMQQueue(queueName);

			services.Configure<RabbitMQQueuesOptions>(options =>
				options.Queues.Add(queue));

			return new RabbitMQQueueConfiguration(queue);
		}
	}
}