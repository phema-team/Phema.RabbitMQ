using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public static partial class RabbitMQConnectionBuilderExtensions
	{
		public static IRabbitMQConsumerBuilder<TPayload> AddConsumer<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			params IRabbitMQQueueBuilder<TPayload>[] queues)
		{
			var declaration = new RabbitMQConsumerDeclaration(
				typeof(TPayload),
				connection.Declaration,
				queues.Select(queue => queue.Declaration).ToArray());

			connection.Services
				.Configure<RabbitMQOptions>(
					options => options.ConsumerDeclarations.Add(declaration));

			return new RabbitMQConsumerBuilder<TPayload>(declaration);
		}
	}
}