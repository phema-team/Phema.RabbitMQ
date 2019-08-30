using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Phema.RabbitMQ.ConsumerPriority
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
				await Task.Delay(500, stoppingToken);
				await producer.Produce($"Index: {index++}");
			}
		}
	}
}