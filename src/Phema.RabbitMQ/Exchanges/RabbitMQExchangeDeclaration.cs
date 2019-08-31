using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	public sealed class RabbitMQExchangeDeclaration
	{
		public RabbitMQExchangeDeclaration(
			RabbitMQConnectionDeclaration connection,
			string type,
			string name)
		{
			Connection = connection;
			Type = type;
			Name = name;
			Arguments = new Dictionary<string, object>();
			Bindings = new List<RabbitMQExchangeBindingDeclaration>();
		}

		public RabbitMQConnectionDeclaration Connection { get; }
		public string Type { get; }
		public string Name { get; }
		public bool Durable { get; set; }
		public bool Internal { get; set; }
		public bool NoWait { get; set; }
		public bool AutoDelete { get; set; }

		public bool Deleted { get; set; }
		public bool UnusedOnly { get; set; }

		public IDictionary<string, object> Arguments { get; }
		public IList<RabbitMQExchangeBindingDeclaration> Bindings { get; }
	}
}