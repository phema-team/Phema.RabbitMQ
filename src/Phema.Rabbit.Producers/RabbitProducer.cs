using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	public class RabbitProducer<TPayload> : IDisposable
	{
		internal IModel Channel { get; set; }
		internal RabbitExchange Exchange { get; set; }
		internal RabbitQueue<TPayload> Queue { get; set; }
		internal RabbitOptions Options { get; set; }
		
		protected internal virtual string RoutingKey => null;
		protected internal virtual bool Mandatory => false;
		protected internal virtual IBasicProperties Properties => null;
		
		protected void Produce(TPayload payload)
		{
			Channel.BasicPublish(
				Exchange.Name,
				RoutingKey ?? Queue.RoutingKey,
				Mandatory,
				Properties,
				Options.Encoding.GetBytes(JsonConvert.SerializeObject(payload, Options.SerializerSettings)));
		}

		protected Task ProduceAsync(TPayload payload)
		{
			return Task.Run(() => Produce(payload));
		}

		public void Dispose()
		{
			Channel.Close();
			Channel.Dispose();
		}
	}
}