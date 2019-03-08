using System.Threading.Tasks;

using RabbitMQ.Client;
using Phema.Serialization;


namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducer<TPayload>
	{
		Task Produce(TPayload payload);

		Task BatchProduce(params TPayload[] payloads);
	}

	internal sealed class RabbitMQProducer<TPayload> : IRabbitMQProducer<TPayload>
	{
		// Each generic type have unique lock object, because unique channel
		private static readonly object @lock = new object();
		
		private readonly RabbitMQProducer producer;
		private readonly IModel channel;
		private readonly ISerializer serializer;
		private readonly IBasicProperties properties;

		public RabbitMQProducer(
			IModel channel,
			ISerializer serializer,
			RabbitMQProducer producer,
			IBasicProperties properties)
		{
			this.producer = producer;
			this.channel = channel;
			this.serializer = serializer;
			this.properties = properties;
		}

		public Task Produce(TPayload payload)
		{
			lock (@lock)
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

		public Task BatchProduce(params TPayload[] payloads)
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

			lock (@lock)
			{
				batch.Publish();
			}

			return Task.CompletedTask;
		}
	}
}