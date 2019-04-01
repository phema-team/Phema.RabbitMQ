using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Phema.RabbitMQ;

namespace AspNetCoreProducerConsumer
{
	public class Payload
	{
	}
	
	public class PayloadConsumer : IRabbitMQConsumer<Payload>
	{
		public Task Consume(Payload payload)
		{
			return Task.CompletedTask;
		}
	}

	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddRabbitMQ("test", "amqp://test.test")
				.AddConsumerGroup("consumers", group =>
					group.AddAsyncConsumer<Payload, PayloadConsumer>("queue")
						.Tagged("tag"))
				.AddQueueGroup("queues", group =>
					group.AddQueue("queue")
						.Durable()
						.BoundTo("exchange"))
				.AddExchangeGroup("exchanges", group =>
					group.AddDirectExchange("exchange")
						.Durable())
				.AddProducerGroup("producers", group =>
					group.AddProducer<Payload>("exchange")
						.RoutingKey("queue")
						.Persistent());
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.Run(async context =>
			{
				var producer = context.RequestServices.GetRequiredService<IRabbitMQProducer<Payload>>();

				await producer.Produce(new Payload());
			});
		}
	}
}