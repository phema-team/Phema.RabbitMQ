namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangeBindingBuilderExtensions
	{
		/// <summary>
		///   Declare exchange to exchange routing key
		/// </summary>
		public static IRabbitMQExchangeBindingBuilder RoutingKey(
			this IRabbitMQExchangeBindingBuilder builder,
			string routingKey)
		{
			return builder.RoutingKeys(routingKey);
		}

		/// <summary>
		///   Declare exchange to exchange routing keys
		/// </summary>
		public static IRabbitMQExchangeBindingBuilder RoutingKeys(
			this IRabbitMQExchangeBindingBuilder builder,
			params string[] routingKeys)
		{
			foreach (var routingKey in routingKeys)
			{
				builder.Declaration.RoutingKeys.Add(routingKey);
			}

			return builder;
		}

		/// <summary>
		///   Nowait for exchange to exchange declaration
		/// </summary>
		public static IRabbitMQExchangeBindingBuilder NoWait(
			this IRabbitMQExchangeBindingBuilder builder)
		{
			builder.Declaration.NoWait = true;

			return builder;
		}

		/// <summary>
		///   Delete exchange binding
		/// </summary>
		public static IRabbitMQExchangeBindingBuilder Deleted(
			this IRabbitMQExchangeBindingBuilder builder)
		{
			builder.Declaration.Deleted = true;

			return builder;
		}

		/// <summary>
		///   Declare RabbitMQ arguments. Allow multiple
		/// </summary>
		public static IRabbitMQExchangeBindingBuilder WithArgument<TValue>(
			this IRabbitMQExchangeBindingBuilder builder,
			string argument,
			TValue value)
		{
			builder.Declaration.Arguments.Add(argument, value);

			return builder;
		}
	}
}