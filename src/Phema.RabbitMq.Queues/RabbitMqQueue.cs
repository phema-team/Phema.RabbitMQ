using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQQueue
	{
		public RabbitMQQueue(string name)
		{
			Name = name;
			Arguments = new Dictionary<string, object>();
		}

		public string Name { get; }
		public bool Durable { get; set; }
		public bool Exclusive { get; set; }
		public bool AutoDelete { get; set; }
		public IDictionary<string, object> Arguments { get; }
	}
}