using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQProducersOptions
	{
		public RabbitMQProducersOptions()
		{
			Declarations = new List<IRabbitMQProducerDeclaration>();
		}

		public IList<IRabbitMQProducerDeclaration> Declarations { get; }
	}
}