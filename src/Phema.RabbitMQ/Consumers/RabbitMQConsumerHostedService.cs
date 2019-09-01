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
		private readonly IRabbitMQConnectionProvider connectionProvider;

		public RabbitMQConsumerHostedService(
			IServiceProvider serviceProvider,
			IOptions<RabbitMQOptions> options,
			IRabbitMQConnectionProvider connectionProvider)
		{
			this.options = options.Value;
			this.serviceProvider = serviceProvider;
			this.connectionProvider = connectionProvider;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			foreach (var declaration in options.ConsumerDeclarations)
			{
				foreach (var queueDeclaration in declaration.QueueDeclarations)
				{
					var channel = await connectionProvider
						.FromDeclaration(declaration.ConnectionDeclaration)
						.CreateChannelAsync();

					channel.BasicQos(declaration);

					channel.BasicConsume(
						serviceProvider,
						options,
						queueDeclaration,
						declaration,
						cancellationToken);
				}
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}