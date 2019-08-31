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

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args)
				.ConfigureLogging(builder => builder.AddConsole())
				.ConfigureServices((hostContext, services) =>
				{
					services.AddRabbitMQ(options => options
							.UseConnectionFactory(factory => factory.ClientProvidedName = "pipeline"))
						.AddConnection("connection", connection =>
						{
							var exchange = connection.AddDirectExchange("exchange")
								.AutoDelete()
								.Durable();

							// Click requests
							var clickRequests = connection.AddQueue<ClickRequest>("click_requests")
								.AutoDelete()
								.Durable()
								.BoundTo(exchange);

							connection.AddProducer<ClickRequest>(exchange)
								.RoutedTo(clickRequests);

							connection.AddConsumer(clickRequests)
								.Subscribe(async (scope, click) =>
								{
									Console.WriteLine("Click request: " + click.Id);

									var producer = scope.ServiceProvider.GetRequiredService<IRabbitMQProducer>();

									await producer.Publish(new ProcessClick
									{
										Id = click.Id
									});
								});

							// Processing clicks
							var processClicks = connection.AddQueue<ProcessClick>("process_clicks")
								.AutoDelete()
								.Durable()
								.BoundTo(exchange);

							connection.AddProducer<ProcessClick>(exchange)
								.RoutedTo(processClicks);

							connection.AddConsumer(processClicks)
								.Count(2)
								.Subscribe(async (scope, click) =>
								{
									await Console.Out.WriteLineAsync($"Click {click.Id} processed");
								});
						});

					services.AddHostedService<Worker>();
				});
		}
	}
}