using System;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeConfiguration
	{
		IRabbitMQExchangeConfiguration Durable();
		IRabbitMQExchangeConfiguration AutoDelete();
		IRabbitMQExchangeConfiguration WithArgument<TValue>(string argument, TValue value);
		IRabbitMQExchangeConfiguration WithBoundExchange(
			string exchangeName,
			Action<IRabbitMQExchangeBindingConfiguration> options = null);
	}

	internal sealed class RabbitMQExchangeConfiguration : IRabbitMQExchangeConfiguration
	{
		private readonly RabbitMQExchange exchange;

		public RabbitMQExchangeConfiguration(RabbitMQExchange exchange)
		{
			this.exchange = exchange;
		}

		/// <summary>
		/// Sets durable to true. Exchange won't be deleted on restart
		/// </summary>
		/// <returns></returns>
		public IRabbitMQExchangeConfiguration Durable()
		{
			exchange.Durable = true;
			return this;
		}

		/// <summary>
		/// Sets auto-delete flag. Exchange will be deleted when no more bound exchanges or queues
		/// </summary>
		public IRabbitMQExchangeConfiguration AutoDelete()
		{
			exchange.AutoDelete = true;
			return this;
		}

		/// <summary>
		/// Sets rabbitmq arguments
		/// </summary>
		public IRabbitMQExchangeConfiguration WithArgument<TValue>(string argument, TValue value)
		{
			if (argument is null)
				throw new ArgumentNullException(nameof(argument));

			if (exchange.Arguments.ContainsKey(argument))
				throw new ArgumentException($"Argument {argument} already registered", nameof(argument));
				
			exchange.Arguments.Add(argument, value);
			return this;
		}
		
		/// <summary>
		/// Bind source exchange with tagret exchange
		/// </summary>
		public IRabbitMQExchangeConfiguration WithBoundExchange(
			string exchangeName,
			Action<IRabbitMQExchangeBindingConfiguration> options = null)
		{
			if (exchangeName is null)
				throw new ArgumentNullException(nameof(exchangeName));
			
			var binding = new RabbitMQExchangeBinding(exchangeName);
			
			options?.Invoke(new RabbitMQExchangeBindingConfiguration(binding));
			
			exchange.BoundExchanges.Add(binding);
			
			return this;
		}
	}
}