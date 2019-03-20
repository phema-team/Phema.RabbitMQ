using System;
using Microsoft.Extensions.DependencyInjection;
using Phema.RabbitMQ;
using RabbitMQ.Client;
using Xunit;

namespace TestProject1
{
	public class RabbitMQExtensionsTests
	{
		[Fact]
		public void ConnectionFactoryAndScopedChannelRegistered()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMQ("instance", "amqp://user:pass@host:10000/vhost");

			services.AddPhemaRabbitMQ("instance", factory =>
			{
				factory.Uri = new Uri("amqp://user:pass@host:10000/vhost");

				var connectionFactory = (ConnectionFactory) factory;

				Assert.Equal("user", connectionFactory.UserName);
				Assert.Equal("pass", connectionFactory.Password);
				Assert.Equal("host", connectionFactory.HostName);
				Assert.Equal(10000, connectionFactory.Port);
				Assert.Equal("vhost", connectionFactory.VirtualHost);
			});
		}
	}
}