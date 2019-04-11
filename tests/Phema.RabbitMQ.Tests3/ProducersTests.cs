using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Phema.RabbitMQ.Internal;
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
				.AddProducerGroup(group =>
					group.AddProducer<ProducersTests>("exchange"));

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<RabbitMQProducersOptions>>().Value;

			var declaration = Assert.Single(options.Declarations);

			Assert.Empty(declaration.Arguments);
			Assert.False(declaration.Die);
			Assert.Equal("exchange", declaration.ExchangeName);
			Assert.Equal(RabbitMQDefaults.DefaultGroupName, declaration.GroupName);
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
				.AddProducerGroup("exchanges", group =>
					group.AddProducer<ProducersTests>("exchange")
						.WithArgument("x-argument", "argument")
						.WaitForConfirms(TimeSpan.Zero)
						.Mandatory()
						.WithProperty(x => x.Persistent = true)
						.RoutingKey("routing_key")
						.AppId("app1")
						.Transactional());

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<RabbitMQProducersOptions>>().Value;

			var declaration = Assert.Single(options.Declarations);

			var (key, value) = Assert.Single(declaration.Arguments);
			Assert.Equal("x-argument", key);
			Assert.Equal("argument", value);

			Assert.True(declaration.Die);
			Assert.Equal("exchange", declaration.ExchangeName);
			Assert.Equal("exchanges", declaration.GroupName);
			Assert.True(declaration.Mandatory);
			Assert.Single(declaration.Properties);
			Assert.Equal("routing_key", declaration.RoutingKey);
			Assert.Equal(TimeSpan.Zero, declaration.Timeout);
			Assert.True(declaration.Transactional);
			Assert.True(declaration.WaitForConfirms);
		}
	}
}