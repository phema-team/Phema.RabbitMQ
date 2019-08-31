using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Phema.RabbitMQ.CustomSerialization
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
				await producer.Publish($"Message: {index++}");

				await Task.Delay(1000, stoppingToken);
			}
		}
	}
}