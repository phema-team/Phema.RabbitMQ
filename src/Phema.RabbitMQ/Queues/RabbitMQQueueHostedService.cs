using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQQueueHostedService : IHostedService
	{
		private readonly RabbitMQOptions options;
		private readonly IRabbitMQConnectionProvider connectionProvider;

		public RabbitMQQueueHostedService(
			IOptions<RabbitMQOptions> options,
			IRabbitMQConnectionProvider connectionProvider)
		{
			this.options = options.Value;
			this.connectionProvider = connectionProvider;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			foreach (var declaration in options.QueueDeclarations)
			{
				using (var channel = await connectionProvider
					.FromDeclaration(declaration.ConnectionDeclaration)
					.CreateChannelAsync())
				{
					if (declaration.Deleted)
					{
						channel.QueueDelete(declaration);
					}
					else
					{
						channel.QueueDeclare(declaration);

						foreach (var binding in declaration.BindingDeclarations)
						{
							if (binding.Deleted)
							{
								channel.QueueUnbind(declaration, binding);
							}
							else
							{
								channel.QueueBind(declaration, binding);
							}
						}
					}
				}
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}