using System.Collections.Generic;

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQConsumersOptions
	{
		public RabbitMQConsumersOptions()
		{
			Declarations = new List<IRabbitMQConsumerDeclaration>(); 
		}

		public IList<IRabbitMQConsumerDeclaration> Declarations { get; }
	}
}