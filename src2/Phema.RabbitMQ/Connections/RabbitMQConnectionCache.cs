using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConnectionCache
	{
		IConnection FromDeclaration(RabbitMQConnectionDeclaration connection);
	}

	internal sealed class RabbitMQConnectionCache : IRabbitMQConnectionCache
	{
		private readonly RabbitMQOptions options;
		private readonly ConcurrentDictionary<string, IConnection> connections;

		public RabbitMQConnectionCache(IOptions<RabbitMQOptions> options)
		{
			this.options = options.Value;
			connections = new ConcurrentDictionary<string, IConnection>();
		}

		public IConnection FromDeclaration(RabbitMQConnectionDeclaration declaration)
		{
			return connections.GetOrAdd(declaration.Name, _ => 
				new RabbitMQConnection(options.InstanceName is null
					? options.ConnectionFactory.CreateConnection(declaration.Name)
					: options.ConnectionFactory.CreateConnection($"{options.InstanceName}.{declaration.Name}")));
		}
	}
}