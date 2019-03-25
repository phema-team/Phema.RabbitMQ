using System;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducerDeclaration
	{
		string GroupName { get; }
		string ExchangeName { get; }
		string RoutingKey { get; set; }
		bool Mandatory { get; set; }
		bool Transactional { get; set; }
		bool WaitForConfirms { get; set; }
		bool Die { get; set; }
		TimeSpan? Timeout { get; set; }
		IDictionary<string, object> Arguments { get; }
		IList<Action<IBasicProperties>> Properties { get; }
	}

	// ReSharper disable once UnusedTypeParameter
	internal sealed class RabbitMQProducerDeclaration<TPayload> : IRabbitMQProducerDeclaration
	{
		public RabbitMQProducerDeclaration(string groupName, string exchangeName)
		{
			GroupName = groupName;
			ExchangeName = exchangeName;
			Properties = new List<Action<IBasicProperties>>();
			Arguments = new Dictionary<string, object>();
		}

		public string GroupName { get; }
		public string ExchangeName { get; }
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