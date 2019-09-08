using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangeBuilderExtensions
	{
		/// <summary>
		///   Declare exchange as durable
		/// </summary>
		public static TBuilder Durable<TBuilder>(
			this TBuilder builder)
			where TBuilder : IRabbitMQExchangeBuilderCore
		{
			builder.ExchangeDeclaration.Durable = true;

			return builder;
		}

		/// <summary>
		///   Nowait for exchange declaration
		/// </summary>
		public static TBuilder NoWait<TBuilder>(
			this TBuilder builder)
			where TBuilder : IRabbitMQExchangeBuilderCore
		{
			builder.ExchangeDeclaration.NoWait = true;

			return builder;
		}

		/// <summary>
		///   Declare exchange as internal.
		///   Exchange can't be accessed from producers directly, only by exchange to exchange bindings
		/// </summary>
		public static TBuilder Internal<TBuilder>(
			this TBuilder builder)
			where TBuilder : IRabbitMQExchangeBuilderCore
		{
			builder.ExchangeDeclaration.Internal = true;

			return builder;
		}

		/// <summary>
		///   Declare auto-delete flag to exchange
		/// </summary>
		public static TBuilder AutoDelete<TBuilder>(
			this TBuilder builder)
			where TBuilder : IRabbitMQExchangeBuilderCore
		{
			builder.ExchangeDeclaration.AutoDelete = true;

			return builder;
		}

		/// <summary>
		///   Delete excahnge declaration
		/// </summary>
		public static TBuilder Deleted<TBuilder>(
			this TBuilder builder,
			bool unusedOnly = false)
			where TBuilder : IRabbitMQExchangeBuilderCore
		{
			builder.ExchangeDeclaration.Deleted = true;
			builder.ExchangeDeclaration.UnusedOnly = unusedOnly;

			return builder;
		}

		/// <summary>
		///   Declare RabbitMQ arguments. Allow multiple
		/// </summary>
		public static TBuilder Argument<TBuilder>(
			this TBuilder builder,
			string argument,
			object value)
			where TBuilder : IRabbitMQExchangeBuilderCore
		{
			builder.ExchangeDeclaration.Arguments.Add(argument, value);

			return builder;
		}

		#region BoundTo

		private static TBuilder BoundTo<TBuilder>(
			this TBuilder builder,
			IRabbitMQExchangeBuilderCore exchange,
			Action<IRabbitMQExchangeBindingBuilder> binding)
			where TBuilder : IRabbitMQExchangeBuilderCore
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			var declaration = new RabbitMQExchangeBindingDeclaration(exchange.ExchangeDeclaration);

			binding?.Invoke(new RabbitMQExchangeBindingBuilder(declaration));

			builder.ExchangeDeclaration.BindingDeclarations.Add(declaration);

			return builder;
		}

		/// <summary>
		///   Declare exchange to exchange binding
		/// </summary>
		public static IRabbitMQExchangeBuilder BoundTo(
			this IRabbitMQExchangeBuilder builder,
			IRabbitMQExchangeBuilderCore exchange,
			Action<IRabbitMQExchangeBindingBuilder> binding = null)
		{
			return builder.BoundTo<IRabbitMQExchangeBuilder>(exchange, binding);
		}

		/// <summary>
		///   Declare exchange to exchange binding
		/// </summary>
		public static IRabbitMQExchangeBuilder<TPayload> BoundTo<TPayload>(
			this IRabbitMQExchangeBuilder<TPayload> builder,
			IRabbitMQExchangeBuilder<TPayload> exchange,
			Action<IRabbitMQExchangeBindingBuilder> binding = null)
		{
			return builder.BoundTo<IRabbitMQExchangeBuilder<TPayload>>(exchange, binding);
		}

		/// <summary>
		///   Declare exchange to exchange binding
		/// </summary>
		public static IRabbitMQExchangeBuilder<TPayload> BoundTo<TPayload>(
			this IRabbitMQExchangeBuilder<TPayload> builder,
			IRabbitMQExchangeBuilder exchange,
			Action<IRabbitMQExchangeBindingBuilder> binding = null)
		{
			return builder.BoundTo<IRabbitMQExchangeBuilder<TPayload>>(exchange, binding);
		}

		#endregion

		#region AlternateTo

		private static TBuilder AlternateTo<TBuilder>(
			this TBuilder builder,
			IRabbitMQExchangeBuilderCore exchange)
			where TBuilder : IRabbitMQExchangeBuilderCore
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return builder.Argument("alternate-exchange", exchange.ExchangeDeclaration.Name);
		}
		
		/// <summary>
		///   Declare alternate-exchange argument. When message can't be routed in current exchange,
		///   instead of mark as dead, publish to specified exchange
		/// </summary>
		public static IRabbitMQExchangeBuilder AlternateTo(
			this IRabbitMQExchangeBuilder builder,
			IRabbitMQExchangeBuilderCore exchange)
		{
			return builder.AlternateTo<IRabbitMQExchangeBuilder>(exchange);
		}

		/// <summary>
		///   Declare alternate-exchange argument. When message can't be routed in current exchange,
		///   instead of mark as dead, publish to specified exchange
		/// </summary>
		public static IRabbitMQExchangeBuilder<TPayload> AlternateTo<TPayload>(
			this IRabbitMQExchangeBuilder<TPayload> builder,
			IRabbitMQExchangeBuilder<TPayload> exchange)
		{
			return builder.AlternateTo<IRabbitMQExchangeBuilder<TPayload>>(exchange);
		}
		
		/// <summary>
		///   Declare alternate-exchange argument. When message can't be routed in current exchange,
		///   instead of mark as dead, publish to specified exchange
		/// </summary>
		public static IRabbitMQExchangeBuilder<TPayload> AlternateTo<TPayload>(
			this IRabbitMQExchangeBuilder<TPayload> builder,
			IRabbitMQExchangeBuilder exchange)
		{
			return builder.AlternateTo<IRabbitMQExchangeBuilder<TPayload>>(exchange);
		}
		
		#endregion
	}
}