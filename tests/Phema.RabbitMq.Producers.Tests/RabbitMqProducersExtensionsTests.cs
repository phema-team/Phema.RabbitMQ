using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Phema.RabbitMQ.Producers.Tests
{
	public class TestPayload
	{
	}

	public class RabbitMQProducersExtensionsTests
	{
		[Fact]
		public void ProducersRegistered()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMQ("instance")
				.AddProducerGroup(options =>
					options.AddProducer<TestPayload>("exchangename", "queuename")
						.Mandatory()
						.WithProperty(p => p.Persistent = true));

			Assert.Single(services.Where(s => s.ServiceType == typeof(IRabbitMQProducer<TestPayload>)));
		}

		[Fact]
		public void UXTest()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMQ("instance")
				.AddProducerGroup(options =>
					options.AddProducer<TestPayload>("exchangename", "queuename")
						.Mandatory()
						.Transactional()
						.WaitForConfirms()
						.Persistent()
						.WithPriority(10)
						.WithRoutingKey("routingkey")
						.WithArgument("argument", "value")
						.WithMessageTimeToLive(2_000)
						.WithHeader("header", "value")
						.WithProperty(p => p.Persistent = true));
		}
	}
}