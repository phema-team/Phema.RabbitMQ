using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQBuilder
	{
		IServiceCollection Services { get; }
		IRabbitMQConnectionFactory ConnectionFactory { get; }
	}

	internal sealed class RabbitMQBuilder : IRabbitMQBuilder
	{
		public RabbitMQBuilder(
			IServiceCollection services,
			IRabbitMQConnectionFactory connectionFactory)
		{
			Services = services;
			ConnectionFactory = connectionFactory;
		}

		public IServiceCollection Services { get; }
		public IRabbitMQConnectionFactory ConnectionFactory { get; }
	}
}