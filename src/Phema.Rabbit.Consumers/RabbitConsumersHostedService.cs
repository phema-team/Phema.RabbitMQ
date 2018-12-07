using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	internal sealed class RabbitConsumersHostedService : IHostedService
	{
		private readonly IConnection connection;
		private readonly IServiceProvider provider;

		public RabbitConsumersHostedService(IServiceProvider provider, IConnection connection)
		{
			this.provider = provider;
			this.connection = connection;
		}
		
		public Task StartAsync(CancellationToken cancellationToken)
		{
			var options = provider.GetRequiredService<IOptions<RabbitConsumerOptions>>().Value;

			foreach (var consumer in options.ConsumerActions)
			{
				consumer(provider, connection);
			}
			
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