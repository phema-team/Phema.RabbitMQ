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
		bool Passive { get; set; }
		IDictionary<string, object> Arguments { get; }
	}

	internal sealed class RabbitMQQueueMetadata : IRabbitMQQueueMetadata
	{
		public RabbitMQQueueMetadata(string name)
		{
			Name = name;
			Arguments = new Dictionary<string, object>();
		}

		public string Name { get; }
		public bool Durable { get; set; }
		public bool Exclusive { get; set; }
		public bool AutoDelete { get; set; }
		public bool NoWait { get; set; }
		public bool Passive { get; set; }
		public IDictionary<string, object> Arguments { get; }
	}
}