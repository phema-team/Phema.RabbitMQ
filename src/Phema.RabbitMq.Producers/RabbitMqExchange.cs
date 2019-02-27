using System.Collections.Generic;

namespace Phema.RabbitMq
{
	internal sealed class RabbitMqExchange
	{
		public RabbitMqExchange(string type, string name)
		{
			Type = type;
			Name = name;
			Arguments = new Dictionary<string, object>();
		}
		
		public string Type { get; }
		public string Name { get; }
		public bool Durable { get; set; }
		public bool AutoDelete { get; set; }
		public IDictionary<string, object> Arguments { get; }
	}
}