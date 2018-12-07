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
	
	public abstract class DirectRabbitExchange : RabbitExchange
	{
		internal override string Type => ExchangeType.Direct;
	}

	internal sealed class DefaultDirectRabbitExchange : DirectRabbitExchange
	{
		public override string Name => "amq.direct";
	}
	
	public abstract class FanoutRabbitExchange<TPayload> : RabbitExchange
	{
		internal override string Type => ExchangeType.Fanout;
	}

	internal sealed class DefaultFanoutRabbitExchange<TPayload> : FanoutRabbitExchange<TPayload>
	{
		public override string Name => "amq.fanout";
	}
	
	public abstract class HeadersRabbitExchange : RabbitExchange
	{
		internal override string Type => ExchangeType.Headers;
	}

	internal class DefaultHeadersRabbitExchange : HeadersRabbitExchange
	{
		public override string Name => "amq.headers";
	}
	
	public abstract class TopicRabbitExchange : RabbitExchange
	{
		internal override string Type => ExchangeType.Topic;
	}

	internal class DefaultTopicRabbitExchange : TopicRabbitExchange
	{
		public override string Name => "amq.topic";
	}
}