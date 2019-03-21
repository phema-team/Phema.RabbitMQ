using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Phema.RabbitMQ;
using Phema.Serialization;

namespace SimpleProducerConsumer
{
	public class Program
	{
		private static async Task Main()
		{
			var services = new ServiceCollection();

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

			var provider = services.BuildServiceProvider();

			// Start consuming manualy
			await provider.GetRequiredService<IHostedService>().StartAsync(CancellationToken.None);

			var producer = provider.GetRequiredService<IRabbitMQProducer<Payload>>();

			for (var index = 0; index < 10; index++)
			{
				await producer.Produce(
					new Payload { Name = $"Produce: {index}"});
			}

			await producer.BatchProduce(
				new Payload { Name = "Produce: 11" },
				new Payload { Name = "Produce: 12" });

			Console.ReadLine();
		}
	}
}