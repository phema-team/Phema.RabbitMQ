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
		// Each generic type has unique lock object, because unique channel
		private static readonly object Lock = new object();
		
		private readonly IModel channel;
		private readonly ISerializer serializer;
		private readonly IBasicProperties properties;
		private readonly IRabbitMQProducerMetadata metadata;

		public RabbitMQProducer(
			IModel channel,
			ISerializer serializer,
			IRabbitMQProducerMetadata metadata,
			IBasicProperties properties)
		{
			this.channel = channel;
			this.metadata = metadata;
			this.serializer = serializer;
			this.properties = properties;
		}

		public Task Produce(TPayload payload)
		{
			lock (Lock)
			{
				channel.BasicPublish(
					metadata.ExchangeName,
					metadata.RoutingKey ?? metadata.QueueName,
					metadata.Mandatory,
					properties,
					serializer.Serialize(payload));
			}

			return Task.CompletedTask;
		}

		public Task BatchProduce(IEnumerable<TPayload> payloads)
		{
			var batch = channel.CreateBasicPublishBatch();

			foreach (var payload in payloads)
				batch.Add(
					metadata.ExchangeName,
					metadata.RoutingKey ?? metadata.QueueName,
					metadata.Mandatory,
					properties,
					serializer.Serialize(payload));

			lock (Lock)
				batch.Publish();

			return Task.CompletedTask;
		}
	}
}