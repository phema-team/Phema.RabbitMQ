using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static class RabbitMQDefaults
	{
		public const string DefaultInstanceName = "";
		public const string DefaultGroupName = "";
		
		public static ConnectionFactory ConnectionFactory => new ConnectionFactory
		{
			DispatchConsumersAsync = true,
			// TODO: Hack for now, because StackOverflowException
			AutomaticRecoveryEnabled = false
		};
	}
}