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
		private readonly string groupName;
		private readonly IServiceCollection services;

		public RabbitMQExchangesBuilder(IServiceCollection services, string groupName)
		{
			this.services = services;
			this.groupName = groupName;
		}

		public IRabbitMQExchangeBuilder AddExchange(string exchangeType, string exchangeName)
		{
			if (exchangeType is null)
				throw new ArgumentNullException(nameof(exchangeType));

			if (exchangeName is null)
				throw new ArgumentNullException(nameof(exchangeName));

			exchangeName = groupName == RabbitMQDefaults.DefaultGroupName
				? exchangeName
				: $"{groupName}.{exchangeName}";

			var declaration = new RabbitMQExchangeDeclaration(exchangeType, exchangeName);

			services.Configure<RabbitMQExchangesOptions>(options =>
			{
				if (options.Exchanges.Any(e => e.Name == exchangeName))
					throw new ArgumentException($"Exchange {exchangeName} already registered", nameof(exchangeName));

				options.Exchanges.Add(declaration);
			});

			return new RabbitMQExchangeBuilder(declaration);
		}
	}
}