namespace Phema.RabbitMq
{
	public interface IRabbitMqExchangeConfiguration
	{
		IRabbitMqExchangeConfiguration Durable();
		IRabbitMqExchangeConfiguration AutoDelete();
		IRabbitMqExchangeConfiguration WithArgument(string argument, string value);
	}

	internal sealed class RabbitMqExchangeConfiguration : IRabbitMqExchangeConfiguration
	{
		private readonly RabbitMqExchange exchange;

		public RabbitMqExchangeConfiguration(RabbitMqExchange exchange)
		{
			this.exchange = exchange;
		}

		public IRabbitMqExchangeConfiguration Durable()
		{
			exchange.Durable = true;
			return this;
		}

		public IRabbitMqExchangeConfiguration AutoDelete()
		{
			exchange.AutoDelete = true;
			return this;
		}

		public IRabbitMqExchangeConfiguration WithArgument(string argument, string value)
		{
			exchange.Arguments.Add(argument, value);
			return this;
		}
	}
}