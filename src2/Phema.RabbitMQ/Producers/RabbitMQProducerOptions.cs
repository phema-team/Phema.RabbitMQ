using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQProducerOptions
	{
		public RabbitMQProducerOptions()
		{
			Producers = new ConcurrentDictionary<Type, RabbitMQProducerEntry>();
		}
		
		public IDictionary<Type, RabbitMQProducerEntry> Producers { get; }
	}

	internal sealed class RabbitMQProducerEntry
	{
		public IModel Channel { get; set; }
		public IBasicProperties Properties { get; set; }
		public IRabbitMQProducerMetadata Metadata { get; set; }
	}
}