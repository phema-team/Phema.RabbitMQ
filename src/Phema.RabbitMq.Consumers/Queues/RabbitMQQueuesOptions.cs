using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQQueuesOptions
	{
		public RabbitMQQueuesOptions()
		{
			Queues = new List<RabbitMQQueueMetadata>();
		}

		public IList<RabbitMQQueueMetadata> Queues { get; }
	}
}