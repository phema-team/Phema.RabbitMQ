using System.Collections.Generic;
using System.Text.Json;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public class RabbitMQOptions
	{
		public RabbitMQOptions()
		{
			InstanceName = null;
			ConnectionFactory = new ConnectionFactory
			{
				DispatchConsumersAsync = true,
				// TODO: Hack for now, because StackOverflowException
				AutomaticRecoveryEnabled = false
			};
			JsonSerializerOptions = new JsonSerializerOptions();

			ConnectionDeclarations = new List<RabbitMQConnectionDeclaration>();
			ExchangeDeclarations = new List<RabbitMQExchangeDeclaration>();
			QueueDeclarations = new List<RabbitMQQueueDeclaration>();
			ConsumerDeclarations = new List<RabbitMQConsumerDeclaration>();
			ProducerDeclarations = new List<RabbitMQProducerDeclaration>();
		}

		public string InstanceName { get; set; }
		public ConnectionFactory ConnectionFactory { get; set; }
		public JsonSerializerOptions JsonSerializerOptions { get; set; }

		public IList<RabbitMQConnectionDeclaration> ConnectionDeclarations { get; }
		public IList<RabbitMQExchangeDeclaration> ExchangeDeclarations { get; }
		public IList<RabbitMQQueueDeclaration> QueueDeclarations { get; }
		public List<RabbitMQConsumerDeclaration> ConsumerDeclarations { get; }
		public List<RabbitMQProducerDeclaration> ProducerDeclarations { get; }
	}
}