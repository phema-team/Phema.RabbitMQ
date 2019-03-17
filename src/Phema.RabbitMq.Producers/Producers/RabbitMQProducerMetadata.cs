using System;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducerMetadata
	{
		string ExchangeName { get; }
		string QueueName { get; }
		string RoutingKey { get; set; }
		bool Mandatory { get; set; }
		IDictionary<string, object> Arguments { get; }
		IList<Action<IBasicProperties>> Properties { get; }
	}

	internal sealed class RabbitMQProducerMetadata : IRabbitMQProducerMetadata
	{
		public RabbitMQProducerMetadata(string exchangeName, string queueName)
		{
			ExchangeName = exchangeName;
			QueueName = queueName;
			Properties = new List<Action<IBasicProperties>>();
			Arguments = new Dictionary<string, object>();
		}

		public string ExchangeName { get; }
		public string QueueName { get; }
		public string RoutingKey { get; set; }
		public bool Mandatory { get; set; }
		public IDictionary<string, object> Arguments { get; }
		public IList<Action<IBasicProperties>> Properties { get; }
	}
}