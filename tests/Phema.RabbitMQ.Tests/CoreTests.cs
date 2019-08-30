using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Phema.RabbitMQ.Tests
{
	public class CoreTests
	{
		[Fact]
		public void Defaults()
		{
			var services = new ServiceCollection();

			services.AddRabbitMQ();

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value;

			Assert.True(options.ConnectionFactory.DispatchConsumersAsync);
		}

		[Fact]
		public void Specified()
		{
			var services = new ServiceCollection();

			services.AddRabbitMQ(o =>
			{
				o.InstanceName = "test";
				o.ConnectionFactory.HostName = "test.test";
				o.ConnectionFactory.UserName = "test";
				o.ConnectionFactory.Password = "password";
			});

			var provider = services.BuildServiceProvider();

			var options = provider.GetRequiredService<IOptions<RabbitMQOptions>>().Value;

			Assert.Equal("test", options.InstanceName);
			Assert.Equal("test.test", options.ConnectionFactory.HostName);
			Assert.Equal("test", options.ConnectionFactory.UserName);
			Assert.Equal("password", options.ConnectionFactory.Password);
		}
	}
}