using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using Phema.RabbitMq;

using RabbitMQ.Client;

using Xunit;

namespace TestProject1
{
	public class RabbitMqExtensionsTests
	{
		[Fact]
		public void ByDefaultConnectionFactoryDispatchAsyncConsumers()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMq("instance", options => Assert.True(options.DispatchConsumersAsync));
		}

		[Fact]
		public void ConnectionFactoryAndScopedChannelRegistered()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMq("instance", options => options.HostName = "hostname");

			var connection = Assert.Single(services.Where(s => s.ServiceType == typeof(IConnection)));
			Assert.Equal(ServiceLifetime.Singleton, connection.Lifetime);

			var channel = Assert.Single(services.Where(s => s.ServiceType == typeof(IModel)));
			Assert.Equal(ServiceLifetime.Scoped, channel.Lifetime);
		}
	}
}