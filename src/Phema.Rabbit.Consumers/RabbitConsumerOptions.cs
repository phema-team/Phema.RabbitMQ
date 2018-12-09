using System;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	/// <summary>
	/// Used in <see cref="RabbitConsumersHostedService"/>
	/// </summary>
	internal class RabbitConsumerOptions
	{
		public RabbitConsumerOptions()
		{
			ConsumerActions = new List<Action<IServiceProvider, IConnection>>();
		}
		
		public IList<Action<IServiceProvider, IConnection>> ConsumerActions { get; }
	}
}