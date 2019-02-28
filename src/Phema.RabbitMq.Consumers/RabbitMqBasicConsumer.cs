using System;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;

using Phema.Serialization;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Phema.RabbitMq
{
	internal sealed class RabbitMqBasicConsumer<TPayload, TPayloadConsumer> : AsyncEventingBasicConsumer
		where TPayloadConsumer : IRabbitMqConsumer<TPayload>
	{
		private readonly IServiceProvider provider;
		private readonly RabbitMqConsumer consumer;

		public RabbitMqBasicConsumer(
			IServiceProvider provider,
			IModel channel,
			RabbitMqConsumer consumer)
			: base(channel)
		{
			this.provider = provider;
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
				var serializer = scope.ServiceProvider.GetRequiredService<ISerializer>();

				var model = serializer.Deserialize<TPayload>(body);

				try
				{
					await scope.ServiceProvider
						.GetRequiredService<TPayloadConsumer>()
						.Consume(model);
				}
				catch
				{
					if (!consumer.AutoAck)
					{
						Model.BasicNack(deliveryTag, consumer.Multiple, !redelivered && consumer.Requeue);
					}
					
					throw;
				}
				
				if (!consumer.AutoAck)
				{
					Model.BasicAck(deliveryTag, consumer.Multiple);
				}
			}
		}
	}
}