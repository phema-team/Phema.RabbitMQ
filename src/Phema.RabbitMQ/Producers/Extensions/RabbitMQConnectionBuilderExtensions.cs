using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public static partial class RabbitMQConnectionBuilderExtensions
	{
		public static IRabbitMQProducerBuilder<TPayload> AddProducer<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			IRabbitMQExchangeBuilder<TPayload> exchange)
		{
			var declaration = new RabbitMQProducerDeclaration(
				typeof(TPayload),
				connection.Declaration,
				exchange.Declaration);

			connection.Services
				.Configure<RabbitMQOptions>(options => options.ProducerDeclarations.Add(declaration));

			return new RabbitMQProducerBuilder<TPayload>(declaration);
		}
	}
}