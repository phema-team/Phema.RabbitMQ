using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueueMetadata
	{
		string Name { get; }
		bool Durable { get; set; }
		bool Exclusive { get; set; }
		bool AutoDelete { get; set; }
		bool NoWait { get; set; }
		
		bool Purged { get; set; }
		
		bool Deleted { get; set; }
		bool IfUnused { get; set; }
		bool IfEmpty { get; set; }
		
		IDictionary<string, object> Arguments { get; }
	}

	internal sealed class RabbitMQQueueMetadata : IRabbitMQQueueMetadata
	{
		public RabbitMQQueueMetadata(string queueName)
		{
			Name = queueName;
			Arguments = new Dictionary<string, object>();
		}

		public string Name { get; }
		public bool Durable { get; set; }
		public bool Exclusive { get; set; }
		public bool AutoDelete { get; set; }
		public bool NoWait { get; set; }
		
		public bool Purged { get; set; }
		
		public bool Deleted { get; set; }
		public bool IfUnused { get; set; }
		public bool IfEmpty { get; set; }
		
		public IDictionary<string, object> Arguments { get; }
	}
}