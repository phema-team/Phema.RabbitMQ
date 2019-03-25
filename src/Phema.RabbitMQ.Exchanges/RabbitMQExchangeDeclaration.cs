using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeDeclaration
	{
		string GroupName { get; }
		string ExchangeType { get; }
		string ExchangeName { get; }
		bool Durable { get; set; }
		bool Internal { get; set; }
		bool NoWait { get; set; }
		bool AutoDelete { get; set; }

		bool Deleted { get; set; }
		bool IfUnused { get; set; }

		IDictionary<string, object> Arguments { get; }
		IList<IRabbitMQExchangeBindingDeclaration> ExchangeBindings { get; }
	}

	internal sealed class RabbitMQExchangeDeclaration : IRabbitMQExchangeDeclaration
	{
		public RabbitMQExchangeDeclaration(string groupName, string exchangeType, string exchangeName)
		{
			GroupName = groupName;
			ExchangeType = exchangeType;
			ExchangeName = exchangeName;
			Arguments = new Dictionary<string, object>();
			ExchangeBindings = new List<IRabbitMQExchangeBindingDeclaration>();
		}

		public string GroupName { get; }
		public string ExchangeType { get; }
		public string ExchangeName { get; }
		public bool Durable { get; set; }
		public bool Internal { get; set; }
		public bool NoWait { get; set; }
		public bool AutoDelete { get; set; }

		public bool Deleted { get; set; }
		public bool IfUnused { get; set; }

		public IDictionary<string, object> Arguments { get; }
		public IList<IRabbitMQExchangeBindingDeclaration> ExchangeBindings { get; }
	}
}