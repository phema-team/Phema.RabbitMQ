using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	internal class RabbitMQConsumer
	{
		protected RabbitMQConsumer(string queueName)
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

	internal sealed class RabbitMQConsumer<TPayload, TPayloadConsumer> : RabbitMQConsumer
	{
		public RabbitMQConsumer(string queueName) : base(queueName)
		{
		}
	}
}