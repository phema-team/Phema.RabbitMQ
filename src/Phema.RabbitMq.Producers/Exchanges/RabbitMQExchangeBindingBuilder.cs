namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeBindingBuilder
		: IRabbitMQRoutingKeyBuilder<IRabbitMQExchangeBindingBuilder>,
			IRabbitMQWithArgumentBuilder<IRabbitMQExchangeBindingBuilder>
	{
	}

	internal sealed class RabbitMQExchangeBindingBuilder : IRabbitMQExchangeBindingBuilder
	{
		private readonly RabbitMQExchangeBinding binding;

		public RabbitMQExchangeBindingBuilder(RabbitMQExchangeBinding binding)
		{
			this.binding = binding;
		}

		public IRabbitMQExchangeBindingBuilder WithRoutingKey(string routingKey)
		{
			binding.RoutingKey = routingKey;
			return this;
		}

		public IRabbitMQExchangeBindingBuilder WithArgument<TValue>(string argument, TValue value)
		{
			binding.Arguments.Add(argument, value);
			return this;
		}
	}
}