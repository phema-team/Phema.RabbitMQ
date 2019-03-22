namespace Phema.RabbitMQ
{
	public static class RabbitMQExchangeBindingProviderExtensions
	{
		/// <summary>
		///   Sets exchange to exchange routing key
		/// </summary>
		public static IRabbitMQExchangeBindingProvider WithRoutingKey(
			this IRabbitMQExchangeBindingProvider builder,
			string routingKey)
		{
			builder.Metadata.RoutingKey = routingKey;

			return builder;
		}

		/// <summary>
		///   Sets nowait for exchange to exchange declaration
		/// </summary>
		public static IRabbitMQExchangeBindingProvider NoWait(
			this IRabbitMQExchangeBindingProvider builder)
		{
			builder.Metadata.NoWait = true;

			return builder;
		}

		/// <summary>
		///   Sets RabbitMQ arguments. Allow multiple
		/// </summary>
		public static IRabbitMQExchangeBindingProvider WithArgument<TValue>(
			this IRabbitMQExchangeBindingProvider builder,
			string argument,
			TValue value)
		{
			builder.Metadata.Arguments.Add(argument, value);

			return builder;
		}
	}
}