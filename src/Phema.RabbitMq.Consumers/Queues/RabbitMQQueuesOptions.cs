using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQQueuesOptions
	{
		public RabbitMQQueuesOptions()
		{
			Queues = new List<IRabbitMQQueueMetadata>();
		}

		public IList<IRabbitMQQueueMetadata> Queues { get; }
	}
}