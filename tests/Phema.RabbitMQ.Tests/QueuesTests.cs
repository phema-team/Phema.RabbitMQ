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

			services.AddRabbitMQ(options => options
					.UseConnectionUrl("amqp://test.test")
					.UseClientProvidedName("test"))
				.AddConnection("connection", group => group.AddQueue<string>("queue"));

			var provider = services.BuildServiceProvider();

			var declarations = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value.QueueDeclarations;

			var declaration = Assert.Single(declarations);

			Assert.Empty(declaration.Arguments);
			Assert.False(declaration.AutoDelete);
			Assert.False(declaration.Deleted);
			Assert.False(declaration.Durable);
			Assert.False(declaration.Exclusive);
			Assert.Equal("connection", declaration.ConnectionDeclaration.Name);
			Assert.False(declaration.EmptyOnly);
			Assert.False(declaration.UnusedOnly);
			Assert.False(declaration.NoWait);
			Assert.Empty(declaration.BindingDeclarations);
			Assert.Equal("queue", declaration.Name);
		}

		[Fact]
		public void Specified()
		{
			var services = new ServiceCollection();

			services.AddRabbitMQ(options => options
					.UseConnectionUrl("amqp://test.test")
					.UseClientProvidedName("test"))
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

			var declarations = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value.QueueDeclarations;

			var declaration = Assert.Single(declarations);

			var (key, value) = Assert.Single(declaration.Arguments);
			Assert.Equal("x-argument", key);
			Assert.Equal("argument", value);

			Assert.True(declaration.AutoDelete);
			Assert.True(declaration.Deleted);
			Assert.True(declaration.Durable);
			Assert.True(declaration.Exclusive);
			Assert.Equal("group", declaration.ConnectionDeclaration.Name);
			Assert.True(declaration.EmptyOnly);
			Assert.True(declaration.UnusedOnly);
			Assert.True(declaration.NoWait);

			var binding = Assert.Single(declaration.BindingDeclarations);
			Assert.Equal("exchange", binding.ExchangeDeclaration.Name);
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