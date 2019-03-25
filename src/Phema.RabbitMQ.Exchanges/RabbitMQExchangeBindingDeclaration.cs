using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBindingDeclaration
	{
		string ExchangeName { get; }
		string RoutingKey { get; set; }
		bool NoWait { get; set; }
		bool Deleted { get; set; }
		IDictionary<string, object> Arguments { get; }
	}

	internal sealed class RabbitMQExchangeBindingDeclaration : IRabbitMQExchangeBindingDeclaration
	{
		public RabbitMQExchangeBindingDeclaration(string exchangeName)
		{
			ExchangeName = exchangeName;
			Arguments = new Dictionary<string, object>();
		}

		public string ExchangeName { get; }
		public string RoutingKey { get; set; }
		public bool NoWait { get; set; }
		public bool Deleted { get; set; }
		public IDictionary<string, object> Arguments { get; }
	}
}