using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using Xunit;

namespace Phema.RabbitMq.Consumers.Tests
{
	public class TestPayload
	{
	}

	public class TestPayloadConsumer : IRabbitMqConsumer<TestPayload>
	{
		public async ValueTask Consume(TestPayload payload)
		{
		}
	}

	public class RabbitMqConsumersExtensionsTests
	{
		[Fact]
		public void ConsumersRegistered()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMq("instance")
				.AddConsumers(options =>
					options.AddConsumer<TestPayload, TestPayloadConsumer>("queuename")
						.WithTag("consumertag")
						.WithPrefetch(0)
						.WithConsumers(1)
						.Exclusive()
						.NoLocal()
						.AutoAck()
						.Requeue(true)
						.WithArgument("x-argument", "value"));

			var provider = services.BuildServiceProvider();

			var consumers = provider.GetRequiredService<IOptions<RabbitMqConsumersOptions>>().Value;

			// var consumer = Assert.Single(consumers.Consumers);
			//
			// Assert.Equal("queuename", consumer.QueueName);
			// Assert.Equal("consumertag", consumer.Tag);
			// Assert.Equal(0, consumer.Prefetch);
			// Assert.Equal(1, consumer.Consumers);
			//
			// Assert.True(consumer.Exclusive);
			// Assert.True(consumer.NoLocal);
			// Assert.True(consumer.AutoAck);
			// Assert.True(consumer.Requeue);
			// Assert.True(consumer.Multiple);
			//
			// var (key, value) = Assert.Single(consumer.Arguments);
			//
			// Assert.Equal("x-argument", key);
			// Assert.Equal("value", value);
		}

		[Fact]
		public void ConsumersRegisteredByDefault()
		{
			var services = new ServiceCollection();

			services.AddPhemaRabbitMq("instance")
				.AddConsumers(options =>
					options.AddConsumer<TestPayload, TestPayloadConsumer>("queuename"));

			var provider = services.BuildServiceProvider();

			var consumers = provider.GetRequiredService<IOptions<RabbitMqConsumersOptions>>().Value;

			// var consumer = Assert.Single(consumers.Consumers);
			//
			// Assert.Equal("queuename", consumer.QueueName);
			// Assert.Null(consumer.Tag);
			// Assert.Equal(0, consumer.Prefetch);
			// Assert.Equal(1, consumer.Consumers);
			//
			// Assert.False(consumer.Exclusive);
			// Assert.False(consumer.NoLocal);
			// Assert.False(consumer.AutoAck);
			// Assert.False(consumer.Requeue);
			// Assert.False(consumer.Multiple);
			//
			// Assert.Empty(consumer.Arguments);
		}
	}
}