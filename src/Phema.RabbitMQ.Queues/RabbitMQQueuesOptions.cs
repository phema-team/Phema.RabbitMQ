using System.Collections.Generic;

namespace Phema.RabbitMQ
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