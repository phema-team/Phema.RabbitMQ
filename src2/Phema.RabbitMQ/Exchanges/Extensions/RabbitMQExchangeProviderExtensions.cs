using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangeProviderExtensions
	{
		/// <summary>
		///   Marks exchange as durable
		/// </summary>
		public static IRabbitMQExchangeProvider Durable(this IRabbitMQExchangeProvider builder)
		{
			builder.Metadata.Durable = true;

			return builder;
		}

		/// <summary>
		///   Sets nowait for exchange declaration
		/// </summary>
		public static IRabbitMQExchangeProvider NoWait(this IRabbitMQExchangeProvider builder)
		{
			builder.Metadata.NoWait = true;

			return builder;
		}

		/// <summary>
		///   Sets internal for exchange declaration.
		///   Exchange can't be accessed from producers directly, only by exchange to exchange bindings
		/// </summary>
		public static IRabbitMQExchangeProvider Internal(this IRabbitMQExchangeProvider builder)
		{
			builder.Metadata.Internal = true;

			return builder;
		}

		/// <summary>
		///   Sets auto-delete flag to exchange
		/// </summary>
		public static IRabbitMQExchangeProvider AutoDelete(this IRabbitMQExchangeProvider builder)
		{
			builder.Metadata.AutoDelete = true;

			return builder;
		}
		
		/// <summary>
		///   Sets auto-delete flag to exchange
		/// </summary>
		public static IRabbitMQExchangeProvider Deleted(
			this IRabbitMQExchangeProvider builder,
			bool ifUnused = false)
		{
			builder.Metadata.Deleted = true;
			builder.Metadata.IfUnused = ifUnused;

			return builder;
		}

		/// <summary>
		///   Sets RabbitMQ arguments. Allow multiple
		/// </summary>
		public static IRabbitMQExchangeProvider WithArgument<TValue>(
			this IRabbitMQExchangeProvider builder,
			string argument,
			TValue value)
		{
			builder.Metadata.Arguments.Add(argument, value);

			return builder;
		}

		/// <summary>
		///   Bind exchange to exchange
		/// </summary>
		public static IRabbitMQExchangeProvider WithBoundExchange(
			this IRabbitMQExchangeProvider builder,
			string exchangeName,
			Action<IRabbitMQExchangeBindingProvider> binding = null)
		{
			if (exchangeName is null)
				throw new ArgumentNullException(nameof(exchangeName));

			var metadata = new RabbitMQExchangeBindingMetadata(exchangeName);

			binding?.Invoke(new RabbitMQExchangeBindingProvider(metadata));

			builder.Metadata.ExchangeBindings.Add(metadata);

			return builder;
		}

		/// <summary>
		///   Sets alternate-exchange argument. When message can't be routed in current exchange,
		///   instead of mark as dead, publish to specified exchange
		/// </summary>
		public static IRabbitMQExchangeProvider WithAlternateExchange(
			this IRabbitMQExchangeProvider configuration,
			string exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.WithArgument("alternate-exchange", exchange);
		}
	}
}