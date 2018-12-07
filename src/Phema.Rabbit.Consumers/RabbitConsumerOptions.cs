using System;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	internal class RabbitConsumerOptions
	{
		public RabbitConsumerOptions()
		{
			ConsumerActions = new List<Action<IServiceProvider, IConnection>>();
		}
		
		internal IList<Action<IServiceProvider, IConnection>> ConsumerActions { get; }
	}
}