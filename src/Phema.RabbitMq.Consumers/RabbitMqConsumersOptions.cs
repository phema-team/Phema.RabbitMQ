using System;
using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQConsumersOptions
	{
		public RabbitMQConsumersOptions()
		{
			ConsumerDispatchers = new List<Action<IServiceProvider>>();
		}

		public IList<Action<IServiceProvider>> ConsumerDispatchers { get; }
	}
}