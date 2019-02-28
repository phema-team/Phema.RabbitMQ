using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMq
{
	public static class RabbitMqExchangesExtensions
	{
		public static IRabbitMqConfiguration AddExchanges(
			this IRabbitMqConfiguration configuration,
			Action<IRabbitMqExchangesConfiguration> options)
		{
			options(new RabbitMqExchangesConfiguration(configuration.Services));
			return configuration;
		}
	}
}