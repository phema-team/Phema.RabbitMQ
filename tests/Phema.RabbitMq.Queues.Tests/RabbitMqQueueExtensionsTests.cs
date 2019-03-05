using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Xunit;

namespace Phema.RabbitMQ.Queues.Tests
{
	public class RabbitMQQueueExtensionsTests
	{
		[Fact]
		public void UXTest()
		{
			var services = new ServiceCollection();
			
			services.AddPhemaRabbitMQ("instance")
				.AddQueues(options =>
					options.AddQueue("queuename")
						.Durable()
						.Exclusive()
						.AutoDelete()
						.WithArgument("x-argument", "somevalue")
						.WithMaxMessageCount(10)
						.WithMaxMessageSize(10000)
						.WithDeadLetterExchange("exchangename")
						.WithDeadLetterRoutingKey("routingkey")
						.WithMessageTimeToLive(10_000)
						.WithTimeToLive(1_000_000)
						.WithMaxPriority(12)
						.WithRejectPublishOnOverflow());
		}
		
		[Fact]
		public void QueuesRegistered()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMQ("instance")
				.AddQueues(options =>
					options.AddQueue("queuename")
						.Durable()
						.Exclusive()
						.AutoDelete()
						.WithArgument("x-argument", "somevalue"));

			var provider = services.BuildServiceProvider();

			var queues = provider.GetRequiredService<IOptions<RabbitMQQueuesOptions>>().Value;

			var queue = Assert.Single(queues.Queues);

			Assert.Equal("queuename", queue.Name);
			Assert.True(queue.Durable);
			Assert.True(queue.Exclusive);
			Assert.True(queue.AutoDelete);

			var (arg, value) = Assert.Single(queue.Arguments);

			Assert.Equal("x-argument", arg);
			Assert.Equal("somevalue", value);
		}

		[Fact]
		public void QueuesRegisteredByDefault()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMQ("instance")
				.AddQueues(options => options.AddQueue("queuename"));

			var provider = services.BuildServiceProvider();

			var queues = provider.GetRequiredService<IOptions<RabbitMQQueuesOptions>>().Value;

			var queue = Assert.Single(queues.Queues);

			Assert.Equal("queuename", queue.Name);
			Assert.False(queue.Durable);
			Assert.False(queue.Exclusive);
			Assert.False(queue.AutoDelete);

			Assert.Empty(queue.Arguments);
		}
	}
}