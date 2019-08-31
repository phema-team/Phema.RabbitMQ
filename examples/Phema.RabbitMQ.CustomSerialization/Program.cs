using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Phema.RabbitMQ.CustomSerialization
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
					services.AddRabbitMQ(options =>
							options.UseSerialization(
								serializer: payload => Encoding.UTF8.GetBytes(payload.ToString()),
								deserializer: (bytes, type) => Encoding.UTF8.GetString(bytes)))
						.AddConnection(connection =>
						{
							var exchange = connection.AddDirectExchange("exchange")
								.AutoDelete();

							var queue = connection.AddQueue<string>("queue")
								.AutoDelete()
								.BoundTo(exchange);

							connection.AddConsumer(queue)
								.Subscribe(async payload => await Console.Out.WriteLineAsync(payload));

							connection.AddProducer<string>(exchange)
								.RoutedTo(queue);
						});

					services.AddHostedService<Worker>();
				});
	}
}