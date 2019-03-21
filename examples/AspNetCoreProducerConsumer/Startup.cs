using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Phema.RabbitMQ;
using Phema.Serialization;

namespace AspNetCoreProducerConsumer
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddPhemaJsonSerializer();

			services.AddPhemaRabbitMQ("TestInstance", "amqp://username:password@url/vhost")
				.AddQueueGroup("Queues", group =>
					group.AddQueue("TestQueue")
						.Durable()
						.AutoDelete())
				.AddConsumerGroup("Consumers", group =>
					group.AddConsumer<Payload, PayloadConsumer>("Queues.TestQueue")
						.WithTag("TestConsumer")
						.AutoAck()
						.WithCount(2))
				.AddExchangeGroup("Exchanges", group =>
					group.AddFanoutExchange("TestExchange")
						.Durable()
						.AutoDelete())
				.AddProducerGroup("Producers", group =>
					group.AddProducer<Payload>("Exchanges.TestExchange", "Queues.TestQueue")
						.Persistent()
						.WaitForConfirms());
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.Run(async context =>
			{
				var producer = app.ApplicationServices.GetRequiredService<IRabbitMQProducer<Payload>>();

				for (var index = 0; index < 10; index++)
				{
					await producer.Produce(
						new Payload { Name = $"Produce: {index}"});
				}

				await producer.BatchProduce(
					new Payload { Name = "Produce: 11" },
					new Payload { Name = "Produce: 12" });
			});
		}
	}
}