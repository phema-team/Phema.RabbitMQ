using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Xunit;

namespace Phema.RabbitMQ.Tests
{
	public class ExchangesTests
	{
		[Fact]
		public void Default()
		{
			var services = new ServiceCollection();

			services.AddRabbitMQ(options => options
					.UseConnectionUrl("amqp://test.test")
					.UseClientProvidedName("test"))
				.AddConnection("connection", connection =>
					connection.AddDirectExchange("exchange"));

			var provider = services.BuildServiceProvider();

			var declarations = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value.ExchangeDeclarations;

			var declaration = Assert.Single(declarations);

			Assert.Empty(declaration.Arguments);
			Assert.False(declaration.AutoDelete);
			Assert.False(declaration.Deleted);
			Assert.False(declaration.Durable);
			Assert.Empty(declaration.Bindings);
			Assert.Equal("exchange", declaration.Name);
			Assert.Equal(ExchangeType.Direct, declaration.Type);
			Assert.Equal("connection", declaration.Connection.Name);
			Assert.False(declaration.UnusedOnly);
			Assert.False(declaration.Internal);
			Assert.False(declaration.NoWait);
		}

		[Fact]
		public void Specified()
		{
			var services = new ServiceCollection();

			services.AddRabbitMQ(options => options
					.UseConnectionUrl("amqp://test.test")
					.UseClientProvidedName("test"))
				.AddConnection("exchanges", connection =>
					connection.AddTopicExchange("exchange")
						.Argument("x-argument", "argument")
						.AutoDelete()
						.Deleted(true)
						.Durable()
						.BoundTo(connection.AddDirectExchange("exchange2"), b =>
							b.RoutedTo("routing_key")
								.Deleted()
								.NoWait()
								.Argument("x-argument", "argument"))
						.Internal()
						.NoWait());

			var provider = services.BuildServiceProvider();

			var declarations = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value.ExchangeDeclarations;

			var declaration = declarations.First();

			var (key, value) = Assert.Single(declaration.Arguments);
			Assert.Equal("x-argument", key);
			Assert.Equal("argument", value);

			Assert.True(declaration.AutoDelete);
			Assert.True(declaration.Deleted);
			Assert.True(declaration.Durable);
			var binding = Assert.Single(declaration.Bindings);
			Assert.Equal("routing_key", binding.RoutingKey);
			Assert.Equal("exchange2", binding.Exchange.Name);
			Assert.True(binding.Deleted);
			Assert.True(binding.NoWait);

			(key, value) = Assert.Single(binding.Arguments);
			Assert.Equal("x-argument", key);
			Assert.Equal("argument", value);

			Assert.Equal("exchange", declaration.Name);
			Assert.Equal(ExchangeType.Topic, declaration.Type);
			Assert.Equal("exchanges", declaration.Connection.Name);
			Assert.True(declaration.UnusedOnly);
			Assert.True(declaration.Internal);
			Assert.True(declaration.NoWait);
		}
	}
}