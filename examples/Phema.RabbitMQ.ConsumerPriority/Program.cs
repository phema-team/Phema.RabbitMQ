using System;
using System.Threading.Tasks;
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

							var queue = connection.AddQueue<string>("queue")
								.AutoDelete()
								.BoundTo(exchange);

							// Typed checks
							connection.AddConsumer(queue)
								.Prefetch(1)
								.Priority(1)
								.Subscribe(async payload =>
								{
									await Console.Out.WriteLineAsync("1" + payload);
								});

							// Typed checks
							connection.AddConsumer(queue)
								.Prefetch(1)
								.Priority(2)
								.Subscribe(async payload =>
								{
									await Console.Out.WriteLineAsync("2" + payload);
									await Task.Delay(2000);
								});

							connection.AddProducer<string>(exchange)
								// Typed checks
								.RoutedTo(queue);
						});

					services.AddHostedService<Worker>();
				});
		}
	}
}