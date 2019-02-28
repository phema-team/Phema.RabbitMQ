using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Xunit;

namespace Phema.RabbitMq.Queues.Tests
{
	public class RabbitMqQueueExtensionsTests
	{
		[Fact]
		public void QueuesRegistered()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMq("instance")
				.AddQueues(options =>
					options.AddQueue("queuename")
						.Durable()
						.Exclusive()
						.AutoDelete()
						.WithArgument("x-argument", "somevalue"));

			var provider = services.BuildServiceProvider();

			var queues = provider.GetRequiredService<IOptions<RabbitMqQueuesOptions>>().Value;

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

			services.AddPhemaRabbitMq("instance")
				.AddQueues(options => options.AddQueue("queuename"));

			var provider = services.BuildServiceProvider();

			var queues = provider.GetRequiredService<IOptions<RabbitMqQueuesOptions>>().Value;

			var queue = Assert.Single(queues.Queues);
			
			Assert.Equal("queuename", queue.Name);
			Assert.False(queue.Durable);
			Assert.False(queue.Exclusive);
			Assert.False(queue.AutoDelete);

			Assert.Empty(queue.Arguments);
		}
	}
}