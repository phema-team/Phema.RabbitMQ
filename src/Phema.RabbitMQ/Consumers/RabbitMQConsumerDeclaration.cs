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
			RabbitMQConnectionDeclaration connectionDeclaration,
			RabbitMQQueueDeclaration[] queueDeclarations)
		{
			Type = type;
			ConnectionDeclaration = connectionDeclaration;
			QueueDeclarations = queueDeclarations;
			Count = 1;
			Arguments = new Dictionary<string, object>();
			Subscriptions = new List<Func<IServiceScope, object, CancellationToken, ValueTask>>();
		}

		public Type Type { get; }
		public RabbitMQConnectionDeclaration ConnectionDeclaration { get; }
		public RabbitMQQueueDeclaration[] QueueDeclarations { get; }
		public ICollection<Func<IServiceScope, object, CancellationToken, ValueTask>> Subscriptions { get; }

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