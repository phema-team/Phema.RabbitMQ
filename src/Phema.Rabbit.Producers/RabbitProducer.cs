using System;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	public class RabbitProducer<TPayload> : IDisposable
	{
		internal IModel Model { get; set; }
		internal Action<IModel, TPayload> ProduceAction { get; set; }
		
		protected internal virtual IBasicProperties Properties => null;

		protected void Produce(TPayload payload)
		{
			ProduceAction(Model, payload);
		}

		public void Dispose()
		{
			Model.Close();
			Model.Dispose();
		}
	}
}