using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	internal sealed class RabbitHostedService : IHostedService
	{
		private IConnection connection;
		private readonly IServiceProvider provider;

		public RabbitHostedService(IServiceProvider provider)
		{
			this.provider = provider;
		}
		
		public Task StartAsync(CancellationToken cancellationToken)
		{
			var connectionFactory = provider.GetRequiredService<IOptions<ConnectionFactory>>().Value;
			connection = connectionFactory.CreateConnection();

			var options = provider.GetRequiredService<IOptions<RabbitOptions>>().Value;

			foreach (var action in options.Actions)
			{
				action(provider, connection);
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