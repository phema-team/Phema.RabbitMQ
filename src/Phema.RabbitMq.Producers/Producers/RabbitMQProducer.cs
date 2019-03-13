using System.Collections.Generic;
using System.Threading.Tasks;
using Phema.Serialization;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducer<TPayload>
	{
		Task Produce(TPayload payload);

		Task BatchProduce(IEnumerable<TPayload> payloads);
	}

	internal sealed class RabbitMQProducer<TPayload> : IRabbitMQProducer<TPayload>
	{
		// Each generic type have unique lock object, because unique channel
		private static readonly object Lock = new object();

		private readonly RabbitMQProducerMetadata producer;
		private readonly IModel channel;
		private readonly ISerializer serializer;
		private readonly IBasicProperties properties;

		public RabbitMQProducer(
			IModel channel,
			ISerializer serializer,
			RabbitMQProducerMetadata producer,
			IBasicProperties properties)
		{
			this.producer = producer;
			this.channel = channel;
			this.serializer = serializer;
			this.properties = properties;
		}

		public Task Produce(TPayload payload)
		{
			lock (Lock)
			{
				channel.BasicPublish(
					producer.ExchangeName,
					producer.RoutingKey ?? producer.QueueName,
					producer.Mandatory,
					properties,
					serializer.Serialize(payload));
			}

			return Task.CompletedTask;
		}

		public Task BatchProduce(IEnumerable<TPayload> payloads)
		{
			var batch = channel.CreateBasicPublishBatch();

			foreach (var payload in payloads)
			{
				batch.Add(
					producer.ExchangeName,
					producer.RoutingKey ?? producer.QueueName,
					producer.Mandatory,
					properties,
					serializer.Serialize(payload));
			}

			lock (Lock)
			{
				batch.Publish();
			}

			return Task.CompletedTask;
		}
	}
}