using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMq
{
	public interface IRabbitMqQueuesConfiguration
	{
		IRabbitMqQueueConfiguration AddQueue(string queueName);
	}

	internal sealed class RabbitMqQueuesConfiguration : IRabbitMqQueuesConfiguration
	{
		private readonly IServiceCollection services;

		public RabbitMqQueuesConfiguration(IServiceCollection services)
		{
			this.services = services;
		}

		public IRabbitMqQueueConfiguration AddQueue(string queueName)
		{
			var queue = new RabbitMqQueue(queueName);

			services.Configure<RabbitMqQueuesOptions>(options =>
				options.Queues.Add(queue));

			return new RabbitMqQueueConfiguration(queue);
		}
	}
}