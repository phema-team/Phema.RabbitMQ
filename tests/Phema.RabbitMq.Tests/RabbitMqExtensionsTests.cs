using System.Linq;

using Microsoft.Extensions.DependencyInjection;

using Phema.RabbitMQ;

using RabbitMQ.Client;

using Xunit;

namespace TestProject1
{
	public class RabbitMQExtensionsTests
	{
		[Fact]
		public void ByDefaultConnectionFactoryDispatchAsyncConsumers()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMQ("instance", options => Assert.True(options.DispatchConsumersAsync));
		}

		[Fact]
		public void ConnectionFactoryAndScopedChannelRegistered()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMQ("instance", options => options.HostName = "hostname");

			var connection = Assert.Single(services.Where(s => s.ServiceType == typeof(IConnection)));
			Assert.Equal(ServiceLifetime.Singleton, connection.Lifetime);
		}
	}
}