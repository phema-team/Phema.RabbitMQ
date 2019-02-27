using System;
using System.Collections.Generic;

using RabbitMQ.Client;

namespace Phema.RabbitMq
{
	internal sealed class RabbitMqProducer
	{
		public RabbitMqProducer(string exchangeName, string queueName)
		{
			ExchangeName = exchangeName;
			QueueName = queueName;
			Properties = new List<Action<IBasicProperties>>();
		}
		
		public string ExchangeName { get; }
		public string QueueName { get; }
		public bool Mandatory { get; set; }
		public IList<Action<IBasicProperties>> Properties { get; set; }
	}
}