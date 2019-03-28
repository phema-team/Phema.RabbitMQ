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
						.Tag("tag")
						.Prefetched(1)
						.Count(1)
						.Exclusive()
						.NoLocal()
						.AutoAck()
						.Requeue()
						.Priority(2)
						.WithArgument("x-argument", "argument"))
				.AddQueueGroup("queues", group =>
					group.AddQueue("queue")
						.AutoDelete()
						.Exclusive()
						.Deleted()
						.NoWait()
						.Lazy()
						.Durable()
						.MaxPriority(10)
						.TimeToLive(10000)
						.MaxMessageSize(1000)
						.MaxMessageCount(1000)
						.MessageTimeToLive(1000)
						.RejectPublish()
						.BoundTo("exchange", binding =>
							binding.RoutingKey("routing_key")
								.NoWait()
								.Deleted()
								.WithArgument("x-argument", "argument"))
						.WithArgument("x-argument", "argument")
						.DeadLetterExchange("exchange", "routing_key"))
				.AddExchangeGroup("exchanges", group =>
					group.AddDirectExchange("exchange")
						.Internal()
						.Durable()
						.NoWait()
						.Deleted()
						.AutoDelete()
						.AlternateExchange("exchange")
						.BoundTo("exchange", binding =>
							binding.RoutingKey("routing_key")
								.NoWait()
								.Deleted()
								.WithArgument("x-argument", "argument"))
						.WithArgument("x-argument", "argument"))
				.AddProducerGroup("producers", group =>
					group.AddProducer<Payload>("exchange")
						.RoutingKey("routing_key")
						.WaitForConfirms()
						.Transactional()
						.Mandatory()
						.Priority(1)
						.Persistent()
						.MessageTimeToLive(10000)
						.WithHeader("x-header", "header")
						.WithProperty(x => x.Persistent = true)
						.WithArgument("x-argument", "argument"));
		}
	}
}