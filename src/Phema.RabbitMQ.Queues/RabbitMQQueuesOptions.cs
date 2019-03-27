using System.Collections.Generic;

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQQueuesOptions
	{
		public RabbitMQQueuesOptions()
		{
			Declarations = new List<IRabbitMQQueueDeclaration>();
		}

		public IList<IRabbitMQQueueDeclaration> Declarations { get; }
	}
}