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
		private readonly RabbitMQConsumerMetadata consumer;
		private readonly IServiceProvider provider;
		private readonly ISerializer serializer;

		public RabbitMQBasicConsumer(
			IServiceProvider provider,
			IModel channel,
			RabbitMQConsumerMetadata consumer)
			: base(channel)
		{
			this.provider = provider;
			this.consumer = consumer;
			serializer = provider.GetRequiredService<ISerializer>();
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
					if (!consumer.AutoAck)
						Model.BasicNack(deliveryTag, consumer.Multiple, !redelivered && consumer.Requeue);

					scope.ServiceProvider
						.GetService<ILogger<RabbitMQBasicConsumer<TPayload, TPayloadConsumer>>>()
						?.LogConsumerException<TPayload>(consumer, exception, body, redelivered);

					throw;
				}

				if (!consumer.AutoAck)
					Model.BasicAck(deliveryTag, consumer.Multiple);
			}
		}
	}
}