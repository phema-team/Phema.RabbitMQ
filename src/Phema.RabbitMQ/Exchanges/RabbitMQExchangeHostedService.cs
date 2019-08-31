using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Impl;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQExchangeHostedService : IHostedService
	{
		private readonly RabbitMQOptions options;
		private readonly IRabbitMQChannelProvider channelProvider;

		public RabbitMQExchangeHostedService(
			IOptions<RabbitMQOptions> options,
			IRabbitMQChannelProvider channelProvider)
		{
			this.options = options.Value;
			this.channelProvider = channelProvider;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			foreach (var declaration in options.ExchangeDeclarations)
			{
				using (var channel = channelProvider.FromDeclaration(declaration))
				{
					if (declaration.Deleted)
					{
						EnsureExchangeDeleted(channel, declaration);
					}
					else
					{
						EnsureExcnahgeDeclared(channel, declaration);

						foreach (var binding in declaration.Bindings)
						{
							EnsureExchangeBindings(channel, declaration, binding);
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

		private static void EnsureExchangeDeleted(IModel channel, RabbitMQExchangeDeclaration declaration)
		{
			if (declaration.NoWait)
			{
				channel.ExchangeDeleteNoWait(
					declaration.Name,
					declaration.UnusedOnly);
			}
			else
			{
				channel.ExchangeDelete(
					declaration.Name,
					declaration.UnusedOnly);
			}
		}

		private static void EnsureExcnahgeDeclared(IModel channel, RabbitMQExchangeDeclaration declaration)
		{
			if (declaration.Internal)
			{
				((IFullModel)channel)._Private_ExchangeDeclare(
					exchange: declaration.Name,
					type: declaration.Type,
					passive: false,
					durable: declaration.Durable,
					autoDelete: declaration.AutoDelete,
					@internal: declaration.Internal,
					nowait: declaration.NoWait,
					arguments: declaration.Arguments);
			}
			else
			{
				if (declaration.NoWait)
				{
					channel.ExchangeDeclareNoWait(
						exchange: declaration.Name,
						type: declaration.Type,
						durable: declaration.Durable,
						autoDelete: declaration.AutoDelete,
						arguments: declaration.Arguments);
				}
				else
				{
					channel.ExchangeDeclare(
						exchange: declaration.Name,
						type: declaration.Type,
						durable: declaration.Durable,
						autoDelete: declaration.AutoDelete,
						arguments: declaration.Arguments);
				}
			}
		}

		private static void EnsureExchangeBindings(
			IModel channel,
			RabbitMQExchangeDeclaration exchange,
			RabbitMQExchangeBindingDeclaration binding)
		{
			var routingKey = binding.RoutingKey ?? binding.Exchange.Name;

			if (binding.Deleted)
			{
				if (binding.NoWait)
				{
					channel.ExchangeUnbindNoWait(
						destination: exchange.Name,
						source: binding.Exchange.Name,
						routingKey: routingKey,
						arguments: binding.Arguments);
				}
				else
				{
					channel.ExchangeUnbind(
						destination: exchange.Name,
						source: binding.Exchange.Name,
						routingKey: routingKey,
						arguments: binding.Arguments);
				}
			}
			else
			{
				if (binding.NoWait)
				{
					channel.ExchangeBindNoWait(
						destination: exchange.Name,
						source: binding.Exchange.Name,
						routingKey: routingKey,
						arguments: binding.Arguments);
				}
				else
				{
					channel.ExchangeBind(
						destination: exchange.Name,
						source: binding.Exchange.Name,
						routingKey: routingKey,
						arguments: binding.Arguments);
				}
			}
		}
	}
}