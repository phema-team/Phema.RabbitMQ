using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	/// <summary>
	/// Used to define <see cref="RabbitProducer{TPayload,TRabbitExchange}"/>
	/// </summary>
	public class RabbitProducer<TPayload, TRabbitExchange> : IDisposable
		where TRabbitExchange : RabbitExchange
	{
		internal IModel Channel { get; set; }
		internal TRabbitExchange Exchange { get; set; }
		internal RabbitQueue<TPayload> Queue { get; set; }
		internal RabbitOptions Options { get; set; }
		
		protected internal virtual string RoutingKey => null;
		protected internal virtual bool Mandatory => false;
		protected internal virtual IBasicProperties Properties => null;
		
		/// <summary>
		/// Used to sent <see cref="TPayload"/> to broker
		/// </summary>
		protected void Produce(TPayload payload)
		{
			Channel.BasicPublish(
				Exchange.Name,
				RoutingKey ?? Queue.RoutingKey,
				Mandatory,
				Properties,
				Options.Encoding.GetBytes(JsonConvert.SerializeObject(payload, Options.SerializerSettings)));
		}

		/// <summary>
		/// Used to asynchronosly sent <see cref="TPayload"/> to broker
		/// </summary>
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