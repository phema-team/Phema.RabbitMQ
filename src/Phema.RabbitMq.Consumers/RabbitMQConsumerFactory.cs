using System;
using RabbitMQ.Client;

using ISerializer = Phema.Serialization.ISerializer;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConsumerFactory
	{
		IBasicConsumer CreateConsumer<TPayload, TPayloadConsumer>(IModel channel, IRabbitMQConsumerDeclaration declaration)
			where TPayloadConsumer : IRabbitMQConsumer<TPayload>;
	}
}

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQConsumerFactory : IRabbitMQConsumerFactory
	{
		private readonly ISerializer serializer;
		private readonly IServiceProvider provider;

		public RabbitMQConsumerFactory(IServiceProvider provider, ISerializer serializer)
		{
			this.provider = provider;
			this.serializer = serializer;
		}

		public IBasicConsumer CreateConsumer<TPayload, TPayloadConsumer>(IModel channel, IRabbitMQConsumerDeclaration declaration)
			where TPayloadConsumer : IRabbitMQConsumer<TPayload>
		{
			return new RabbitMQBasicConsumer<TPayload, TPayloadConsumer>(provider, channel, declaration, serializer);
		}
	}
}