using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Phema.RabbitMQ.Workers
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureServices((context, services) =>
				{
					services.AddRabbitMQ("instance")
						.AddConnection("name", connection =>
						{
							var exchange = connection.AddDirectExchange("exchange1")
								.Durable()
								.AutoDelete();

							var queue1 = connection.AddQueue<ToQueue1>("queue1")
								.Durable()
								.AutoDelete()
								.BoundTo(exchange, binding => binding.RoutingKey("1"));

							var queue2 = connection.AddQueue<ToQueue2>("queue2")
								.Durable()
								.AutoDelete()
								.BoundTo(exchange, binding => binding.RoutingKey("2"));

							connection.AddConsumer(queue1, Consumer1)
								.AutoAck();

							connection.AddConsumer(queue2, Consumer2)
								.AutoAck();

							connection.AddProducer<ToQueue1>(exchange)
								.RoutingKey("1");

							connection.AddProducer<ToQueue2>(exchange)
								.RoutingKey("2");
						});

					services.AddHostedService<BackgroundService>();
				});

		private static async ValueTask Consumer1(IServiceScope scope, ToQueue1 payload)
		{
			await Console.Out.WriteLineAsync("1" + payload.Payload);
		}
		
		private static async ValueTask Consumer2(IServiceScope scope, ToQueue2 payload)
		{
			await Console.Out.WriteLineAsync("2 " + payload.Payload);
		}
	}

	public class ToQueue1
	{
		public string Payload { get; set; }
	}

	public class ToQueue2
	{
		public string Payload { get; set; }
	}
}