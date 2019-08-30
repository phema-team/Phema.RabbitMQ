using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public sealed class RabbitMQConsumerDeclaration
	{
		public RabbitMQConsumerDeclaration(
			Type type,
			RabbitMQConnectionDeclaration connection,
			RabbitMQQueueDeclaration queue,
			Func<IServiceScope, object, CancellationToken, ValueTask> consumer)
		{
			Type = type;
			Connection = connection;
			Queue = queue;
			Consumer = consumer;
			Count = 1;
			Arguments = new Dictionary<string, object>();
		}

		public Type Type { get; }
		public RabbitMQConnectionDeclaration Connection { get; }
		public RabbitMQQueueDeclaration Queue { get; }
		public Func<IServiceScope, object, CancellationToken, ValueTask> Consumer { get; }

		public string Tag { get; set; }
		public ushort PrefetchCount { get; set; }
		public bool Global { get; set; }
		public uint Count { get; set; }
		public bool Exclusive { get; set; }
		public bool NoLocal { get; set; }
		public bool AutoAck { get; set; }
		public bool Requeue { get; set; }
		public bool Multiple { get; set; }
		public IDictionary<string, object> Arguments { get; }
	}
}