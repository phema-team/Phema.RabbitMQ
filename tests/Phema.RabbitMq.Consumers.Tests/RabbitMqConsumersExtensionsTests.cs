using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Xunit;

namespace Phema.RabbitMQ.Consumers.Tests
{
	public class TestPayload
	{
	}

	public class TestPayloadConsumer : IRabbitMQConsumer<TestPayload>
	{
		public async Task Consume(TestPayload payload)
		{
		}
	}

	public class RabbitMQConsumersExtensionsTests
	{
		[Fact]
		public void UXTest()
		{
			var services = new ServiceCollection();
			
			services.AddPhemaRabbitMQ("instance")
				.AddConsumers(options =>
					options.AddConsumer<TestPayload, TestPayloadConsumer>("queuename")
						.WithTag("consumertag")
						.WithPrefetch(0)
						.WithConsumers(1)
						.Exclusive()
						.NoLocal()
						.AutoAck()
						.Requeue(true)
						.WithArgument("x-argument", "value")
						.WithPriority(10));
		}
		
		[Fact]
		public void ConsumersRegistered()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMQ("instance")
				.AddConsumers(options =>
					options.AddConsumer<TestPayload, TestPayloadConsumer>("queuename")
						.WithTag("consumertag")
						.WithPrefetch(0)
						.WithConsumers(1)
						.Exclusive()
						.NoLocal()
						.AutoAck()
						.Requeue(true)
						.WithArgument("x-argument", "value"));

			Assert.Single(services.Where(s => s.ServiceType == typeof(TestPayloadConsumer)));
			
			var provider = services.BuildServiceProvider();

			var consumers = provider.GetRequiredService<IOptions<RabbitMQConsumersOptions>>().Value;

			Assert.Single(consumers.ConsumerDispatchers);
		}
	}
}