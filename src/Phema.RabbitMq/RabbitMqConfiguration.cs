using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMq
{
	public interface IRabbitMqConfiguration
	{
		IServiceCollection Services { get; }
	}

	internal sealed class RabbitMqConfiguration : IRabbitMqConfiguration
	{
		public RabbitMqConfiguration(IServiceCollection services)
		{
			Services = services;
		}
		
		public IServiceCollection Services { get; }
	}
}