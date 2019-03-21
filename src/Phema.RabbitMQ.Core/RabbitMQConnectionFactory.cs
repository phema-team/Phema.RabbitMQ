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
		private readonly IConnectionFactory factory;
		private readonly IDictionary<string, IConnection> connections;

		public RabbitMQConnectionFactory(string instanceName, IConnectionFactory factory)
		{
			this.factory = factory;
			this.instanceName = instanceName;
			connections = new ConcurrentDictionary<string, IConnection>();
		}

		public IConnection CreateConnection(string groupName)
		{
			if (!connections.TryGetValue(groupName, out var connection))
			{
				connections.Add(groupName, 
					connection = groupName == RabbitMQDefaults.DefaultGroupName
						? factory.CreateConnection(instanceName)
						: factory.CreateConnection($"{instanceName}.{groupName}"));
			}

			return connection;
		}
	}
}