using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	public sealed class RabbitMQExchangeBindingDeclaration
	{
		public RabbitMQExchangeBindingDeclaration(RabbitMQExchangeDeclaration exchangeDeclaration)
		{
			ExchangeDeclaration = exchangeDeclaration;
			Arguments = new Dictionary<string, object>();
		}

		public RabbitMQExchangeDeclaration ExchangeDeclaration { get; }
		public string RoutingKey { get; set; }
		public bool NoWait { get; set; }
		public bool Deleted { get; set; }
		public IDictionary<string, object> Arguments { get; }
	}
}