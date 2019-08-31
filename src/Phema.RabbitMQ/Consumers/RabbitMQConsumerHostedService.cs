using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQConsumerHostedService : IHostedService
	{
		private readonly RabbitMQOptions options;
		private readonly IServiceProvider serviceProvider;
		private readonly IRabbitMQConnectionCache connectionCache;

		public RabbitMQConsumerHostedService(
			IServiceProvider serviceProvider,
			IOptions<RabbitMQOptions> options,
			IRabbitMQConnectionCache connectionCache)
		{
			this.options = options.Value;
			this.serviceProvider = serviceProvider;
			this.connectionCache = connectionCache;
		}

		public Task StartAsync(CancellationToken token)
		{
			foreach (var declaration in options.ConsumerDeclarations)
			{
				var channel = connectionCache.FromDeclaration(declaration.Connection).CreateModel();

				// PrefetchSize != 0 is not implemented for now
				channel.BasicQos(0, declaration.PrefetchCount, declaration.Global);

				for (var index = 0; index < declaration.Count; index++)
				{
					channel.BasicConsume(
						queue: declaration.Queue.Name,
						autoAck: declaration.AutoAck,
						consumerTag:  $"{declaration.Tag}_{index}",
						noLocal: declaration.NoLocal,
						exclusive: declaration.Exclusive,
						arguments: declaration.Arguments,
						consumer: new RabbitMQConsumer(channel, options, serviceProvider, declaration, token));
				}
			}

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}