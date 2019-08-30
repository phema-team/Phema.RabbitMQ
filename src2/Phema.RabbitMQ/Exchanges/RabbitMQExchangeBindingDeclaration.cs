using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	public sealed class RabbitMQExchangeBindingDeclaration
	{
		public RabbitMQExchangeBindingDeclaration(RabbitMQExchangeDeclaration exchange)
		{
			Exchange = exchange;
			RoutingKeys = new List<string>();
			Arguments = new Dictionary<string, object>();
		}

		public RabbitMQExchangeDeclaration Exchange { get; }
		public IList<string> RoutingKeys { get; }
		public bool NoWait { get; set; }
		public bool Deleted { get; set; }
		public IDictionary<string, object> Arguments { get; }
	}
}