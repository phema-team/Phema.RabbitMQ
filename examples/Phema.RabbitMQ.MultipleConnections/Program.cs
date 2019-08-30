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
					var builder = services.AddRabbitMQ();

					builder.AddConnection("main", main =>
					{
						var exchange = main.AddFanoutExchange<ToQueue>("exchange")
							.AutoDelete()
							.Durable();

						var queue = main.AddQueue<ToQueue>("queue")
							.AutoDelete()
							.BoundTo(exchange);

						builder.AddConnection("consumers", consumers =>
						{
							consumers.AddConsumer(queue, async (scope, payload) =>
							{
								await Console.Out.WriteLineAsync(
									$"Connection: {consumers.Declaration.Name}, payload: {payload.Id.ToString()}");
							});
						});

						builder.AddConnection("producers", producers =>
						{
							producers.AddProducer(exchange);
						});
					});

					services.AddHostedService<Worker>();
				});
		}
	}
}