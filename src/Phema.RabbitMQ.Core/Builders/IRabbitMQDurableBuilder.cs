namespace Phema.RabbitMQ
{
	public interface IRabbitMQDurableBuilder<TBuilder>
	{
		/// <summary>
		/// Sets durability 
		/// </summary>
		TBuilder Durable();
	}

	public interface IRabbitMQAutoDeleteBuilder<TBuilder>
	{
		/// <summary>
		/// Sets auto delete 
		/// </summary>
		TBuilder AutoDelete();
	}

	public interface IRabbitMQExclusiveBuilder<TBuilder>
	{
		/// <summary>
		/// Sets exclusive 
		/// </summary>
		TBuilder Exclusive();
	}

	public interface IRabbitMQRoutingKeyBuilder<TBuilder>
	{
		/// <summary>
		/// Sets routing key
		/// </summary>
		TBuilder WithRoutingKey(string routingKey);
	}

	public interface IRabbitMQWithArgumentBuilder<TBuilder>
	{
		/// <summary>
		/// Sets RabbitMQ argument
		/// </summary>
		TBuilder WithArgument<TValue>(string argument, TValue value);
	}
}