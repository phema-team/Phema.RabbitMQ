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

		public RabbitMQConnectionFactory(string instanceName, IConnectionFactory factory)
		{
			this.instanceName = instanceName;
			this.factory = factory;
		}

		public IConnection CreateConnection(string groupName)
		{
			return groupName == null
				? factory.CreateConnection(instanceName)
				: factory.CreateConnection($"{instanceName}_{groupName}");
		}
	}
}