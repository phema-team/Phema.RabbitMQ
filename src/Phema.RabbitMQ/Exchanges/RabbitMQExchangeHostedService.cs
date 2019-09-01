using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQExchangeHostedService : IHostedService
	{
		private readonly RabbitMQOptions options;
		private readonly IRabbitMQConnectionProvider connectionProvider;

		public RabbitMQExchangeHostedService(
			IOptions<RabbitMQOptions> options,
			IRabbitMQConnectionProvider connectionProvider)
		{
			this.options = options.Value;
			this.connectionProvider = connectionProvider;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			foreach (var declaration in options.ExchangeDeclarations)
			{
				using (var channel = await connectionProvider
					.FromDeclaration(declaration.ConnectionDeclaration)
					.CreateChannelAsync())
				{
					if (declaration.Deleted)
					{
						channel.ExchangeDelete(declaration);
					}
					else
					{
						channel.ExchangeDeclare(declaration);

						foreach (var binding in declaration.BindingDeclarations)
						{
							if (binding.Deleted)
							{
								channel.ExchangeUnbind(declaration, binding);
							}
							else
							{
								channel.ExchangeBind(declaration, binding);
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