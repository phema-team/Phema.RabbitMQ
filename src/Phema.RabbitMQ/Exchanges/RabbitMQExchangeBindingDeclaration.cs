using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	public sealed class RabbitMQExchangeBindingDeclaration
	{
		public RabbitMQExchangeBindingDeclaration(RabbitMQExchangeDeclaration exchange)
		{
			Exchange = exchange;
			Arguments = new Dictionary<string, object>();
		}

		public RabbitMQExchangeDeclaration Exchange { get; }
		public string RoutingKey { get; set; }
		public bool NoWait { get; set; }
		public bool Deleted { get; set; }
		public IDictionary<string, object> Arguments { get; }
	}
}