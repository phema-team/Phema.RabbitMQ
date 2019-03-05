namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeConfiguration
	{
		IRabbitMQExchangeConfiguration Durable();
		IRabbitMQExchangeConfiguration AutoDelete();
		IRabbitMQExchangeConfiguration WithArgument(string argument, string value);
	}

	internal sealed class RabbitMQExchangeConfiguration : IRabbitMQExchangeConfiguration
	{
		private readonly RabbitMQExchange exchange;

		public RabbitMQExchangeConfiguration(RabbitMQExchange exchange)
		{
			this.exchange = exchange;
		}

		public IRabbitMQExchangeConfiguration Durable()
		{
			exchange.Durable = true;
			return this;
		}

		public IRabbitMQExchangeConfiguration AutoDelete()
		{
			exchange.AutoDelete = true;
			return this;
		}

		public IRabbitMQExchangeConfiguration WithArgument(string argument, string value)
		{
			exchange.Arguments.Add(argument, value);
			return this;
		}
	}
}