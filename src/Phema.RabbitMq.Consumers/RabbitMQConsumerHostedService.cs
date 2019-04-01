using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Impl;

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQConsumerHostedService<TPayload, TPayloadConsumer> : IHostedService
		where TPayloadConsumer : IRabbitMQConsumer<TPayload>
	{
		private readonly RabbitMQConsumersOptions options;
		private readonly IRabbitMQConsumerFactory consumerFactory;
		private readonly IRabbitMQConnectionFactory connectionFactory;

		public RabbitMQConsumerHostedService(
			IOptions<RabbitMQConsumersOptions> options,
			IRabbitMQConsumerFactory consumerFactory,
			IRabbitMQConnectionFactory connectionFactory)
		{
			this.options = options.Value;
			this.consumerFactory = consumerFactory;
			this.connectionFactory = connectionFactory;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			var declarations = options.Declarations.OfType<RabbitMQConsumerDeclaration<TPayload, TPayloadConsumer>>();

			foreach (var declaration in declarations)
			{
				var channel = (IFullModel) connectionFactory.CreateConnection(declaration.GroupName).CreateModel();

				EnsurePrefetchCount(channel, declaration);

				for (var index = 0; index < declaration.Count; index++)
				{
					channel.BasicConsume(
						queue: declaration.QueueName,
						autoAck: declaration.AutoAck,
						consumerTag: declaration.Tag,
						noLocal: declaration.NoLocal,
						exclusive: declaration.Exclusive,
						arguments: declaration.Arguments,
						consumer: consumerFactory.CreateConsumer<TPayload, TPayloadConsumer>(channel, declaration));
				}
			}

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
		
		private static void EnsurePrefetchCount(IModel channel, IRabbitMQConsumerDeclaration consumer)
		{
			// PrefetchSize != 0 is not implemented for now
			channel.BasicQos(0, consumer.PrefetchCount, consumer.Global);
		}
	}
}