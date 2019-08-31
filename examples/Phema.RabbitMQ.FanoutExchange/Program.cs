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
							var exchange = connection.AddFanoutExchange<ToQueue>("exchange")
								.AutoDelete()
								.Durable();

							var queue1 = connection.AddQueue<ToQueue>("queue1")
								.AutoDelete()
								// Typed checks
								.BoundTo(exchange);

							var queue2 = connection.AddQueue<ToQueue>("queue2")
								.AutoDelete()
								// Typed checks
								.BoundTo(exchange);

							// Typed checks
							connection.AddConsumer(queue1)
								.Dispatch(async (scope, payload) =>
								{
									await Console.Out.WriteLineAsync("1:" + payload.Name);
								});

							// Typed checks
							connection.AddConsumer(queue2)
								.Dispatch(async (scope, payload) =>
								{
									await Console.Out.WriteLineAsync("2:" + payload.Name);
								});

							connection.AddProducer(exchange);
						});

					services.AddHostedService<Worker>();
				});
		}
	}
}