using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQQueueHostedService : IHostedService
	{
		private readonly RabbitMQOptions options;
		private readonly IRabbitMQConnectionCache connectionCache;

		public RabbitMQQueueHostedService(IOptions<RabbitMQOptions> options, IRabbitMQConnectionCache connectionCache)
		{
			this.options = options.Value;
			this.connectionCache = connectionCache;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			foreach (var declaration in options.QueueDeclarations)
			{
				using (var channel = connectionCache.FromDeclaration(declaration.Connection).CreateModel())
				{
					if (declaration.Deleted)
					{
						EnsureQueueDeleted(channel, declaration);
					}
					else
					{
						EnsureQueueDeclared(channel, declaration);

						foreach (var binding in declaration.Bindings)
						{
							EnsureQueueBinding(channel, declaration, binding);
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

		private static void EnsureQueueDeleted(IModel channel, RabbitMQQueueDeclaration declaration)
		{
			try
			{
				if (declaration.NoWait)
				{
					channel.QueueDeleteNoWait(
						declaration.Name,
						declaration.IfUnused,
						declaration.IfEmpty);
				}
				else
				{
					channel.QueueDelete(
						declaration.Name,
						declaration.IfUnused,
						declaration.IfEmpty);
				}
			}
			catch (OperationInterruptedException) when (declaration.IfUnused || declaration.IfEmpty)
			{
				// RabbitMQ.Client does not ignore PRECONDITION_FAILED
				// Means that queue is used or not empty, so just ignore exception
				// TODO: Some logging
			}
		}

		private static void EnsureQueueDeclared(IModel channel, RabbitMQQueueDeclaration declaration)
		{
			if (declaration.NoWait)
			{
				channel.QueueDeclareNoWait(
					queue: declaration.Name,
					durable: declaration.Durable,
					exclusive: declaration.Exclusive,
					autoDelete: declaration.AutoDelete,
					arguments: declaration.Arguments);
			}
			else
			{
				channel.QueueDeclare(
					queue: declaration.Name,
					durable: declaration.Durable,
					exclusive: declaration.Exclusive,
					autoDelete: declaration.AutoDelete,
					arguments: declaration.Arguments);
			}
		}

		private static void EnsureQueueBinding(
			IModel channel,
			RabbitMQQueueDeclaration declaration,
			RabbitMQQueueBindingDeclaration binding)
		{
			var routingKey = binding.RoutingKey ?? declaration.Name;

			if (binding.Deleted)
			{
				channel.QueueUnbind(
					declaration.Name,
					binding.Exchange.Name,
					routingKey,
					binding.Arguments);
			}
			else
			{
				if (binding.NoWait)
				{
					channel.QueueBindNoWait(
						declaration.Name,
						binding.Exchange.Name,
						routingKey,
						binding.Arguments);
				}
				else
				{
					channel.QueueBind(
						declaration.Name,
						binding.Exchange.Name,
						routingKey,
						binding.Arguments);
				}
			}
		}
	}
}