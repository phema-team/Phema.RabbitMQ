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
		// Each generic type have unique lock object
		private static readonly object @lock = new object();

		private readonly Action<TPayload> producer;

		public RabbitMqProducer(Action<TPayload> producer)
		{
			this.producer = producer;
		}

		public ValueTask Produce(TPayload payload)
		{
			lock (@lock)
			{
				producer(payload);
			}

			return new ValueTask();
		}
	}
}