using System;
using System.Collections.Generic;

namespace Phema.RabbitMq
{
	internal sealed class RabbitMqConsumersOptions
	{
		public RabbitMqConsumersOptions()
		{
			Consumers = new List<RabbitMqConsumer>();
			ConsumerDispatchers = new List<Action<IServiceProvider>>();
		}
		
		public IList<RabbitMqConsumer> Consumers { get; }
		public IList<Action<IServiceProvider>> ConsumerDispatchers { get; }
	}
}