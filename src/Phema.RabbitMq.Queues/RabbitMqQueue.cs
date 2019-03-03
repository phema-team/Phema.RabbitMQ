using System.Collections.Generic;

namespace Phema.RabbitMq
{
	internal sealed class RabbitMqQueue
	{
		public RabbitMqQueue(string name)
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