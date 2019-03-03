using System;
using System.Collections.Generic;

namespace Phema.RabbitMq
{
	internal sealed class RabbitMqConsumersOptions
	{
		public RabbitMqConsumersOptions()
		{
			ConsumerDispatchers = new List<Action<IServiceProvider>>();
		}
		
		public IList<Action<IServiceProvider>> ConsumerDispatchers { get; }
	}
}