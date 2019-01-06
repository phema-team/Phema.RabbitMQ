using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Phema.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Phema.Rabbit
{
	/// <summary>
	/// Used for consuming rabbit messages
	/// </summary>
	/// <typeparam name="TPayload"></typeparam>
	/// <typeparam name="TRabbitConsumer"></typeparam>
	internal class RabbitBasicConsumer<TPayload, TRabbitConsumer> : AsyncEventingBasicConsumer
		where TRabbitConsumer : RabbitConsumer<TPayload>
	{
		private readonly IServiceProvider provider;

		public RabbitBasicConsumer(IServiceProvider provider, IModel model) : base(model)
		{
			this.provider = provider;
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
				var consumer = scope.ServiceProvider.GetRequiredService<TRabbitConsumer>();
				var serializer = scope.ServiceProvider.GetRequiredService<ISerializer>();

				var model = serializer.Deserialize<TPayload>(body);

				try
				{
					await consumer.Consume(model);
				}
				catch (Exception)
				{
					if (!consumer.AutoAck)
					{
						Model.BasicNack(deliveryTag, false, consumer.Requeue);
					}
					
					throw;
				}
				
				if (!consumer.AutoAck)
				{
					Model.BasicAck(deliveryTag, false);
				}
			}
		}
	}
}