using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	public static class RabbitExtensions
	{
		public static IRabbitBuilder AddRabbit(this IServiceCollection services)
		{
			services.ConfigureOptions<ConnectionFactoryPostConfigure>();

			services.TryAddSingleton(provider =>
			{
				var factory = provider.GetRequiredService<IOptions<ConnectionFactory>>().Value;

				var options = provider.GetRequiredService<IOptions<RabbitOptions>>().Value;

				return options.InstanceName == null
					? factory.CreateConnection()
					: factory.CreateConnection(options.InstanceName);
			});
			
			return new RabbitBuilder(services);
		}
		
		public static IRabbitBuilder AddRabbit(this IServiceCollection services, Action<RabbitOptions> action)
		{
			services.Configure(action);
			return services.AddRabbit();
		}
	}
}