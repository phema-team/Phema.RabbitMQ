using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQExchangesOptions
	{
		public RabbitMQExchangesOptions()
		{
			Exchanges = new List<RabbitMQExchange>();
		}

		public IList<RabbitMQExchange> Exchanges { get; }
	}
}