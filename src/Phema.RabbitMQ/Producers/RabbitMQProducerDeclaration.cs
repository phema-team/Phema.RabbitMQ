using System;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public sealed class RabbitMQProducerDeclaration
	{
		public RabbitMQProducerDeclaration(Type type, RabbitMQConnectionDeclaration connection, RabbitMQExchangeDeclaration exchange)
		{
			Type = type;
			Connection = connection;
			Exchange = exchange;
			Properties = new List<Action<IBasicProperties>>();
			Arguments = new Dictionary<string, object>();
		}

		public Type Type { get; }
		public RabbitMQConnectionDeclaration Connection { get; }
		public RabbitMQExchangeDeclaration Exchange { get; }
		public string RoutingKey { get; set; }
		public bool Mandatory { get; set; }
		public bool Transactional { get; set; }
		public bool WaitForConfirms { get; set; }
		public bool Die { get; set; }
		public TimeSpan? Timeout { get; set; }
		public IDictionary<string, object> Arguments { get; }
		public IList<Action<IBasicProperties>> Properties { get; }
	}
}