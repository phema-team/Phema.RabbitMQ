using System;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	public abstract class RabbitExchange
	{
		public abstract string Name { get; }
		internal virtual string Type => throw new InvalidOperationException();
		public virtual bool Durable => true;
		public virtual bool AutoDelete => true;
		public virtual IDictionary<string, object> Arguments => null;
	}
	
	public abstract class DirectRabbitExchange : RabbitExchange
	{
		internal override string Type => ExchangeType.Direct;
	}
	
	public abstract class FanoutRabbitExchange<TPayload> : RabbitExchange
	{
		internal override string Type => ExchangeType.Fanout;
	}
	
	public abstract class HeadersRabbitExchange : RabbitExchange
	{
		internal override string Type => ExchangeType.Headers;
	}
	
	public abstract class TopicRabbitExchange : RabbitExchange
	{
		internal override string Type => ExchangeType.Topic;
	}
}