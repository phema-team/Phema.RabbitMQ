using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Phema.Serialization;

namespace Phema.RabbitMQ
{
	public class Payload
	{
		public string Name { get; set; }
	}
	
	public class PayloadConsumer : IRabbitMQConsumer<Payload>
	{
		public Task Consume(Payload payload)
		{
			return Console.Out.WriteLineAsync(payload.Name);
		}
	}

	public class Class1
	{
		static async Task Main()
		{
			var services = new ServiceCollection();

			services.AddPhemaJsonSerializer();

			services.AddPhemaRabbitMQ("test", factory =>
			{
			});

			var provider = services.BuildServiceProvider();
			
			var rabbitmq = provider.GetRequiredService<IRabbitMQProvider>();

			rabbitmq.QueueGroup("Queues", group =>
			{
				group.Queue("Queue", queue =>
				{
					queue.Durable();
				});
			});

			rabbitmq.ExchangeGroup("Exchanges", group =>
				group.DirectExchange("Exchange", exchange =>
					exchange.Durable()));

			rabbitmq.ProducerGroup("Producers", group =>
				group.Producer<Payload>("Exchanges.Exchange", "Queues.Queue", 
					producer =>
						producer.Persistent()));

			rabbitmq.ConsumerGroup("Consumers", group =>
				group.Consumer<Payload, PayloadConsumer>("Queues.Queue", 
					producer =>
						producer.WithCount(1)));

			await provider.GetRequiredService<IRabbitMQProducer<Payload>>()
				.Produce(new Payload { Name = "Test1" });

			using (var scope = provider.CreateScope())
			{
				await scope.ServiceProvider.GetRequiredService<IRabbitMQProducer<Payload>>()
					.Produce(new Payload { Name = "Test2" });
			}
			
			rabbitmq.ProducerGroup("Producers", group =>
				group.Producer<Payload>("Exchanges.Exchange", "Queues.Queue", 
					producer =>
						producer.Persistent()
							.WaitForConfirms()));

			var p = provider.GetRequiredService<IRabbitMQProducer<Payload>>();
			
			for (int i = 0; i < 1_000; i++)
			{
				await p.Produce(new Payload { Name = "Test" + i });

				if (i == 500)
				{
					rabbitmq.ConsumerGroup("Consumers", group =>
						group.Consumer<Payload, PayloadConsumer>("Queues.Queue", 
							producer =>
								producer
									.Canceled()));
				}
			}

			Console.ReadLine();
		}
	}
}