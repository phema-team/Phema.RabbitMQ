using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQExchangesOptions
	{
		public RabbitMQExchangesOptions()
		{
			Exchanges = new List<RabbitMQExchangeMetadata>();
		}

		public IList<RabbitMQExchangeMetadata> Exchanges { get; }
	}
}