using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQExchangeBinding
	{
		public RabbitMQExchangeBinding(string exchangeName)
		{
			ExchangeName = exchangeName;
			Arguments = new Dictionary<string, object>();
		}

		public string ExchangeName { get; }
		public string RoutingKey { get; set; }
		public IDictionary<string, object> Arguments { get; }
	}
}