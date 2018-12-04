using System;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	public class RabbitProducer<TPayload>
	{
		internal Action<TPayload> ProduceAction { get; set; }
		
		protected internal virtual IBasicProperties Properties => null;

		protected void Produce(TPayload payload)
		{
			ProduceAction(payload);
		}
	}
}