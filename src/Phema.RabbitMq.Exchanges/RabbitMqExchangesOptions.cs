using System.Collections.Generic;

namespace Phema.RabbitMq
{
	internal sealed class RabbitMqExchangesOptions
	{
		public RabbitMqExchangesOptions()
		{
			Exchanges = new List<RabbitMqExchange>();
		}

		public IList<RabbitMqExchange> Exchanges { get; }
	}
}