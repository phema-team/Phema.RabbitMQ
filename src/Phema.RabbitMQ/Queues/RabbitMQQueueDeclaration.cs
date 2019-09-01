using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	public sealed class RabbitMQQueueDeclaration
	{
		public RabbitMQQueueDeclaration(
			RabbitMQConnectionDeclaration connectionDeclaration,
			string name)
		{
			ConnectionDeclaration = connectionDeclaration;
			Name = name;
			Arguments = new Dictionary<string, object>();
			BindingDeclarations = new List<RabbitMQQueueBindingDeclaration>();
		}

		public RabbitMQConnectionDeclaration ConnectionDeclaration { get; }
		public string Name { get; }
		public bool Durable { get; set; }
		public bool Exclusive { get; set; }
		public bool AutoDelete { get; set; }
		public bool NoWait { get; set; }
		public bool Deleted { get; set; }
		public bool UnusedOnly { get; set; }
		public bool EmptyOnly { get; set; }
		public IDictionary<string, object> Arguments { get; }
		public IList<RabbitMQQueueBindingDeclaration> BindingDeclarations { get; }
	}
}