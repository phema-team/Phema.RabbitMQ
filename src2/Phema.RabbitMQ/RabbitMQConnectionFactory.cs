using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConnectionFactory
	{
		IConnection CreateConnection(string groupName);
	}

	internal sealed class RabbitMQConnectionFactory : IRabbitMQConnectionFactory
	{
		private readonly string instanceName;
		private readonly ConnectionFactory factory;
		private readonly IDictionary<string, IConnection> connections;

		public RabbitMQConnectionFactory(string instanceName, Action<ConnectionFactory> factory)
		{
			this.instanceName = instanceName;

			this.factory = new ConnectionFactory
			{
				DispatchConsumersAsync = true,
				AutomaticRecoveryEnabled = false
			};
			factory(this.factory);

			connections = new ConcurrentDictionary<string, IConnection>();
		}

		public IConnection CreateConnection(string groupName)
		{
			groupName = groupName == RabbitMQDefaults.DefaultGroupName
				? instanceName
				: $"{instanceName}.{groupName}";

			if (!connections.TryGetValue(groupName, out var connection))
			{
				connections.Add(groupName, connection = factory.CreateConnection(groupName));
			}

			return connection;
		}
	}
}