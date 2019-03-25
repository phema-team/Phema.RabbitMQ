using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Impl;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQQueueHostedService : IHostedService
	{
		private readonly RabbitMQQueuesOptions options;
		private readonly IRabbitMQConnectionFactory connectionFactory;

		public RabbitMQQueueHostedService(IOptions<RabbitMQQueuesOptions> options, IRabbitMQConnectionFactory connectionFactory)
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
						EnsureQueueDeleted(channel, declaration);
					}
					else
					{
						EnsureQueueDeclared(channel, declaration);
						
						foreach (var binding in declaration.QueueBindings)
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
		
		private static void EnsureQueueDeleted(IFullModel channel, IRabbitMQQueueDeclaration declaration)
		{
			try
			{
				channel._Private_QueueDelete(declaration.QueueName, declaration.IfUnused, declaration.IfEmpty, declaration.NoWait);
			}
			catch (OperationInterruptedException) when (declaration.IfUnused || declaration.IfEmpty)
			{
				// RabbitMQ.Client does not ignore PRECONDITION_FAILED
				// Means that queue is used or not empty, so just ignore exception
				// TODO: Some logging
			}
		}

		private static void EnsureQueueDeclared(IModel channel, IRabbitMQQueueDeclaration declaration)
		{
			if (declaration.NoWait)
			{
				channel.QueueDeclareNoWait(
					queue: declaration.QueueName,
					durable: declaration.Durable,
					exclusive: declaration.Exclusive,
					autoDelete: declaration.AutoDelete,
					arguments: declaration.Arguments);
			}
			else
			{
				channel.QueueDeclare(
					queue: declaration.QueueName,
					durable: declaration.Durable,
					exclusive: declaration.Exclusive,
					autoDelete: declaration.AutoDelete,
					arguments: declaration.Arguments);
			}
		}
		
		private static void EnsureQueueBinding(
			IModel channel,
			IRabbitMQQueueDeclaration declaration,
			IRabbitMQQueueBindingDeclaration binding)
		{
			if (binding.Deleted)
			{
				channel.QueueUnbind(
					declaration.QueueName,
					binding.ExchangeName,
					binding.RoutingKey ?? declaration.QueueName,
					binding.Arguments);
			}
			else
			{
				if (binding.NoWait)
				{
					channel.QueueBindNoWait(
						declaration.QueueName,
						binding.ExchangeName,
						binding.RoutingKey ?? declaration.QueueName,
						binding.Arguments);
				}
				else
				{
					channel.QueueBind(
						declaration.QueueName,
						binding.ExchangeName,
						binding.RoutingKey ?? declaration.QueueName,
						binding.Arguments);
				}
			}
		}
	}
}