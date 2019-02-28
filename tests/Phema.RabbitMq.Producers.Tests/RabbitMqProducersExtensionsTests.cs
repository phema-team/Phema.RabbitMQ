using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

			var provider = services.BuildServiceProvider();

			var producers = provider.GetRequiredService<IOptions<RabbitMqProducersOptions>>().Value;

			var producer = Assert.Single(producers.Producers);
			
			Assert.Equal("exchangename", producer.ExchangeName);
			Assert.Equal("queuename", producer.QueueName);
			Assert.True(producer.Mandatory);
			Assert.Single(producer.Properties);
		}
		
		[Fact]
		public void ProducersRegisteredByDefault()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMq("instance")
				.AddProducers(options =>
					options.AddProducer<TestPayload>("exchangename", "queuename"));

			var provider = services.BuildServiceProvider();

			var producers = provider.GetRequiredService<IOptions<RabbitMqProducersOptions>>().Value;

			var producer = Assert.Single(producers.Producers);
			
			Assert.Equal("exchangename", producer.ExchangeName);
			Assert.Equal("queuename", producer.QueueName);
			Assert.False(producer.Mandatory);
			Assert.Empty(producer.Properties);
		}
	}
}