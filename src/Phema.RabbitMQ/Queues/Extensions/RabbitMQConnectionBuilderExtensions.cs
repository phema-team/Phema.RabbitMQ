using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public static partial class RabbitMQConnectionBuilderExtensions
	{
		public static IRabbitMQQueueBuilder<TPayload> AddQueue<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			string queueName)
		{
			var declaration = new RabbitMQQueueDeclaration(connection.ConnectionDeclaration, queueName);

			connection.Services
				.Configure<RabbitMQOptions>(
					options => options.QueueDeclarations.Add(declaration));

			return new RabbitMQQueueBuilder<TPayload>(declaration);
		}
	}
}