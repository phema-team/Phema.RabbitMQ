using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	public sealed class RabbitMQQueueDeclaration
	{
		public RabbitMQQueueDeclaration(
			RabbitMQConnectionDeclaration connection,
			string name)
		{
			Connection = connection;
			Name = name;
			Arguments = new Dictionary<string, object>();
			Bindings = new List<RabbitMQQueueBindingDeclaration>();
		}

		public RabbitMQConnectionDeclaration Connection { get; }
		public string Name { get; }
		public bool Durable { get; set; }
		public bool Exclusive { get; set; }
		public bool AutoDelete { get; set; }
		public bool NoWait { get; set; }
		public bool Deleted { get; set; }
		public bool IfUnused { get; set; }
		public bool IfEmpty { get; set; }
		public IDictionary<string, object> Arguments { get; }
		public IList<RabbitMQQueueBindingDeclaration> Bindings { get; }
	}
}