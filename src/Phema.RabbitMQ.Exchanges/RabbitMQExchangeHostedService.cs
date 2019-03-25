using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Impl;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQExchangeHostedService : IHostedService
	{
		private readonly RabbitMQExchangesOptions options;
		private readonly IRabbitMQConnectionFactory connectionFactory;

		public RabbitMQExchangeHostedService(IOptions<RabbitMQExchangesOptions> options,
			IRabbitMQConnectionFactory connectionFactory)
		{
			this.options = options.Value;
			this.connectionFactory = connectionFactory;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			foreach (var declaration in options.Declarations)
			{
				using (var channel = (IFullModel) connectionFactory.CreateConnection(declaration.GroupName).CreateModel())
				{
					if (declaration.Deleted)
					{
						EnsureExchangeDeleted(channel, declaration);
					}
					else
					{
						EnsureExcnahgeDeclared(channel, declaration);

						foreach (var binding in declaration.ExchangeBindings)
						{
							EnsureExchangeBinding(channel, declaration, binding);
						}
					}
				}
			}

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		private static void EnsureExchangeDeleted(IFullModel channel, IRabbitMQExchangeDeclaration declaration)
		{
			channel._Private_ExchangeDelete(declaration.ExchangeName, declaration.IfUnused, declaration.NoWait);
		}

		private static void EnsureExcnahgeDeclared(IFullModel channel, IRabbitMQExchangeDeclaration declaration)
		{
			channel._Private_ExchangeDeclare(
				exchange: declaration.ExchangeName,
				type: declaration.ExchangeType,
				passive: false,
				durable: declaration.Durable,
				autoDelete: declaration.AutoDelete,
				@internal: declaration.Internal,
				nowait: declaration.NoWait,
				arguments: declaration.Arguments);
		}

		private static void EnsureExchangeBinding(
			IFullModel channel,
			IRabbitMQExchangeDeclaration exchange,
			IRabbitMQExchangeBindingDeclaration binding)
		{
			if (binding.Deleted)
			{
				channel._Private_ExchangeUnbind(
					destination: exchange.ExchangeName,
					source: binding.ExchangeName,
					routingKey: binding.RoutingKey ?? binding.ExchangeName,
					nowait: binding.NoWait,
					arguments: binding.Arguments);
			}
			else
			{
				channel._Private_ExchangeBind(
					destination: exchange.ExchangeName,
					source: binding.ExchangeName,
					routingKey: binding.RoutingKey ?? binding.ExchangeName,
					nowait: binding.NoWait,
					arguments: binding.Arguments);
			}
		}
	}
}