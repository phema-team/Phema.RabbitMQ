using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueuesBuilder
	{
		/// <summary>
		/// Register new queue
		/// </summary>
		IRabbitMQQueueBuilder AddQueue(string queueName);
	}

	internal sealed class RabbitMQQueuesBuilder : IRabbitMQQueuesBuilder
	{
		private readonly IServiceCollection services;

		public RabbitMQQueuesBuilder(IServiceCollection services)
		{
			this.services = services;
		}

		public IRabbitMQQueueBuilder AddQueue(string queueName)
		{
			if (queueName is null)
				throw new ArgumentNullException(nameof(queueName));

			var metadata = new RabbitMQQueueMetadata(queueName);

			services.Configure<RabbitMQQueuesOptions>(options =>
			{
				if (options.Queues.Any(q => q.Name == queueName))
					throw new ArgumentException($"Queue {queueName} already registered", nameof(queueName));

				options.Queues.Add(metadata);
			});

			return new RabbitMQQueueBuilder(metadata);
		}
	}
}