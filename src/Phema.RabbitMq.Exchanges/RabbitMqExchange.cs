using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQExchange
	{
		public RabbitMQExchange(string type, string name)
		{
			Type = type;
			Name = name;
			Arguments = new Dictionary<string, object>();
			BoundExchanges = new List<RabbitMQExchangeBinding>();
		}

		public string Type { get; }
		public string Name { get; }
		public bool Durable { get; set; }
		public bool AutoDelete { get; set; }
		public IDictionary<string, object> Arguments { get; }
		public IList<RabbitMQExchangeBinding> BoundExchanges { get; set; }
	}
}