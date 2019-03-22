using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBindingMetadata
	{
		string ExchangeName { get; }
		string RoutingKey { get; set; }
		bool NoWait { get; set; }
		IDictionary<string, object> Arguments { get; }
	}

	internal sealed class RabbitMQExchangeBindingMetadata : IRabbitMQExchangeBindingMetadata
	{
		public RabbitMQExchangeBindingMetadata(string exchangeName)
		{
			ExchangeName = exchangeName;
			Arguments = new Dictionary<string, object>();
		}

		public string ExchangeName { get; }
		public string RoutingKey { get; set; }
		public bool NoWait { get; set; }
		public IDictionary<string, object> Arguments { get; }
	}
}