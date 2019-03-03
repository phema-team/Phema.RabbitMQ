using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Phema.Serialization;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Phema.RabbitMq
{
	internal sealed class RabbitMqBasicConsumer<TPayload, TPayloadConsumer> : AsyncEventingBasicConsumer
		where TPayloadConsumer : IRabbitMqConsumer<TPayload>
	{
		private readonly RabbitMqConsumer<TPayload, TPayloadConsumer> consumer;
		private readonly IServiceProvider provider;
		private readonly ISerializer serializer;

		public RabbitMqBasicConsumer(
			IServiceProvider provider,
			IModel channel,
			RabbitMqConsumer<TPayload, TPayloadConsumer> consumer)
			: base(channel)
		{
			this.provider = provider;
			serializer = provider.GetRequiredService<ISerializer>();
			this.consumer = consumer;
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
						.Consume(model);
				}
				catch (Exception exception)
				{
					if (!consumer.AutoAck) Model.BasicNack(deliveryTag, consumer.Multiple, !redelivered && consumer.Requeue);

					scope.ServiceProvider
						.GetService<ILogger<RabbitMqBasicConsumer<TPayload, TPayloadConsumer>>>()
						?.LogConsumerException<TPayload>(consumer, exception, redelivered);

					throw;
				}

				if (!consumer.AutoAck) Model.BasicAck(deliveryTag, consumer.Multiple);
			}
		}
	}
}