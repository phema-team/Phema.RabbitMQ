using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;

using Xunit;

namespace Phema.RabbitMQ.Exchanges.Tests
{
	public class RabbitMQExchangesExtensionsTests
	{
		[Fact]
		public void UXTest()
		{
			var services = new ServiceCollection();
			
			services.AddPhemaRabbitMQ("instance")
				.AddExchanges(options =>
					options.AddDirectExchange("amq.direct")
						.Durable()
						.AutoDelete()
						.WithArgument("x-argument", "value")
						.WithBoundExchange("exchangename", exchange =>
							exchange.WithRoutingKey("routingkey")
								.WithArgument("argument", "value"))
						.WithAlternateExchange("alternameexchangename"));
		}
		
		[Fact]
		public void ExchangesRegistered()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMQ("instance")
				.AddExchanges(options =>
					options.AddDirectExchange("amq.direct")
						.Durable()
						.AutoDelete()
						.WithArgument("x-argument", "value"));

			var provider = services.BuildServiceProvider();

			var exchanges = provider.GetRequiredService<IOptions<RabbitMQExchangesOptions>>().Value;

			var exchange = Assert.Single(exchanges.Exchanges);

			Assert.Equal("amq.direct", exchange.Name);
			Assert.Equal(ExchangeType.Direct, exchange.Type);
			Assert.True(exchange.Durable);
			Assert.True(exchange.AutoDelete);

			var (key, value) = Assert.Single(exchange.Arguments);

			Assert.Equal("x-argument", key);
			Assert.Equal("value", value);
		}

		[Fact]
		public void ExchangesRegisteredByDefault()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMQ("instance")
				.AddExchanges(options => options.AddFanoutExchange("amq.direct"));

			var provider = services.BuildServiceProvider();

			var exchanges = provider.GetRequiredService<IOptions<RabbitMQExchangesOptions>>().Value;

			var exchange = Assert.Single(exchanges.Exchanges);

			Assert.Equal("amq.direct", exchange.Name);
			Assert.Equal(ExchangeType.Fanout, exchange.Type);
			Assert.False(exchange.Durable);
			Assert.False(exchange.AutoDelete);

			Assert.Empty(exchange.Arguments);
		}
	}
}