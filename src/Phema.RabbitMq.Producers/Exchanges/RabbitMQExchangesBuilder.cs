using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangesBuilder
	{
		/// <summary>
		///   Register new exchange
		/// </summary>
		IRabbitMQExchangeBuilder AddExchange(string exchangeType, string exchangeName);
	}

	public class RabbitMQExchangesBuilder : IRabbitMQExchangesBuilder
	{
		private readonly IServiceCollection services;

		public RabbitMQExchangesBuilder(IServiceCollection services)
		{
			this.services = services;
		}

		public IRabbitMQExchangeBuilder AddExchange(string exchangeType, string exchangeName)
		{
			if (exchangeType is null)
				throw new ArgumentNullException(nameof(exchangeType));

			if (exchangeName is null)
				throw new ArgumentNullException(nameof(exchangeName));

			var metadata = new RabbitMQExchangeMetadata(exchangeType, exchangeName);

			services.Configure<RabbitMQExchangesOptions>(options =>
			{
				if (options.Exchanges.Any(e => e.Name == exchangeName))
					throw new ArgumentException($"Exchange {exchangeName} already registered", nameof(exchangeName));

				options.Exchanges.Add(metadata);
			});

			return new RabbitMQExchangeBuilder(metadata);
		}
	}
}