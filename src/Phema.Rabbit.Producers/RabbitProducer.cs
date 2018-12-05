using System;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	public class RabbitProducer<TPayload> : IDisposable
	{
		internal IModel Channel { get; set; }
		internal Action<IModel, TPayload> ProduceAction { get; set; }
		
		protected internal virtual IBasicProperties Properties => null;

		protected void Produce(TPayload payload)
		{
			ProduceAction(Channel, payload);
		}

		public void Dispose()
		{
			Channel.Close();
			Channel.Dispose();
		}
	}
}