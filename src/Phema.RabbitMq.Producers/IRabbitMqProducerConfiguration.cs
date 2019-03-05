using System;

using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducerConfiguration
	{
		IRabbitMQProducerConfiguration WithRoutingKey(string routingKey);
		IRabbitMQProducerConfiguration Mandatory();
		IRabbitMQProducerConfiguration WithArgument<TValue>(string argument, TValue value);
		IRabbitMQProducerConfiguration WithProperties(params Action<IBasicProperties>[] properties);
	}

	internal sealed class RabbitMQProducerConfiguration : IRabbitMQProducerConfiguration
	{
		private readonly RabbitMQProducer producer;

		public RabbitMQProducerConfiguration(RabbitMQProducer producer)
		{
			this.producer = producer;
		}

		/// <summary>
		/// Sets routing-key (default queueName)
		/// </summary>
		public IRabbitMQProducerConfiguration WithRoutingKey(string routingKey)
		{
			if (routingKey is null)
				throw new ArgumentNullException(nameof(routingKey));
			
			producer.RoutingKey = routingKey;
			return this;
		}

		/// <summary>
		/// Sets mangatory flag. When the message cannot be routed to a queue, returns to client. If false - drops
		/// </summary>
		/// <returns></returns>
		public IRabbitMQProducerConfiguration Mandatory()
		{
			producer.Mandatory = true;
			return this;
		}
		
		/// <summary>
		/// Sets rabbitmq arguments
		/// </summary>
		public IRabbitMQProducerConfiguration WithArgument<TValue>(string argument, TValue value)
		{
			if (argument is null)
				throw new ArgumentNullException(nameof(argument));
			
			if (producer.Arguments.ContainsKey(argument))
				throw new ArgumentException($"Argument {argument} already registered", nameof(argument));
			
			producer.Arguments.Add(argument, value);
			return this;
		}
		
		/// <summary>
		/// Sets producers properties applied to all messages
		/// </summary>
		public IRabbitMQProducerConfiguration WithProperties(params Action<IBasicProperties>[] properties)
		{
			foreach (var property in properties)
				producer.Properties.Add(property);

			return this;
		}
	}
}