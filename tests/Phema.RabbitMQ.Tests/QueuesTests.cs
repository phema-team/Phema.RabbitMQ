using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
				.AddConnection("connection", group => group.AddQueue<string>("queue"));

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value;

			var declaration = Assert.Single(options.QueueDeclarations);

			Assert.Empty(declaration.Arguments);
			Assert.False(declaration.AutoDelete);
			Assert.False(declaration.Deleted);
			Assert.False(declaration.Durable);
			Assert.False(declaration.Exclusive);
			Assert.Equal("connection", declaration.Connection.Name);
			Assert.False(declaration.IfEmpty);
			Assert.False(declaration.IfUnused);
			Assert.False(declaration.NoWait);
			Assert.Empty(declaration.Bindings);
			Assert.Equal("queue", declaration.Name);
		}

		[Fact]
		public void Specified()
		{
			var services = new ServiceCollection();

			services.AddRabbitMQ("test", "amqp://test.test")
				.AddConnection("group", connection => connection.AddQueue<string>("queue")
					.Argument("x-argument", "argument")
					.AutoDelete()
					.Deleted(true, true)
					.Durable()
					.Exclusive()
					.NoWait()
					.BoundTo(connection.AddDirectExchange("exchange"), b =>
						b.RoutedTo("routing_key")
							.NoWait()
							.Deleted()
							.Argument("x-argument", "argument")));

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value;

			var declaration = Assert.Single(options.QueueDeclarations);

			var (key, value) = Assert.Single(declaration.Arguments);
			Assert.Equal("x-argument", key);
			Assert.Equal("argument", value);

			Assert.True(declaration.AutoDelete);
			Assert.True(declaration.Deleted);
			Assert.True(declaration.Durable);
			Assert.True(declaration.Exclusive);
			Assert.Equal("group", declaration.Connection.Name);
			Assert.True(declaration.IfEmpty);
			Assert.True(declaration.IfUnused);
			Assert.True(declaration.NoWait);

			var binding = Assert.Single(declaration.Bindings);
			Assert.Equal("exchange", binding.Exchange.Name);
			Assert.Equal("routing_key", binding.RoutingKey);
			Assert.True(binding.Deleted);
			Assert.True(binding.NoWait);
			(key, value) = Assert.Single(binding.Arguments);
			Assert.Equal("x-argument", key);
			Assert.Equal("argument", value);

			Assert.Equal("queue", declaration.Name);
		}
	}
}