using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeMetadata
	{
		string Type { get; }
		string Name { get; }
		bool Durable { get; set; }
		bool Internal { get; set; }
		bool NoWait { get; set; }
		bool Passive { get; set; }
		bool AutoDelete { get; set; }
		IDictionary<string, object> Arguments { get; }
		IList<IRabbitMQExchangeBindingMetadata> ExchangeBindings { get; }
	}

	internal sealed class RabbitMQExchangeMetadata : IRabbitMQExchangeMetadata
	{
		public RabbitMQExchangeMetadata(string type, string name)
		{
			Type = type;
			Name = name;
			Arguments = new Dictionary<string, object>();
			ExchangeBindings = new List<IRabbitMQExchangeBindingMetadata>();
		}

		public string Type { get; }
		public string Name { get; }
		public bool Durable { get; set; }
		public bool Internal { get; set; }
		public bool NoWait { get; set; }
		public bool Passive { get; set; }
		public bool AutoDelete { get; set; }
		public IDictionary<string, object> Arguments { get; }
		public IList<IRabbitMQExchangeBindingMetadata> ExchangeBindings { get; }
	}
}