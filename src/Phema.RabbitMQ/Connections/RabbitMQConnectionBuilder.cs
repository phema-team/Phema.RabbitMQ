using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConnectionBuilder
	{
		IServiceCollection Services { get; }
		RabbitMQConnectionDeclaration ConnectionDeclaration { get; }
	}

	internal sealed class RabbitMQConnectionBuilder : IRabbitMQConnectionBuilder
	{
		public RabbitMQConnectionBuilder(IServiceCollection services, RabbitMQConnectionDeclaration connectionDeclaration)
		{
			Services = services;
			ConnectionDeclaration = connectionDeclaration;
		}

		public IServiceCollection Services { get; }
		public RabbitMQConnectionDeclaration ConnectionDeclaration { get; }
	}
}