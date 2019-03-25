using System.Collections.Generic;

namespace Phema.RabbitMQ
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