using System;
using Phema.Serialization;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConsumerFactory
	{
		IBasicConsumer CreateConsumer<TPayload, TPayloadConsumer>(IModel channel, IRabbitMQConsumerMetadata metadata)
			where TPayloadConsumer : IRabbitMQConsumer<TPayload>;
	}

	internal sealed class RabbitMQConsumerFactory : IRabbitMQConsumerFactory
	{
		private readonly IServiceProvider provider;
		private readonly ISerializer serializer;

		public RabbitMQConsumerFactory(IServiceProvider provider, ISerializer serializer)
		{
			this.provider = provider;
			this.serializer = serializer;
		}

		public IBasicConsumer CreateConsumer<TPayload, TPayloadConsumer>(IModel channel, IRabbitMQConsumerMetadata metadata)
			where TPayloadConsumer : IRabbitMQConsumer<TPayload>
		{
			return new RabbitMQBasicConsumer<TPayload, TPayloadConsumer>(provider, channel, metadata, serializer);
		}
	}
}