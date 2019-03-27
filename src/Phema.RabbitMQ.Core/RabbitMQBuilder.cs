using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQBuilder
	{
		IServiceCollection Services { get; }
	}
}

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQBuilder : IRabbitMQBuilder
	{
		public RabbitMQBuilder(IServiceCollection services)
		{
			Services = services;
		}

		public IServiceCollection Services { get; }
	}
}