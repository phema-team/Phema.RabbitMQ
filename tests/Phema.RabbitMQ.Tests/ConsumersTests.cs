using System;
using System.Threading.Tasks;
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

			services.AddRabbitMQ("test", "amqp://test.test")
				.AddConnection("connection", connection =>
					connection.AddConsumer(connection.AddQueue<ConsumersTests>("queue"), (scope, s) => new ValueTask()));

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value;

			var declaration = Assert.Single(options.ConsumerDeclarations);
			
			Assert.Empty(declaration.Arguments);
			Assert.False(declaration.AutoAck);
			Assert.Equal(1u, declaration.Count);
			Assert.False(declaration.Exclusive);
			Assert.False(declaration.Global);
			Assert.False(declaration.Multiple);
			Assert.False(declaration.NoLocal);
			Assert.Equal(0, declaration.PrefetchCount);
			Assert.Equal("queue", declaration.Queue.Name);
			Assert.Equal("connection", declaration.Connection.Name);
			Assert.False(declaration.Requeue);
			Assert.True(Guid.TryParse(declaration.Tag, out _));
		}
		
		[Fact]
		public void Specified()
		{
			var services = new ServiceCollection();

			services.AddRabbitMQ("test", "amqp://test.test")
				.AddConnection("consumers", connection =>
					connection.AddConsumer(connection.AddQueue<ConsumersTests>("queue"), (scope, tests) => new ValueTask())
						.Argument("x-argument", "argument")
						.AutoAck()
						.Count(2)
						.Exclusive()
						.Requeue(true)
						.NoLocal()
						.Prefetch(2)
						.Tagged("tag"));

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value;

			var declaration = Assert.Single(options.ConsumerDeclarations);
			
			var (key, value) = Assert.Single(declaration.Arguments);
			Assert.Equal("x-argument", key);
			Assert.Equal("argument", value);
			
			Assert.True(declaration.AutoAck);
			Assert.Equal(2u, declaration.Count);
			Assert.True(declaration.Exclusive);
			Assert.False(declaration.Global);
			Assert.Equal("consumers", declaration.Connection.Name);
			Assert.True(declaration.Multiple);
			Assert.True(declaration.NoLocal);
			Assert.Equal(2, declaration.PrefetchCount);
			Assert.Equal("queue", declaration.Queue.Name);
			Assert.True(declaration.Requeue);
			Assert.Equal("tag", declaration.Tag);
		}
	}
}