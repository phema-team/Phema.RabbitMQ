using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Phema.Rabbit
{
	public class RabbitBasicConsumer<TPayload, TRabbitConsumer> : AsyncEventingBasicConsumer
		where TRabbitConsumer : RabbitConsumer<TPayload>
	{
		private readonly IServiceProvider provider;

		public RabbitBasicConsumer(IServiceProvider provider, IModel model) : base(model)
		{
			this.provider = provider;
		}
		
		public override async Task HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey,
			IBasicProperties properties, byte[] body)
		{
			using (var scope = provider.CreateScope())
			{
				var scopeProvider = scope.ServiceProvider;
				
				var consumer = scopeProvider.GetRequiredService<TRabbitConsumer>();
				var options = scopeProvider.GetRequiredService<IOptions<RabbitOptions>>().Value;

				var model = JsonConvert.DeserializeObject<TPayload>(options.Encoding.GetString(body));

				try
				{
					await consumer.Consume(model);
				}
				catch (Exception)
				{
					if (!consumer.AutoAck)
					{
						Model.BasicNack(deliveryTag, false, true);
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