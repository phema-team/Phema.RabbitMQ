using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Phema.RabbitMQ.Internal;
using Xunit;

namespace Phema.RabbitMQ.Tests
{
	public class ConsumersTests
	{
		private class AsyncConsumersTestsAsyncConsumer : IRabbitMQAsyncConsumer<ConsumersTests>
		{
			public Task Consume(ConsumersTests payload)
			{
				throw new System.NotImplementedException();
			}
		}

		[Fact]
		public void Default()
		{
			var services = new ServiceCollection();

			services.AddRabbitMQ("test", "amqp://test.test")
				.AddConsumerGroup(group =>
					group.AddAsyncConsumer<ConsumersTests, AsyncConsumersTestsAsyncConsumer>("queue"));

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<RabbitMQConsumersOptions>>().Value;

			var declaration = Assert.Single(options.Declarations);
			
			Assert.Empty(declaration.Arguments);
			Assert.False(declaration.AutoAck);
			Assert.Equal(1u, declaration.Count);
			Assert.False(declaration.Exclusive);
			Assert.False(declaration.Global);
			Assert.Equal(RabbitMQDefaults.DefaultGroupName, declaration.GroupName);
			Assert.False(declaration.Multiple);
			Assert.False(declaration.NoLocal);
			Assert.Equal(0, declaration.PrefetchCount);
			Assert.Equal("queue", declaration.QueueName);
			Assert.False(declaration.Requeue);
			Assert.Null(declaration.Tag);
		}
		
		[Fact]
		public void Specified()
		{
			var services = new ServiceCollection();

			services.AddRabbitMQ("test", "amqp://test.test")
				.AddConsumerGroup("consumers", group =>
					group.AddAsyncConsumer<ConsumersTests, AsyncConsumersTestsAsyncConsumer>("queue")
						.WithArgument("x-argument", "argument")
						.AutoAck()
						.Count(2)
						.Exclusive()
						.Requeue(true)
						.NoLocal()
						.Prefetch(2)
						.Tagged("tag"));

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<RabbitMQConsumersOptions>>().Value;

			var declaration = Assert.Single(options.Declarations);
			
			var (key, value) = Assert.Single(declaration.Arguments);
			Assert.Equal("x-argument", key);
			Assert.Equal("argument", value);
			
			Assert.True(declaration.AutoAck);
			Assert.Equal(2u, declaration.Count);
			Assert.True(declaration.Exclusive);
			Assert.True(declaration.Global);
			Assert.Equal("consumers", declaration.GroupName);
			Assert.True(declaration.Multiple);
			Assert.True(declaration.NoLocal);
			Assert.Equal(2, declaration.PrefetchCount);
			Assert.Equal("queue", declaration.QueueName);
			Assert.True(declaration.Requeue);
			Assert.Equal("tag", declaration.Tag);
		}
	}
}