using System;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	public static class RabbitExtensions
	{
		public static IRabbitBuilder AddRabbit(this IServiceCollection services)
		{
			services.AddHostedService<RabbitHostedService>();
			services.ConfigureOptions<ConnectionFactoryPostConfigureOptions>();
			return new RabbitBuilder(services);
		}
		
		public static IRabbitBuilder AddRabbit(this IServiceCollection services, Action<RabbitOptions> action)
		{
			services.Configure(action);
			return services.AddRabbit();
		}
	}
}