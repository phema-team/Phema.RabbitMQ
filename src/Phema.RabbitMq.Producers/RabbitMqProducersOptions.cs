using System.Collections.Generic;

namespace Phema.RabbitMq
{
	internal sealed class RabbitMqProducersOptions
	{
		public RabbitMqProducersOptions()
		{
			Producers = new List<RabbitMqProducer>();
		}

		public IList<RabbitMqProducer> Producers { get; }
	}
}