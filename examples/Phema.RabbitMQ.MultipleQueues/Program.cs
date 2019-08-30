using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Phema.RabbitMQ.ConsumerPriority
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.ConfigureServices((hostContext, services) =>
				{
					services.AddRabbitMQ()
						.AddConnection("connection", connection =>
						{
							var exchange = connection.AddDirectExchange("exchange")
								.AutoDelete()
								.Durable();

							var queue1 = connection.AddQueue<ToQueue1>("queue1")
								.AutoDelete()
								.BoundTo(exchange);

							var queue2 = connection.AddQueue<ToQueue2>("queue2")
								.AutoDelete()
								.BoundTo(exchange);

							// Typed checks
							connection.AddConsumer(queue1, async (scope, payload) =>
							{
								await Console.Out.WriteLineAsync("1:" + payload.Name);
							});

							// Typed checks
							connection.AddConsumer(queue2, async (scope, payload) =>
							{
								await Console.Out.WriteLineAsync("2:" + payload.Age);
							});

							connection.AddProducer<ToQueue1>(exchange)
								// Typed checks
								.ToQueue(queue1);

							connection.AddProducer<ToQueue2>(exchange)
								// Typed checks
								.ToQueue(queue2);
						});

					services.AddHostedService<Worker>();
				});
		}
	}
}