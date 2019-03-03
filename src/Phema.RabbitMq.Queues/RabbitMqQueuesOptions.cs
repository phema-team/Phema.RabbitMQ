using System.Collections.Generic;

namespace Phema.RabbitMq
{
	internal sealed class RabbitMqQueuesOptions
	{
		public RabbitMqQueuesOptions()
		{
			Queues = new List<RabbitMqQueue>();
		}

		public IList<RabbitMqQueue> Queues { get; }
	}
}