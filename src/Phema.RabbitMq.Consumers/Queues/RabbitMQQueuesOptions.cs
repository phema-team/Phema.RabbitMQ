using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQQueuesOptions
	{
		public RabbitMQQueuesOptions()
		{
			Queues = new List<IRabbitMQQueueDeclaration>();
		}

		public IList<IRabbitMQQueueDeclaration> Queues { get; }
	}
}