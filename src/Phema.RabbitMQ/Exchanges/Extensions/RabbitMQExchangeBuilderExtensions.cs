using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangeBuilderExtensions
	{
		/// <summary>
		///   Declare exchange as durable
		/// </summary>
		public static IRabbitMQExchangeBuilder<TPayload> Durable<TPayload>(this IRabbitMQExchangeBuilder<TPayload> builder)
		{
			builder.Declaration.Durable = true;

			return builder;
		}

		/// <summary>
		///   Nowait for exchange declaration
		/// </summary>
		public static IRabbitMQExchangeBuilder<TPayload> NoWait<TPayload>(this IRabbitMQExchangeBuilder<TPayload> builder)
		{
			builder.Declaration.NoWait = true;

			return builder;
		}

		/// <summary>
		///   Declare exchange as internal.
		///   Exchange can't be accessed from producers directly, only by exchange to exchange bindings
		/// </summary>
		public static IRabbitMQExchangeBuilder<TPayload> Internal<TPayload>(this IRabbitMQExchangeBuilder<TPayload> builder)
		{
			builder.Declaration.Internal = true;

			return builder;
		}

		/// <summary>
		///   Declare auto-delete flag to exchange
		/// </summary>
		public static IRabbitMQExchangeBuilder<TPayload> AutoDelete<TPayload>(
			this IRabbitMQExchangeBuilder<TPayload> builder)
		{
			builder.Declaration.AutoDelete = true;

			return builder;
		}

		/// <summary>
		///   Delete excahnge declaration
		/// </summary>
		public static IRabbitMQExchangeBuilder<TPayload> Deleted<TPayload>(
			this IRabbitMQExchangeBuilder<TPayload> builder,
			bool ifUnused = false)
		{
			builder.Declaration.Deleted = true;
			builder.Declaration.IfUnused = ifUnused;

			return builder;
		}

		/// <summary>
		///   Declare exchange to exchange binding
		/// </summary>
		public static IRabbitMQExchangeBuilder<TPayload> BoundTo<TPayload>(
			this IRabbitMQExchangeBuilder<TPayload> builder,
			IRabbitMQExchangeBuilder<TPayload> exchange,
			Action<IRabbitMQExchangeBindingBuilder> binding = null)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			var declaration = new RabbitMQExchangeBindingDeclaration(exchange.Declaration);

			binding?.Invoke(new RabbitMQExchangeBindingBuilder(declaration));

			builder.Declaration.Bindings.Add(declaration);

			return builder;
		}

		/// <summary>
		///   Declare alternate-exchange argument. When message can't be routed in current exchange,
		///   instead of mark as dead, publish to specified exchange
		/// </summary>
		public static IRabbitMQExchangeBuilder<TPayload> AlternateTo<TPayload>(
			this IRabbitMQExchangeBuilder<TPayload> configuration,
			IRabbitMQExchangeBuilder<TPayload> exchange)
		{
			if (exchange is null)
				throw new ArgumentNullException(nameof(exchange));

			return configuration.Argument("alternate-exchange", exchange.Declaration.Name);
		}

		/// <summary>
		///   Declare RabbitMQ arguments. Allow multiple
		/// </summary>
		public static IRabbitMQExchangeBuilder<TPayload> Argument<TPayload>(
			this IRabbitMQExchangeBuilder<TPayload> builder,
			string argument,
			object value)
		{
			builder.Declaration.Arguments.Add(argument, value);

			return builder;
		}
	}
}