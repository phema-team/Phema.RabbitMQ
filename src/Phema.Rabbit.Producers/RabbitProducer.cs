using System;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	public class RabbitProducer<TPayload> : IDisposable
	{
		internal IModel Channel { get; set; }
		internal Action<IModel, TPayload> ProduceAction { get; set; }

		protected void Produce(TPayload payload)
		{
			ProduceAction(Channel, payload);
		}

		protected Task ProduceAsync(TPayload payload)
		{
			return Task.Run(() => ProduceAction(Channel, payload));
		}

		public void Dispose()
		{
			Channel.Close();
			Channel.Dispose();
		}
	}
}