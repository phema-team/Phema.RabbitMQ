using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQConnectionHostedService : IHostedService
	{
		private readonly RabbitMQOptions options;
		private readonly IRabbitMQConnectionProvider connectionProvider;
		private readonly IRabbitMQChannelProvider channelProvider;

		public RabbitMQConnectionHostedService(
			IOptions<RabbitMQOptions> options,
			IRabbitMQConnectionProvider connectionProvider,
			IRabbitMQChannelProvider channelProvider)
		{
			this.options = options.Value;
			this.connectionProvider = connectionProvider;
			this.channelProvider = channelProvider;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			foreach (var declaration in options.ConnectionDeclarations)
			{
				connectionProvider.FromDeclaration(declaration);
			}

			foreach (var declaration in options.ProducerDeclarations)
			{
				channelProvider.FromDeclaration(declaration);
			}

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}