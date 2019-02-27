using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Phema.RabbitMq;
using Phema.Serialization;

using Xunit;

namespace TestProject1
{
	public class TestPayload
	{
		public string Name { get; set; }
	}

	public class TestPayloadConsumer : IRabbitMqConsumer<TestPayload>
	{
		public ValueTask Consume(TestPayload payload)
		{
			return new ValueTask();
		}
	}

	public class UnitTest1
	{
		[Fact]
		public async Task Test1()
		{
			var services = new ServiceCollection();

			services.AddJsonSerializer();
			
			services.AddPhemaRabbitMq("test", options => options.HostName = "database.otaku-shelter.ru")
				.AddQueues(options =>
						options.AddQueue("queuename")
							.Durable()
					// .Exclusive()
					// .AutoDelete()
					/*.WithArgument(null)*/)
				.AddConsumers(options =>
						options.AddConsumer<TestPayload, TestPayloadConsumer>("queuename")
							.WithConsumerTag("consumertag")
							.WithPrefetch(0)
							.WithConsumers(1)
							.Exclusive()
							.NoLocal()
							.AutoAck()
							.Requeue(multiple: true)
					/*.WithArgument(null)*/)
				.AddExchanges(options =>
						options.AddDirectExchange("amq.direct")
							.Durable()
					// .AutoDelete()
					/*.WithArgument(null)*/)
				.AddProducers(options =>
					options.AddProducer<TestPayload>("amq.direct", "queuename")
						.Mandatory()
						.WithProperty(p => p.Persistent = true));

			var provider = services.BuildServiceProvider();

			var producer = provider.GetRequiredService<IRabbitMqProducer<TestPayload>>();

			for (int i = 0; i < 1_000; i++)
			{
				await producer.Produce(new TestPayload {Name = $"Test{i}"});
			}
			
			Thread.Sleep(1000);

			var service = provider.GetRequiredService<IHostedService>();

			await service.StartAsync(CancellationToken.None);
			
			Thread.Sleep(1000);

			await service.StopAsync(CancellationToken.None);
		}
	}
}