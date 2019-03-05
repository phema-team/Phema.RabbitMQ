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
				.AddProducers(options =>
					options.AddProducer<TestPayload>("exchangename", "queuename")
						.Mandatory()
						.WithProperties(p => p.Persistent = true));

			Assert.Single(services.Where(s => s.ServiceType == typeof(IRabbitMQProducer<TestPayload>)));
		}
	}
}