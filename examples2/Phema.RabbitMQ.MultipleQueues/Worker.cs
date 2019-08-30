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
				if (index++ % 2 == 0)
				{
					await producer.Produce(new ToQueue1
					{
						Name = "Alice"
					});
				}
				else
				{
					await producer.Produce(new ToQueue2
					{
						Age = 72
					});
				}

				await Task.Delay(200, stoppingToken);
			}
		}
	}
}