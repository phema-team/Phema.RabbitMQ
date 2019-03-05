using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQQueuesOptions
	{
		public RabbitMQQueuesOptions()
		{
			Queues = new List<RabbitMQQueue>();
		}

		public IList<RabbitMQQueue> Queues { get; }
	}
}