using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public static partial class RabbitMQConnectionBuilderExtensions
	{
		public static IRabbitMQConsumerBuilder<TPayload> AddConsumer<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			IRabbitMQQueueBuilder<TPayload> queue,
			Func<IServiceScope, TPayload, CancellationToken, ValueTask> consumer)
		{
			var declaration = new RabbitMQConsumerDeclaration(
				typeof(TPayload),
				connection.Declaration,
				queue.Declaration,
				(scope, payload, token) => consumer(scope, (TPayload)payload, token));

			connection.Services
				.Configure<RabbitMQOptions>(options => options.ConsumerDeclarations.Add(declaration));

			return new RabbitMQConsumerBuilder<TPayload>(declaration);
		}
		
		public static IRabbitMQConsumerBuilder<TPayload> AddConsumer<TPayload>(
			this IRabbitMQConnectionBuilder connection,
			IRabbitMQQueueBuilder<TPayload> queue,
			Func<IServiceScope, TPayload, ValueTask> consumer)
		{
			return connection.AddConsumer(queue, (scope, payload, token) => consumer(scope, payload));
		}
	}
}