using System.Collections.Generic;
using System.Threading.Tasks;
using Phema.Serialization;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducer<TPayload>
	{
		Task<bool> Produce(TPayload payload);

		Task<bool> BatchProduce(IEnumerable<TPayload> payloads);
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

		public Task<bool> Produce(TPayload payload)
		{
			return Task.Run(() =>
			{
				lock (Lock)
				{
					channel.BasicPublish(
						metadata.ExchangeName,
						metadata.RoutingKey ?? metadata.QueueName,
						metadata.Mandatory,
						properties,
						serializer.Serialize(payload));

					return !metadata.WaitForConfirms || WaitForConfirms();
				}
			});
		}

		public Task<bool> BatchProduce(IEnumerable<TPayload> payloads)
		{
			var batch = channel.CreateBasicPublishBatch();

			foreach (var payload in payloads)
				batch.Add(
					metadata.ExchangeName,
					metadata.RoutingKey ?? metadata.QueueName,
					metadata.Mandatory,
					properties,
					serializer.Serialize(payload));

			return Task.Run(() =>
			{
				lock (Lock)
				{
					batch.Publish();
					
					return !metadata.WaitForConfirms || WaitForConfirms();
				}
			});
		}

		private bool WaitForConfirms()
		{
			if (metadata.Die)
			{
				if (metadata.Timeout is null)
				{
					channel.WaitForConfirmsOrDie();
				}
				else
				{
					channel.WaitForConfirmsOrDie(metadata.Timeout.Value);
				}

				return true;
			}

			return metadata.Timeout is null 
				? channel.WaitForConfirms() 
				: channel.WaitForConfirms(metadata.Timeout.Value);
		}
	}
}