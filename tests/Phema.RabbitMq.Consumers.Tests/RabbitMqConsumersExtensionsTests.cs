using System.Linq;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Xunit;

namespace Phema.RabbitMq.Consumers.Tests
{
	public class TestPayload
	{
	}

	public class TestPayloadConsumer : IRabbitMqConsumer<TestPayload>
	{
		public async ValueTask Consume(TestPayload payload)
		{
		}
	}

	public class RabbitMqConsumersExtensionsTests
	{
		[Fact]
		public void ConsumersRegistered()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMq("instance")
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

			var consumers = provider.GetRequiredService<IOptions<RabbitMqConsumersOptions>>().Value;

			Assert.Single(consumers.ConsumerDispatchers);
		}
	}
}