using System.Collections.Generic;

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQExchangesOptions
	{
		public RabbitMQExchangesOptions()
		{
			Declarations = new List<IRabbitMQExchangeDeclaration>();
		}

		public IList<IRabbitMQExchangeDeclaration> Declarations { get; }
	}
}