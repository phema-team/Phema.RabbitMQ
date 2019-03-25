using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ.Tests
{
	public class Payload
	{
		public string Name { get; set; }
	}

	public class PayloadConsumer : IRabbitMQConsumer<Payload>
	{
		public Task Consume(Payload payload)
		{
			return Console.Out.WriteLineAsync(payload.Name);
		}
	}

	public class UnitTest1
	{
		[Fact]
		public void Usability()
		{
			var services = new ServiceCollection();

			// TODO: Purged for queue; Redeclared for queue and exchange?

			services.AddRabbitMQ("test", "amqp://test.test")
				.AddConsumerGroup("consumers", group =>
					group.AddConsumer<Payload, PayloadConsumer>("queue")
						.WithTag("tag")
						.Prefetched(1)
						.WithCount(1)
						.Exclusive()
						.NoLocal()
						.AutoAck()
						.Requeue()
						.Priority(2)
						.WithArgument("x-argument", "argument"))
				.AddQueueGroup("queues", group =>
					group.AddQueue("queue")
						.Durable()
						.Exclusive()
						.NoWait()
						.Deleted()
						.AutoDelete()
						.Lazy()
						.MaxMessageCount(1000)
						.MaxMessageSize(1000)
						.DeadLetterExchange("exchange")
						.DeadLetterRoutingKey("routing_key")
						.TimeToLive(10000)
						.MessageTimeToLive(1000)
						.MaxPriority(10)
						.RejectPublishOnOverflow()
						.BoundTo("exchange", binding =>
							binding.WithRoutingKey("routing_key")
								.NoWait()
								.WithArgument("x-argument", "argument"))
						.WithArgument("x-argument", "argument"))
				.AddExchangeGroup("exchanges", group =>
					group.AddDirectExchange("exchange")
						.Durable()
						.NoWait()
						.Internal()
						.AutoDelete()
						.Deleted()
						.BoundTo("exchange", binding =>
							binding.WithRoutingKey("routing_key")
								.NoWait()
								.WithArgument("x-argument", "argument"))
						.AlternateExchange("exchange")
						.WithArgument("x-argument", "argument"))
				.AddProducerGroup("producers", group =>
					group.AddProducer<Payload>("exchange")
						.RoutingKey("routing_key")
						.Mandatory()
						.Transactional()
						.WaitForConfirms()
						.Persistent()
						.Priority(1)
						.MessageTimeToLive(10000)
						.WithHeader("x-header", "header")
						.WithProperty(x => x.Persistent = true)
						.WithArgument("x-argument", "argument"));
		}
	}
}