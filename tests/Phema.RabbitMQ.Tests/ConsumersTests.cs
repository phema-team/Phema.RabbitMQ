using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Phema.RabbitMQ.Tests
{
	public class ConsumersTests
	{
		[Fact]
		public void Default()
		{
			var services = new ServiceCollection();

			// "test", 
			services.AddRabbitMQ(options => options
					.UseConnectionUrl("amqp://test.test")
					.UseClientProvidedName("test"))
				.AddConnection("connection", connection =>
					connection.AddConsumer(connection.AddQueue<ConsumersTests>("queue")));

			var provider = services.BuildServiceProvider();

			var declarations = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value.ConsumerDeclarations;

			var declaration = Assert.Single(declarations);

			Assert.Empty(declaration.Arguments);
			Assert.False(declaration.AutoAck);
			Assert.Equal(1u, declaration.Count);
			Assert.False(declaration.Exclusive);
			Assert.False(declaration.Global);
			Assert.False(declaration.Multiple);
			Assert.False(declaration.NoLocal);
			Assert.Equal(0, declaration.PrefetchCount);
			Assert.Equal("queue", Assert.Single(declaration.QueueDeclarations).Name);
			Assert.Equal("connection", declaration.ConnectionDeclaration.Name);
			Assert.False(declaration.Requeue);
			Assert.Null(declaration.Tag);
		}

		[Fact]
		public void Specified()
		{
			var services = new ServiceCollection();

			services.AddRabbitMQ(options => options
					.UseConnectionUrl("amqp://test.test")
					.UseClientProvidedName("test"))
				.AddConnection("consumers", connection =>
					connection.AddConsumer(connection.AddQueue<ConsumersTests>("queue"))
						.Argument("x-argument", "argument")
						.AutoAck()
						.Count(2)
						.Exclusive()
						.Requeue(true)
						.NoLocal()
						.Prefetch(2)
						.Tagged("tag"));

			var provider = services.BuildServiceProvider();

			var declarations = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value.ConsumerDeclarations;

			var declaration = Assert.Single(declarations);

			var (key, value) = Assert.Single(declaration.Arguments);
			Assert.Equal("x-argument", key);
			Assert.Equal("argument", value);

			Assert.True(declaration.AutoAck);
			Assert.Equal(2u, declaration.Count);
			Assert.True(declaration.Exclusive);
			Assert.False(declaration.Global);
			Assert.Equal("consumers", declaration.ConnectionDeclaration.Name);
			Assert.True(declaration.Multiple);
			Assert.True(declaration.NoLocal);
			Assert.Equal(2, declaration.PrefetchCount);
			Assert.Equal("queue", Assert.Single(declaration.QueueDeclarations).Name);
			Assert.True(declaration.Requeue);
			Assert.Equal("tag", declaration.Tag);
		}
	}
}