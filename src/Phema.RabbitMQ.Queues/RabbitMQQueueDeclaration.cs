using System.Collections.Generic;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueueDeclaration
	{
		string GroupName { get; }
		string QueueName { get; }
		bool Durable { get; set; }
		bool Exclusive { get; set; }
		bool AutoDelete { get; set; }
		bool NoWait { get; set; }
		
		bool Deleted { get; set; }
		bool IfUnused { get; set; }
		bool IfEmpty { get; set; }
		IDictionary<string, object> Arguments { get; }
		IList<IRabbitMQQueueBindingDeclaration> QueueBindings { get; }
	}
}

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQQueueDeclaration : IRabbitMQQueueDeclaration
	{
		public RabbitMQQueueDeclaration(string groupName, string queueName)
		{
			GroupName = groupName;
			QueueName = queueName;
			Arguments = new Dictionary<string, object>();
			QueueBindings = new List<IRabbitMQQueueBindingDeclaration>();
		}

		public string GroupName { get; }
		public string QueueName { get; }
		public bool Durable { get; set; }
		public bool Exclusive { get; set; }
		public bool AutoDelete { get; set; }
		public bool NoWait { get; set; }
		public bool Deleted { get; set; }
		public bool IfUnused { get; set; }
		public bool IfEmpty { get; set; }
		public IDictionary<string, object> Arguments { get; }
		public IList<IRabbitMQQueueBindingDeclaration> QueueBindings { get; }
	}
}