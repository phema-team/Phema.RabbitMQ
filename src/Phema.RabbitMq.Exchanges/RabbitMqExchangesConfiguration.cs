using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangesConfiguration
	{
		IRabbitMQExchangeConfiguration AddExchange(string exchangeType, string exchangeName);
	}

	internal sealed class RabbitMQExchangesConfiguration : IRabbitMQExchangesConfiguration
	{
		private readonly IServiceCollection services;

		public RabbitMQExchangesConfiguration(IServiceCollection services)
		{
			this.services = services;
		}

		public IRabbitMQExchangeConfiguration AddExchange(string exchangeType, string exchangeName)
		{
			var exchange = new RabbitMQExchange(exchangeType, exchangeName);

			services.Configure<RabbitMQExchangesOptions>(options =>
				options.Exchanges.Add(exchange));

			return new RabbitMQExchangeConfiguration(exchange);
		}
	}
}