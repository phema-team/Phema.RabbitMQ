using RabbitMQ.Client;
using ISerializer = Phema.Serialization.ISerializer;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducerFactory
	{
		IRabbitMQProducer<TPayload> CreateProducer<TPayload>(IModel channel, IRabbitMQProducerDeclaration declaration);
	}
}

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQProducerFactory : IRabbitMQProducerFactory
	{
		private readonly ISerializer serializer;

		public RabbitMQProducerFactory(ISerializer serializer)
		{
			this.serializer = serializer;
		}

		public IRabbitMQProducer<TPayload> CreateProducer<TPayload>(
			IModel channel,
			IRabbitMQProducerDeclaration declaration)
		{
			return new RabbitMQProducer<TPayload>(
				channel,
				serializer,
				declaration,
				CreateBasicProperties(channel, declaration));
		}

		private static IBasicProperties CreateBasicProperties(IModel channel, IRabbitMQProducerDeclaration producer)
		{
			var properties = channel.CreateBasicProperties();

			foreach (var property in producer.Properties)
			{
				property(properties);
			}

			return properties;
		}
	}
}