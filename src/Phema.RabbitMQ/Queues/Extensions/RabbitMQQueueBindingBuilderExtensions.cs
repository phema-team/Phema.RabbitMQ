namespace Phema.RabbitMQ
{
	public static class RabbitMQQueueBindingBuilderExtensions
	{
		/// <summary>
		///   Declare exchange to exchange routing key
		/// </summary>
		public static IRabbitMQQueueBindingBuilder RoutingKey(
			this IRabbitMQQueueBindingBuilder builder,
			string routingKey)
		{
			builder.Declaration.RoutingKey = routingKey;

			return builder;
		}

		/// <summary>
		///   Nowait for exchange to queue declaration
		/// </summary>
		public static IRabbitMQQueueBindingBuilder NoWait(this IRabbitMQQueueBindingBuilder builder)
		{
			builder.Declaration.NoWait = true;

			return builder;
		}

		/// <summary>
		///   Delete queue binding
		/// </summary>
		public static IRabbitMQQueueBindingBuilder Deleted(this IRabbitMQQueueBindingBuilder builder)
		{
			builder.Declaration.Deleted = true;

			return builder;
		}

		/// <summary>
		///   Declare RabbitMQ arguments. Allow multiple
		/// </summary>
		public static IRabbitMQQueueBindingBuilder Argument<TValue>(
			this IRabbitMQQueueBindingBuilder builder,
			string argument,
			TValue value)
		{
			builder.Declaration.Arguments.Add(argument, value);

			return builder;
		}
	}
}