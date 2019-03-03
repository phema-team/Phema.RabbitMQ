using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMq
{
	public interface IRabbitMqExchangesConfiguration
	{
		IRabbitMqExchangeConfiguration AddExchange(string exchangeType, string exchangeName);
	}

	internal sealed class RabbitMqExchangesConfiguration : IRabbitMqExchangesConfiguration
	{
		private readonly IServiceCollection services;

		public RabbitMqExchangesConfiguration(IServiceCollection services)
		{
			this.services = services;
		}

		public IRabbitMqExchangeConfiguration AddExchange(string exchangeType, string exchangeName)
		{
			var exchange = new RabbitMqExchange(exchangeType, exchangeName);

			services.Configure<RabbitMqExchangesOptions>(options =>
				options.Exchanges.Add(exchange));

			return new RabbitMqExchangeConfiguration(exchange);
		}
	}
}