using System;
using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeGroupBuilder
	{
		/// <summary>
		///   Register new exchange
		/// </summary>
		IRabbitMQExchangeBuilder AddExchange(string exchangeType, string exchangeName);
	}

	public class RabbitMQExchangeGroupBuilder : IRabbitMQExchangeGroupBuilder
	{
		private readonly string groupName;
		private readonly IServiceCollection services;

		public RabbitMQExchangeGroupBuilder(IServiceCollection services, string groupName)
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

			var declaration = new RabbitMQExchangeDeclaration(groupName, exchangeType, exchangeName);

			services.Configure<RabbitMQExchangesOptions>(options => options.Declarations.Add(declaration));

			return new RabbitMQExchangeBuilder(declaration);
		}
	}
}