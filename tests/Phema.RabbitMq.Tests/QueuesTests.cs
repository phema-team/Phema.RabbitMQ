using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Phema.RabbitMQ.Internal;
using Xunit;

namespace Phema.RabbitMQ.Tests
{
	public class QueuesTests
	{
		[Fact]
		public void Default()
		{
			var services = new ServiceCollection();

			services.AddRabbitMQ("test", "amqp://test.test")
				.AddQueueGroup(group => group.AddQueue("queue"));

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<RabbitMQQueuesOptions>>().Value;

			var declaration = Assert.Single(options.Declarations);
			
			Assert.Empty(declaration.Arguments);
			Assert.False(declaration.AutoDelete);
			Assert.False(declaration.Deleted);
			Assert.False(declaration.Durable);
			Assert.False(declaration.Exclusive);
			Assert.Equal(RabbitMQDefaults.DefaultGroupName, declaration.GroupName);
			Assert.False(declaration.IfEmpty);
			Assert.False(declaration.IfUnused);
			Assert.False(declaration.NoWait);
			Assert.Empty(declaration.QueueBindings);
			Assert.Equal("queue", declaration.QueueName);
		}
		
		[Fact]
		public void Specified()
		{
			var services = new ServiceCollection();

			services.AddRabbitMQ("test", "amqp://test.test")
				.AddQueueGroup("group", group => group.AddQueue("queue")
					.WithArgument("x-argument", "argument")
					.AutoDelete()
					.Deleted(true, true)
					.Durable()
					.Exclusive()
					.NoWait()
					.BoundTo("exchange", b =>
						b.RoutingKey("routing_key")
							.NoWait()
							.Deleted()
							.WithArgument("x-argument", "argument")));

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<RabbitMQQueuesOptions>>().Value;

			var declaration = Assert.Single(options.Declarations);
			
			var (key, value) = Assert.Single(declaration.Arguments);
			Assert.Equal("x-argument", key);
			Assert.Equal("argument", value);

			Assert.True(declaration.AutoDelete);
			Assert.True(declaration.Deleted);
			Assert.True(declaration.Durable);
			Assert.True(declaration.Exclusive);
			Assert.Equal("group", declaration.GroupName);
			Assert.True(declaration.IfEmpty);
			Assert.True(declaration.IfUnused);
			Assert.True(declaration.NoWait);
			
			var binding = Assert.Single(declaration.QueueBindings);
			Assert.Equal("exchange", binding.ExchangeName);
			Assert.Equal("routing_key", binding.RoutingKey);
			Assert.True(binding.Deleted);
			Assert.True(binding.NoWait);
			(key, value) = Assert.Single(binding.Arguments);
			Assert.Equal("x-argument", key);
			Assert.Equal("argument", value);
			
			Assert.Equal("queue", declaration.QueueName);
		}
	}
}