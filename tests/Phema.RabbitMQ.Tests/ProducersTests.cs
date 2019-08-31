using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Phema.RabbitMQ.Tests
{
	public class ProducersTests
	{
		[Fact]
		public void Default()
		{
			var services = new ServiceCollection();

			services.AddRabbitMQ("test", "amqp://test.test")
				.AddConnection("connection", connection =>
					connection.AddProducer<ProducersTests>(connection.AddDirectExchange("exchange")));

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value;

			var declaration = Assert.Single(options.ProducerDeclarations);

			Assert.Empty(declaration.Arguments);
			Assert.False(declaration.Die);
			Assert.Equal("exchange", declaration.Exchange.Name);
			Assert.Equal("connection", declaration.Connection.Name);
			Assert.False(declaration.Mandatory);
			Assert.Empty(declaration.Properties);
			Assert.Null(declaration.RoutingKey);
			Assert.Null(declaration.Timeout);
			Assert.False(declaration.Transactional);
			Assert.False(declaration.WaitForConfirms);
		}

		[Fact]
		public void Specified()
		{
			var services = new ServiceCollection();

			services.AddRabbitMQ("test", "amqp://test.test")
				.AddConnection("exchanges", connection =>
					connection.AddProducer<ProducersTests>(connection.AddDirectExchange("exchange"))
						.Argument("x-argument", "argument")
						.WaitForConfirms(TimeSpan.Zero)
						.Mandatory()
						.Property(x => x.Persistent = true)
						.RoutedTo("routing_key")
						.AppId("app1")
						.Transactional());

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value;

			var declaration = Assert.Single(options.ProducerDeclarations);

			var (key, value) = Assert.Single(declaration.Arguments);
			Assert.Equal("x-argument", key);
			Assert.Equal("argument", value);

			Assert.True(declaration.Die);
			Assert.Equal("exchange", declaration.Exchange.Name);
			Assert.Equal("exchanges", declaration.Connection.Name);
			Assert.True(declaration.Mandatory);
			Assert.Equal(2, declaration.Properties.Count);
			Assert.Equal("routing_key", declaration.RoutingKey);
			Assert.Equal(TimeSpan.Zero, declaration.Timeout);
			Assert.True(declaration.Transactional);
			Assert.True(declaration.WaitForConfirms);
		}
	}
}