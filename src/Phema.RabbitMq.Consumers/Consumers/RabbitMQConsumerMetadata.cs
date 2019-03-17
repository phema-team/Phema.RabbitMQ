using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConsumerMetadata
	{
		string QueueName { get; }
		string Tag { get; set; }
		ushort Prefetch { get; set; }
		int Count { get; set; }
		bool Exclusive { get; set; }
		bool NoLocal { get; set; }
		bool AutoAck { get; set; }
		bool Requeue { get; set; }
		bool Multiple { get; set; }
		IDictionary<string, object> Arguments { get; }
	}

	internal sealed class RabbitMQConsumerMetadata : IRabbitMQConsumerMetadata
	{
		public RabbitMQConsumerMetadata(string queueName)
		{
			QueueName = queueName;
			Count = 1;
			Arguments = new Dictionary<string, object>();
		}

		public string QueueName { get; }
		public string Tag { get; set; }
		public ushort Prefetch { get; set; }
		public int Count { get; set; }
		public bool Exclusive { get; set; }
		public bool NoLocal { get; set; }
		public bool AutoAck { get; set; }
		public bool Requeue { get; set; }
		public bool Multiple { get; set; }
		public IDictionary<string, object> Arguments { get; }
	}
}