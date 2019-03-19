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
		public Task Consume(TestPayload payload)
		{
			return Task.CompletedTask;
		}
	}

	public class RabbitMQConsumersExtensionsTests
	{
		[Fact]
		public void ConsumersRegistered()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMQ("instance")
				.AddConsumers(options =>
					options.AddConsumer<TestPayload, TestPayloadConsumer>("queuename")
						.WithTag("consumertag")
						.WithPrefetchCount(0)
						.WithPrefetchSize(0)
						.WithCount(1)
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

		[Fact]
		public void UXTest()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMQ("instance")
				.AddConsumers(options =>
					options.AddConsumer<TestPayload, TestPayloadConsumer>("queuename")
						.WithTag("consumertag")
						.WithPrefetchCount(0)
						.WithCount(1)
						.Exclusive()
						.NoLocal()
						.AutoAck()
						.Requeue(true)
						.WithArgument("x-argument", "value")
						.WithPriority(10));
		}
	}
}