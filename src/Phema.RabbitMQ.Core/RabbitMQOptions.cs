using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public sealed class RabbitMQOptions
	{
		public RabbitMQOptions()
		{
			InstanceName = RabbitMQDefaults.DefaultInstanceName;
			ConnectionFactory = RabbitMQDefaults.ConnectionFactory;
		}
		
		public string InstanceName { get; set; }
		public ConnectionFactory ConnectionFactory { get; }
	}
}