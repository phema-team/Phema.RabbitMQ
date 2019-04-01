using System.Threading.Tasks;

namespace Phema.RabbitMQ
{
	public static class RabbitMQProducerExtensions
	{
		public static Task BatchProduce<TPayload>(
			this IRabbitMQAsyncProducer<TPayload> asyncProducer,
			params TPayload[] payloads)
		{
			return asyncProducer.BatchProduce(payloads);
		}
	}
}