using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Phema.RabbitMQ.Workers
{
	public class BackgroundService : IHostedService
	{
		private readonly IRabbitMQProducer producer;

		public BackgroundService(IRabbitMQProducer producer)
		{
			this.producer = producer;
		}

		public async  Task StartAsync(CancellationToken cancellationToken)
		{
			var index = 0;
			
			while (!cancellationToken.IsCancellationRequested)
			{
				if (index % 4 == 0)
				{
					await producer.Produce(new ToQueue1{ Payload = "Ка"});
				}
				else
				{
					await producer.Produce(new ToQueue2 { Payload = "Ча"});
				}

				index++;
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}