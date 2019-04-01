using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Phema.RabbitMQ;

namespace ConsoleProducerConsumer
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

	public class Program
	{
		private static async Task Main()
		{
			var services = new ServiceCollection();

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

			var provider = services.BuildServiceProvider();

			// Start hosted services for declaration and consumers
			foreach (var hostedService in provider.GetServices<IHostedService>())
			{
				await hostedService.StartAsync(CancellationToken.None);
			}

			var producer = provider.GetRequiredService<IRabbitMQProducer<Payload>>();

			await producer.Produce(new Payload());

			Console.ReadLine();
		}
	}
}