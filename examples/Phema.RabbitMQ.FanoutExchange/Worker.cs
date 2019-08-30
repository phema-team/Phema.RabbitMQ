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
			while (!stoppingToken.IsCancellationRequested)
			{
				await producer.Produce(new ToQueue
				{
					Name = "Alice"
				});

				await Task.Delay(200, stoppingToken);
			}
		}
	}
}