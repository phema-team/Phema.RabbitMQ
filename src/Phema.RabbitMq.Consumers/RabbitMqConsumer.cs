using System;
using System.Collections.Generic;

namespace Phema.RabbitMq
{
	internal class RabbitMqConsumer
	{
		protected RabbitMqConsumer(string queueName)
		{
			QueueName = queueName;
			Consumers = 1;
			Arguments = new Dictionary<string, object>();
		}

		public string QueueName { get; }
		public string Tag { get; set; }
		public ushort Prefetch { get; set; }
		public int Consumers { get; set; }
		public bool Exclusive { get; set; }
		public bool NoLocal { get; set; }
		public bool AutoAck { get; set; }
		public bool Requeue { get; set; }
		public bool Multiple { get; set; }
		public IDictionary<string, object> Arguments { get; }
	}
	
	internal sealed class RabbitMqConsumer<TPayload, TPayloadConsumer> : RabbitMqConsumer
	{
		public RabbitMqConsumer(string queueName) : base(queueName)
		{
		}
	}
}