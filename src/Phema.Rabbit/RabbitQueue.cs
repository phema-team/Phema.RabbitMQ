using System.Collections.Generic;

namespace Phema.Rabbit
{
	/// <summary>
	/// Used to define <see cref="RabbitQueue{TPayload}"/>
	/// </summary>
	/// <typeparam name="TPayload"></typeparam>
	public abstract class RabbitQueue<TPayload>
	{
		protected internal abstract string Name { get; }
		protected internal virtual string RoutingKey => Name;
		protected internal virtual bool Durable => true;
		protected internal virtual bool Exclusive => false;
		protected internal virtual bool AutoDelete => false;
		protected internal virtual IDictionary<string, object> Arguments => null;
	}
}