using System.Collections.Generic;
using System.Threading.Tasks;

namespace Phema.Rabbit
{
	public abstract class RabbitConsumer<TPayload>
	{
		protected internal virtual int Parallelism { get; } = 1; 
		protected internal abstract string Tag { get; }
		protected internal virtual bool AutoAck => true;
		protected internal virtual bool NoLocal => true;
		protected internal virtual bool Exclusive => false;
		protected internal virtual IDictionary<string, object> Arguments => null;
		
		protected internal abstract Task Consume(TPayload model);
	}
}