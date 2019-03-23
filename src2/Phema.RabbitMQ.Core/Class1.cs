using System;
using System.Linq;
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

			rabbitmq.Queue("Queue", 
				queue => queue.Durable());

			rabbitmq.FanoutExchange("Exchange",
				exchange => exchange.Durable());

			rabbitmq.Producer<Payload>("Exchange", "Queue");

			rabbitmq.Consumer<Payload, PayloadConsumer>("Queue",
				producer => producer.AutoAck().WithCount(10));

			await provider.GetRequiredService<IRabbitMQProducer<Payload>>()
				.BatchProduce(Enumerable.Range(0, 1_000_000).Select(x => new Payload { Name = "Test" + x }));

			Console.ReadLine();
		}
	}
}