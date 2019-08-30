using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Phema.RabbitMQ.Tests
{
	public class RabbitMQExchangeTests
	{
		[Fact]
		public void SimpleExchage()
		{
			var services = new ServiceCollection();
				
			services.AddRabbitMQ(o => {})
				.AddConnection("connection", connection =>
				{
					connection.AddDirectExchange("exchange")
						.Deleted()
						.Durable()
						.Internal()
						.AutoDelete()
						.NoWait()
						.Argument("key", "value");
				});

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value;

			var declaration = Assert.Single(options.ExchangeDeclarations);

			Assert.Equal("exchange", declaration.Name);
			Assert.Equal("direct", declaration.Type);
			Assert.True(declaration.Deleted);
			Assert.True(declaration.Durable);
			Assert.True(declaration.Internal);
			Assert.True(declaration.AutoDelete);
			Assert.True(declaration.NoWait);

			var (key, value) = Assert.Single(declaration.Arguments);

			Assert.Equal("key", key);
			Assert.Equal("value", value);
		}
	}
}