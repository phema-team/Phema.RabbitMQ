using System;
using System.Threading.Tasks;
using Phema.RabbitMQ;

namespace AspNetCoreProducerConsumer
{
	public class PayloadConsumer : IRabbitMQConsumer<Payload>
	{
		public async Task Consume(Payload payload)
		{
			await Console.Out.WriteLineAsync(payload.Name);
		}
	}
}