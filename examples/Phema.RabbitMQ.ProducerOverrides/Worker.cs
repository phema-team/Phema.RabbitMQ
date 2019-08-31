using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Phema.RabbitMQ.ProducerOverrides
{
	public class Worker : BackgroundService
	{
		private readonly IRabbitMQProducer producer;

		public Worker(IRabbitMQProducer producer)
		{
			this.producer = producer;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var index = 0;

			while (!stoppingToken.IsCancellationRequested)
			{
				if (index++ % 2 == 0)
				{
					await producer.Produce("Send to queue1",
						builder => builder.RoutedTo("queue1"));
				}
				else
				{
					await producer.Produce("Send to queue2",
						builder => builder.RoutedTo("queue2"));
				}

				await Task.Delay(1000, stoppingToken);
			}
		}
	}
}