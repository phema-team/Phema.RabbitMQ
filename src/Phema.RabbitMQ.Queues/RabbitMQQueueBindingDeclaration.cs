using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueueBindingDeclaration
	{
		string ExchangeName { get; }
		string RoutingKey { get; set; }
		bool NoWait { get; set; }
		bool Deleted { get; set; }
		IDictionary<string, object> Arguments { get; }
	}
}

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQQueueBindingDeclaration : IRabbitMQQueueBindingDeclaration
	{
		public RabbitMQQueueBindingDeclaration(string exchangeName)
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