using System;
using System.Threading.Tasks;

namespace Phema.RabbitMq
{
	public interface IRabbitMqProducer<TPayload>
	{
		ValueTask Produce(TPayload payload);
	}
	
	internal sealed class RabbitMqProducer<TPayload> : IRabbitMqProducer<TPayload>
	{
		private readonly Action<TPayload> producer;

		public RabbitMqProducer(Action<TPayload> producer)
		{
			this.producer = producer;
		}
		
		public ValueTask Produce(TPayload payload)
		{
			producer(payload);
			return new ValueTask();
		}
	}
}