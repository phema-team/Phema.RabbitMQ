using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQConsumersHostedService : IHostedService
	{
		private readonly IConnection connection;
		private readonly IServiceProvider provider;

		public RabbitMQConsumersHostedService(IServiceProvider provider, IConnection connection)
		{
			this.provider = provider;
			this.connection = connection;
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
			connection.Close();
			connection.Dispose();
			return Task.CompletedTask;
		}
	}
}