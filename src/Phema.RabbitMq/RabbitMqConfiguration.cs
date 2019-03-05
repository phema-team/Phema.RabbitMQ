using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConfiguration
	{
		IServiceCollection Services { get; }
	}

	internal sealed class RabbitMQConfiguration : IRabbitMQConfiguration
	{
		public RabbitMQConfiguration(IServiceCollection services)
		{
			Services = services;
		}

		public IServiceCollection Services { get; }
	}
}