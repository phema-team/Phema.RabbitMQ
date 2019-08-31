using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Phema.RabbitMQ.ProducerOverrides
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureServices((hostContext, services) =>
				{
					services.AddRabbitMQ()
						.AddConnection("connection", connection =>
						{
							var exchange = connection.AddDirectExchange<string>("exchange")
								.AutoDelete();

							var queue1 = connection.AddQueue<string>("queue1")
								.BoundTo(exchange);

							var queue2 = connection.AddQueue<string>("queue2")
								.BoundTo(exchange);

							connection.AddConsumer(queue1, queue2)
								.Dispatch(async payload => await Console.Out.WriteLineAsync(payload));

							connection.AddProducer(exchange);
						});
					
					services.AddHostedService<Worker>();
				});
	}
}