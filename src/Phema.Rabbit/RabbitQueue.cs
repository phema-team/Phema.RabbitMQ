using System.Collections.Generic;

namespace Phema.Rabbit
{
	public abstract class RabbitQueue<TPayload>
	{
		protected internal abstract string Name { get; }
		internal virtual bool Durable => true;
		protected internal virtual bool Exclusive => false;
		protected internal virtual bool AutoDelete => true;
		protected internal virtual IDictionary<string, object> Arguments => null;
	}
}