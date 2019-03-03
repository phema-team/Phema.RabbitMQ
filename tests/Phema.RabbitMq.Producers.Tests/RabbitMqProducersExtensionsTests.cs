using System.Linq;
using Microsoft.Extensions.DependencyInjection;

using Xunit;

namespace Phema.RabbitMq.Producers.Tests
{
	public class TestPayload
	{
	}

	public class RabbitMqProducersExtensionsTests
	{
		[Fact]
		public void ProducersRegistered()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMq("instance")
				.AddProducers(options =>
					options.AddProducer<TestPayload>("exchangename", "queuename")
						.Mandatory()
						.WithProperties(p => p.Persistent = true));

			Assert.Single(services.Where(s => s.ServiceType == typeof(IRabbitMqProducer<TestPayload>)));
		}
	}
}