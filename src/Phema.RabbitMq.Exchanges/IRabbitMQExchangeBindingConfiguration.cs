using System;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBindingConfiguration
	{
		IRabbitMQExchangeBindingConfiguration WithRoutingKey(string routingKey);
		IRabbitMQExchangeBindingConfiguration WithArgument(string argument, string value);
	}
	
	internal sealed class RabbitMQExchangeBindingConfiguration : IRabbitMQExchangeBindingConfiguration
	{
		private readonly RabbitMQExchangeBinding binding;

		public RabbitMQExchangeBindingConfiguration(RabbitMQExchangeBinding binding)
		{
			this.binding = binding;
		}

		/// <summary>
		/// Sets routing-key (default targetExchangeName)
		/// </summary>
		public IRabbitMQExchangeBindingConfiguration WithRoutingKey(string routingKey)
		{
			if (routingKey is null)
				throw new ArgumentNullException(nameof(routingKey));
			
			binding.RoutingKey = routingKey;
			return this;
		}

		/// <summary>
		/// Sets rabbitmq arguments
		/// </summary>
		public IRabbitMQExchangeBindingConfiguration WithArgument(string argument, string value)
		{
			if (argument is null)
				throw new ArgumentNullException(nameof(argument));
			
			if (binding.Arguments.ContainsKey(argument))
				throw new ArgumentException($"Argument {argument} already registered", nameof(argument));
			
			binding.Arguments.Add(argument, value);
			
			return this;
		}
	}
}