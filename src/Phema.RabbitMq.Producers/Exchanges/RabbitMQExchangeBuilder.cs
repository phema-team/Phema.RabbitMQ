using System;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBuilder
		: IRabbitMQDurableBuilder<IRabbitMQExchangeBuilder>,
			IRabbitMQAutoDeleteBuilder<IRabbitMQExchangeBuilder>,
			IRabbitMQWithArgumentBuilder<IRabbitMQExchangeBuilder>
	{
		/// <summary>
		/// Bind exchange to exchange
		/// </summary>
		IRabbitMQExchangeBuilder WithBoundExchange(
			string exchangeName,
			Action<IRabbitMQExchangeBindingBuilder> builder = null);
	}

	internal sealed class RabbitMQExchangeBuilder : IRabbitMQExchangeBuilder
	{
		private readonly RabbitMQExchangeMetadata exchange;

		public RabbitMQExchangeBuilder(RabbitMQExchangeMetadata exchange)
		{
			this.exchange = exchange;
		}

		public IRabbitMQExchangeBuilder Durable()
		{
			exchange.Durable = true;
			return this;
		}

		public IRabbitMQExchangeBuilder AutoDelete()
		{
			exchange.AutoDelete = true;
			return this;
		}

		public IRabbitMQExchangeBuilder WithArgument<TValue>(string argument, TValue value)
		{
			exchange.Arguments.Add(argument, value);
			return this;
		}

		public IRabbitMQExchangeBuilder WithBoundExchange(
			string exchangeName,
			Action<IRabbitMQExchangeBindingBuilder> builder = null)
		{
			if (exchangeName is null)
				throw new ArgumentNullException(nameof(exchangeName));

			var binding = new RabbitMQExchangeBinding(exchangeName);

			builder?.Invoke(new RabbitMQExchangeBindingBuilder(binding));

			exchange.ExchangeBindings.Add(binding);

			return this;
		}
	}
}