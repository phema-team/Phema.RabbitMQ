using System;
using Phema.RabbitMQ.Internal;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangeBuilderExtensions
	{
		/// <summary>
		///   Declare exchange as durable
		/// </summary>
		public static IRabbitMQExchangeBuilder Durable(this IRabbitMQExchangeBuilder builder)
		{
			builder.Declaration.Durable = true;

			return builder;
		}

		/// <summary>
		///   Nowait for exchange declaration
		/// </summary>
		public static IRabbitMQExchangeBuilder NoWait(this IRabbitMQExchangeBuilder builder)
		{
			builder.Declaration.NoWait = true;

			return builder;
		}

		/// <summary>
		///   Declare exchange as internal.
		///   Exchange can't be accessed from producers directly, only by exchange to exchange bindings
		/// </summary>
		public static IRabbitMQExchangeBuilder Internal(this IRabbitMQExchangeBuilder builder)
		{
			builder.Declaration.Internal = true;

			return builder;
		}

		/// <summary>
		///   Declare auto-delete flag to exchange
		/// </summary>
		public static IRabbitMQExchangeBuilder AutoDelete(this IRabbitMQExchangeBuilder builder)
		{
			builder.Declaration.AutoDelete = true;

			return builder;
		}
		
		/// <summary>
		///   Delete excahnge declaration
		/// </summary>
		public static IRabbitMQExchangeBuilder Deleted(
			this IRabbitMQExchangeBuilder builder,
			bool ifUnused = false)
		{
			builder.Declaration.Deleted = true;
			builder.Declaration.IfUnused = ifUnused;

			return builder;
		}

		/// <summary>
		///   Declare exchange to exchange binding
		/// </summary>
		public static IRabbitMQExchangeBuilder BoundTo(
			this IRabbitMQExchangeBuilder builder,
			string exchangeName,
			Action<IRabbitMQExchangeBindingBuilder> binding = null)
		{
			if (exchangeName is null)
				throw new ArgumentNullException(nameof(exchangeName));

			var declaration = new RabbitMQExchangeBindingDeclaration(exchangeName);

			binding?.Invoke(new RabbitMQExchangeBindingBuilder(declaration));

			builder.Declaration.ExchangeBindings.Add(declaration);

			return builder;
		}

		/// <summary>
		///   Declare alternate-exchange argument. When message can't be routed in current exchange,
		///   instead of mark as dead, publish to specified exchange
		/// </summary>
		public static IRabbitMQExchangeBuilder AlternateExchange(
			this IRabbitMQExchangeBuilder configuration,
			string exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.WithArgument("alternate-exchange", exchange);
		}

		/// <summary>
		///   Declare RabbitMQ arguments. Allow multiple
		/// </summary>
		public static IRabbitMQExchangeBuilder WithArgument<TValue>(
			this IRabbitMQExchangeBuilder builder,
			string argument,
			TValue value)
		{
			builder.Declaration.Arguments.Add(argument, value);

			return builder;
		}
	}
}