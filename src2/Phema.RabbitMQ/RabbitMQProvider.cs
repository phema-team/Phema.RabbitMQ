using System;
using Microsoft.Extensions.Options;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProvider
	{
		IRabbitMQProvider ConsumerGroup(string groupName, Action<IRabbitMQConsumerGroupProvider> group);
		IRabbitMQProvider ExchangeGroup(string groupName, Action<IRabbitMQExchangeGroupProvider> group);
		IRabbitMQProvider ProducerGroup(string groupName, Action<IRabbitMQProducerGroupProvider> group);
		IRabbitMQProvider QueueGroup(string groupName, Action<IRabbitMQQueueGroupProvider> group);
	}
	
	internal sealed class RabbitMQProvider : IRabbitMQProvider
	{
		private readonly IRabbitMQConnectionFactory connectionFactory;
		private readonly IRabbitMQConsumerFactory consumerFactory;
		private readonly RabbitMQProducerOptions options;

		public RabbitMQProvider(
			IRabbitMQConnectionFactory connectionFactory,
			IRabbitMQConsumerFactory consumerFactory,
			IOptions<RabbitMQProducerOptions> options)
		{
			this.connectionFactory = connectionFactory;
			this.consumerFactory = consumerFactory;
			this.options = options.Value;
		}
		
		public IRabbitMQProvider ConsumerGroup(string groupName, Action<IRabbitMQConsumerGroupProvider> group)
		{
			if (groupName is null)
				throw new ArgumentNullException(nameof(groupName));

			group.Invoke(new RabbitMQConsumerGroupProvider(connectionFactory.CreateConnection(groupName), consumerFactory));

			return this;
		}

		public IRabbitMQProvider ExchangeGroup(string groupName, Action<IRabbitMQExchangeGroupProvider> group)
		{
			if (groupName is null)
				throw new ArgumentNullException(nameof(groupName));

			group.Invoke(new RabbitMQExchangeGroupProvider(groupName, connectionFactory));

			return this;
		}

		public IRabbitMQProvider ProducerGroup(string groupName, Action<IRabbitMQProducerGroupProvider> group)
		{
			if (groupName is null)
				throw new ArgumentNullException(nameof(groupName));

			group.Invoke(new RabbitMQProducerGroupProvider(connectionFactory.CreateConnection(groupName), options));

			return this;
		}

		public IRabbitMQProvider QueueGroup(string groupName, Action<IRabbitMQQueueGroupProvider> group)
		{
			if (groupName is null)
				throw new ArgumentNullException(nameof(groupName));

			group.Invoke(new RabbitMQQueueGroupProvider(groupName, connectionFactory));

			return this;
		}
	}
}