using System;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public sealed class RabbitMQProducerDeclaration
	{
		public RabbitMQProducerDeclaration(
			Type type,
			RabbitMQConnectionDeclaration connectionDeclaration,
			RabbitMQExchangeDeclaration exchangeDeclaration)
		{
			Type = type;
			ConnectionDeclaration = connectionDeclaration;
			ExchangeDeclaration = exchangeDeclaration;
			Properties = new List<Action<IBasicProperties>>();
			Arguments = new Dictionary<string, object>();
		}

		public Type Type { get; }
		public RabbitMQConnectionDeclaration ConnectionDeclaration { get; }
		public RabbitMQExchangeDeclaration ExchangeDeclaration { get; }
		public string RoutingKey { get; set; }
		public bool Mandatory { get; set; }
		public bool Transactional { get; set; }
		public bool WaitForConfirms { get; set; }
		public bool Die { get; set; }
		public TimeSpan? Timeout { get; set; }
		public IDictionary<string, object> Arguments { get; set; }
		public IList<Action<IBasicProperties>> Properties { get; set; }

		public static RabbitMQProducerDeclaration FromDeclaration(RabbitMQProducerDeclaration declaration)
		{
			return new RabbitMQProducerDeclaration(
				declaration.Type,
				declaration.ConnectionDeclaration,
				declaration.ExchangeDeclaration)
			{
				RoutingKey = declaration.RoutingKey,
				Mandatory = declaration.Mandatory,
				Transactional = declaration.Transactional,
				WaitForConfirms = declaration.WaitForConfirms,
				Die = declaration.Die,
				Timeout = declaration.Timeout,
				Arguments = declaration.Arguments,
				Properties = declaration.Properties
			};
		}
	}
}