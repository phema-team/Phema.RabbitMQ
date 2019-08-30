using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQConnectionHostedService : IHostedService
	{
		private readonly RabbitMQOptions options;
		private readonly IRabbitMQConnectionCache connectionCache;
		private readonly IRabbitMQChannelCache channelCache;

		public RabbitMQConnectionHostedService(
			IOptions<RabbitMQOptions> options,
			IRabbitMQConnectionCache connectionCache,
			IRabbitMQChannelCache channelCache)
		{
			this.options = options.Value;
			this.connectionCache = connectionCache;
			this.channelCache = channelCache;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			foreach (var declaration in options.ConnectionDeclarations)
			{
				connectionCache.FromDeclaration(declaration);
			}

			foreach (var declaration in options.ProducerDeclarations)
			{
				channelCache.FromDeclaration(declaration);
			}

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}