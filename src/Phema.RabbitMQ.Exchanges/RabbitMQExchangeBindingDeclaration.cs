using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBindingDeclaration
	{
		string ExchangeName { get; }
		IList<string> RoutingKeys { get; }
		bool NoWait { get; set; }
		bool Deleted { get; set; }
		IDictionary<string, object> Arguments { get; }
	}
}

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQExchangeBindingDeclaration : IRabbitMQExchangeBindingDeclaration
	{
		public RabbitMQExchangeBindingDeclaration(string exchangeName)
		{
			ExchangeName = exchangeName;
			RoutingKeys = new List<string>();
			Arguments = new Dictionary<string, object>();
		}

		public string ExchangeName { get; }
		public IList<string> RoutingKeys { get; }
		public bool NoWait { get; set; }
		public bool Deleted { get; set; }
		public IDictionary<string, object> Arguments { get; }
	}
}