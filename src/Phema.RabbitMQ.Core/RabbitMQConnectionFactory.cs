using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConnectionFactory
	{
		IConnection CreateConnection(string groupName);
	}
}

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQConnectionFactory : IRabbitMQConnectionFactory
	{
		private readonly RabbitMQOptions options;
		private readonly IDictionary<string, IConnection> connections;

		public RabbitMQConnectionFactory(IOptions<RabbitMQOptions> options)
		{
			this.options = options.Value;
			connections = new ConcurrentDictionary<string, IConnection>();
		}

		public IConnection CreateConnection(string groupName)
		{
			if (!connections.TryGetValue(groupName, out var connection))
			{
				// TODO: Move to options and defaults
				connections.Add(groupName, 
					connection = groupName == RabbitMQDefaults.DefaultGroupName
						? options.InstanceName == RabbitMQDefaults.DefaultInstanceName
							? options.ConnectionFactory.CreateConnection()
							: options.ConnectionFactory.CreateConnection(options.InstanceName)
						: options.ConnectionFactory.CreateConnection($"{options.InstanceName}.{groupName}"));
			}

			return connection;
		}
	}
}