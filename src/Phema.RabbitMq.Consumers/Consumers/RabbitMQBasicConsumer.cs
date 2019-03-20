using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Phema.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQBasicConsumer<TPayload, TPayloadConsumer> : AsyncEventingBasicConsumer
		where TPayloadConsumer : IRabbitMQConsumer<TPayload>
	{
		private readonly IRabbitMQConsumerMetadata metadata;
		private readonly IServiceProvider provider;
		private readonly ISerializer serializer;

		public RabbitMQBasicConsumer(
			IServiceProvider provider,
			IModel channel,
			IRabbitMQConsumerMetadata metadata,
			ISerializer serializer)
			: base(channel)
		{
			this.provider = provider;
			this.metadata = metadata;
			this.serializer = serializer;
		}

		public override async Task HandleBasicDeliver(
			string consumerTag,
			ulong deliveryTag,
			bool redelivered,
			string exchange,
			string routingKey,
			IBasicProperties properties,
			byte[] body)
		{
			using (var scope = provider.CreateScope())
			{
				var model = serializer.Deserialize<TPayload>(body);

				try
				{
					await scope.ServiceProvider
						.GetRequiredService<TPayloadConsumer>()
						.Consume(model)
						.ConfigureAwait(false);
				}
				catch (Exception exception)
				{
					if (!metadata.AutoAck)
					{
						Model.BasicNack(deliveryTag, metadata.Multiple, !redelivered && metadata.Requeue);
					}

					// Remove logging reference, add .WhenConsumerExceptionHappened(...)
					scope.ServiceProvider
						.GetService<ILogger<RabbitMQBasicConsumer<TPayload, TPayloadConsumer>>>()
						?.LogConsumerException<TPayload>(metadata, exception, body, redelivered);

					throw;
				}

				if (!metadata.AutoAck)
				{
					Model.BasicAck(deliveryTag, metadata.Multiple);
				}
			}
		}
	}
}