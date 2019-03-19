using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangeBuilderExtensions
	{
		/// <summary>
		///   Marks exchange as durable
		/// </summary>
		public static IRabbitMQExchangeBuilder Durable(this IRabbitMQExchangeBuilder builder)
		{
			builder.Metadata.Durable = true;

			return builder;
		}

		/// <summary>
		///   Sets nowait for exchange declaration
		/// </summary>
		public static IRabbitMQExchangeBuilder NoWait(this IRabbitMQExchangeBuilder builder)
		{
			builder.Metadata.NoWait = true;

			return builder;
		}

		/// <summary>
		///   Sets internal for exchange declaration.
		///   Exchange can't be accessed from producers directly, only by exchange to exchange bindings
		/// </summary>
		public static IRabbitMQExchangeBuilder Internal(this IRabbitMQExchangeBuilder builder)
		{
			builder.Metadata.Internal = true;

			return builder;
		}

		/// <summary>
		///   Sets auto-delete flag to exchange
		/// </summary>
		public static IRabbitMQExchangeBuilder AutoDelete(this IRabbitMQExchangeBuilder builder)
		{
			builder.Metadata.AutoDelete = true;

			return builder;
		}

		/// <summary>
		///   Sets RabbitMQ arguments. Allow multiple
		/// </summary>
		public static IRabbitMQExchangeBuilder WithArgument<TValue>(
			this IRabbitMQExchangeBuilder builder,
			string argument,
			TValue value)
		{
			builder.Metadata.Arguments.Add(argument, value);

			return builder;
		}

		/// <summary>
		///   Bind exchange to exchange
		/// </summary>
		public static IRabbitMQExchangeBuilder WithBoundExchange(
			this IRabbitMQExchangeBuilder builder,
			string exchangeName,
			Action<IRabbitMQExchangeBindingBuilder> binding = null)
		{
			if (exchangeName is null)
				throw new ArgumentNullException(nameof(exchangeName));

			var metadata = new RabbitMQExchangeBindingMetadata(exchangeName);

			binding?.Invoke(new RabbitMQExchangeBindingBuilder(metadata));

			builder.Metadata.ExchangeBindings.Add(metadata);

			return builder;
		}

		/// <summary>
		///   Sets alternate-exchange argument. When message can't be routed in current exchange,
		///   instead of mark as dead, publish to specified exchange
		/// </summary>
		public static IRabbitMQExchangeBuilder WithAlternateExchange(
			this IRabbitMQExchangeBuilder configuration,
			string exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.WithArgument("alternate-exchange", exchange);
		}
	}
}