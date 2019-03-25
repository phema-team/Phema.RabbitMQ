namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangeBindingBuilderExtensions
	{
		/// <summary>
		///   Declare exchange to exchange routing key
		/// </summary>
		public static IRabbitMQExchangeBindingBuilder WithRoutingKey(
			this IRabbitMQExchangeBindingBuilder builder,
			string routingKey)
		{
			builder.Declaration.RoutingKey = routingKey;

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