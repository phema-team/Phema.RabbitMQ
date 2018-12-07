using System.Collections.Generic;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	public abstract class RabbitExchange
	{
		public abstract string Name { get; }
		internal abstract string Type { get; }
		public virtual bool Durable => true;
		public virtual bool AutoDelete => false;
		public virtual IDictionary<string, object> Arguments => null;
	}
	
	public class DirectRabbitExchange : RabbitExchange
	{
		public override string Name => "amq.direct";
		internal override string Type => ExchangeType.Direct;
	}
	
	public class FanoutRabbitExchange<TPayload> : RabbitExchange
	{
		public override string Name => "amq.fanout";
		internal override string Type => ExchangeType.Fanout;
	}
	
	public class HeadersRabbitExchange : RabbitExchange
	{
		public override string Name => "amq.headers";
		internal override string Type => ExchangeType.Headers;
	}
	
	public class TopicRabbitExchange : RabbitExchange
	{
		public override string Name => "amq.topic";
		internal override string Type => ExchangeType.Topic;
	}
}