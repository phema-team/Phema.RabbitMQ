using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Phema.RabbitMQ.Internal;
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

			services.AddRabbitMQ("test", "amqp://test.test")
				.AddExchangeGroup(group =>
					group.AddDirectExchange("exchange"));

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<RabbitMQExchangesOptions>>().Value;

			var declaration = Assert.Single(options.Declarations);

			Assert.Empty(declaration.Arguments);
			Assert.False(declaration.AutoDelete);
			Assert.False(declaration.Deleted);
			Assert.False(declaration.Durable);
			Assert.Empty(declaration.ExchangeBindings);
			Assert.Equal("exchange", declaration.ExchangeName);
			Assert.Equal(ExchangeType.Direct, declaration.ExchangeType);
			Assert.Equal(RabbitMQDefaults.DefaultGroupName, declaration.GroupName);
			Assert.False(declaration.IfUnused);
			Assert.False(declaration.Internal);
			Assert.False(declaration.NoWait);
		}

		[Fact]
		public void Specified()
		{
			var services = new ServiceCollection();

			services.AddRabbitMQ("test", "amqp://test.test")
				.AddExchangeGroup("exchanges", group =>
					group.AddTopicExchange("exchange")
						.WithArgument("x-argument", "argument")
						.AutoDelete()
						.Deleted(true)
						.Durable()
						.BoundTo("exchange2", b =>
							b.RoutingKeys("routing_key")
								.Deleted()
								.NoWait()
								.WithArgument("x-argument", "argument"))
						.Internal()
						.NoWait());

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<RabbitMQExchangesOptions>>().Value;

			var declaration = Assert.Single(options.Declarations);

			var (key, value) = Assert.Single(declaration.Arguments);
			Assert.Equal("x-argument", key);
			Assert.Equal("argument", value);

			Assert.True(declaration.AutoDelete);
			Assert.True(declaration.Deleted);
			Assert.True(declaration.Durable);
			var binding = Assert.Single(declaration.ExchangeBindings);
			Assert.Equal("routing_key", binding.RoutingKeys.Single());
			Assert.Equal("exchange2", binding.ExchangeName);
			Assert.True(binding.Deleted);
			Assert.True(binding.NoWait);

			(key, value) = Assert.Single(binding.Arguments);
			Assert.Equal("x-argument", key);
			Assert.Equal("argument", value);

			Assert.Equal("exchange", declaration.ExchangeName);
			Assert.Equal(ExchangeType.Topic, declaration.ExchangeType);
			Assert.Equal("exchanges", declaration.GroupName);
			Assert.True(declaration.IfUnused);
			Assert.True(declaration.Internal);
			Assert.True(declaration.NoWait);
		}
	}
}