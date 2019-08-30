using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Phema.RabbitMQ.ConsumerPriority
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureLogging(builder => builder.AddConsole())
				.ConfigureServices((hostContext, services) =>
				{
					services.AddRabbitMQ()
						.AddConnection("connection", connection =>
						{
							var exchange = connection.AddDirectExchange("exchange")
								.AutoDelete()
								.Durable();

							// Click requests
							var clickRequests = connection.AddQueue<ClickRequest>("clickRequests")
								.AutoDelete()
								.Durable()
								.BoundTo(exchange);

							connection.AddProducer<ClickRequest>(exchange)
								.ToQueue(clickRequests);

							connection.AddConsumer(clickRequests, async (scope, click) =>
							{
								Console.WriteLine("Click request: " + click.Id);

								var producer = scope.ServiceProvider.GetRequiredService<IRabbitMQProducer>();
								
								await producer.Produce(new ProcessClick
								{
									Id = click.Id
								});
							});

							// Processing clicks
							var processClicks = connection.AddQueue<ProcessClick>("processClicks")
								.AutoDelete()
								.Durable()
								.BoundTo(exchange);

							connection.AddProducer<ProcessClick>(exchange)
								.ToQueue(processClicks);

							connection.AddConsumer(processClicks, async (scope, click) =>
							{
								await Console.Out.WriteLineAsync($"Click {click.Id} processed");
							})
								.Count(2);
						});

					services.AddHostedService<Worker>();
				});
	}
}