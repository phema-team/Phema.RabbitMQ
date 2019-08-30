using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConnectionBuilder
	{
		IServiceCollection Services { get; }
		RabbitMQConnectionDeclaration Declaration { get; }
	}

	internal sealed class RabbitMQConnectionBuilder : IRabbitMQConnectionBuilder
	{
		public RabbitMQConnectionBuilder(IServiceCollection services, RabbitMQConnectionDeclaration declaration)
		{
			Services = services;
			Declaration = declaration;
		}

		public IServiceCollection Services { get; }
		public RabbitMQConnectionDeclaration Declaration { get; }
	}
}