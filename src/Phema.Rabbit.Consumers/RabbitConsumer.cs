using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phema.Rabbit
{
	public abstract class RabbitConsumer<TPayload>
	{
		protected internal abstract string Name { get; }
		protected internal virtual int Parallelism => 1;
		protected internal virtual ushort? Prefetch => null; 
		protected internal virtual bool AutoAck => false;
		protected internal virtual bool Requeue => true;
		protected internal virtual bool NoLocal => true;
		protected internal virtual bool Exclusive => false;
		protected internal virtual IDictionary<string, object> Arguments => null;
		
		protected internal abstract Task Consume(TPayload model);
	}
}