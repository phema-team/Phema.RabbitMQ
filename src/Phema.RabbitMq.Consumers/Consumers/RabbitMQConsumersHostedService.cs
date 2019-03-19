using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQConsumersHostedService : IHostedService
	{
		private readonly IServiceProvider provider;

		public RabbitMQConsumersHostedService(IServiceProvider provider)
		{
			this.provider = provider;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			var options = provider.GetRequiredService<IOptions<RabbitMQConsumersOptions>>().Value;

			foreach (var dispatcher in options.ConsumerDispatchers)
				dispatcher(provider);

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}