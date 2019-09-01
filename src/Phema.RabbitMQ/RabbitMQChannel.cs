using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Impl;

namespace Phema.RabbitMQ
{
	public class RabbitMQChannel : IDisposable
	{
		private readonly object @lock = new object();

		private readonly ILogger<RabbitMQChannel> logger;
		private readonly IFullModel channel;

		public RabbitMQChannel(
			RabbitMQConnectionDeclaration connectionDeclaration,
			IFullModel channel,
			ILogger<RabbitMQChannel> logger)
		{
			this.logger = logger;
			this.channel = channel;
			ConnectionDeclaration = connectionDeclaration;
		}

		public int ChannelNumber => channel.ChannelNumber;
		public RabbitMQConnectionDeclaration ConnectionDeclaration { get; }

		// Exchanges
		public void ExchangeDeclare(RabbitMQExchangeDeclaration declaration)
		{
			lock (@lock)
			{
				if (declaration.Internal)
				{
					channel._Private_ExchangeDeclare(
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
		}

		public void ExchangeBind(
			RabbitMQExchangeDeclaration declaration,
			RabbitMQExchangeBindingDeclaration binding)
		{
			var routingKey = binding.RoutingKey ?? declaration.Name;

			lock (@lock)
			{
				if (binding.NoWait)
				{
					channel.ExchangeBindNoWait(
						destination: declaration.Name,
						source: binding.ExchangeDeclaration.Name,
						routingKey: routingKey,
						arguments: binding.Arguments);
				}
				else
				{
					channel.ExchangeBind(
						destination: declaration.Name,
						source: binding.ExchangeDeclaration.Name,
						routingKey: routingKey,
						arguments: binding.Arguments);
				}
			}
		}

		public void ExchangeUnbind(
			RabbitMQExchangeDeclaration declaration,
			RabbitMQExchangeBindingDeclaration binding)
		{
			var routingKey = binding.RoutingKey ?? declaration.Name;

			lock (@lock)
			{
				if (binding.NoWait)
				{
					channel.ExchangeUnbindNoWait(
						destination: declaration.Name,
						source: binding.ExchangeDeclaration.Name,
						routingKey: routingKey,
						arguments: binding.Arguments);
				}
				else
				{
					channel.ExchangeUnbind(
						destination: declaration.Name,
						source: binding.ExchangeDeclaration.Name,
						routingKey: routingKey,
						arguments: binding.Arguments);
				}
			}
		}

		public void ExchangeDelete(RabbitMQExchangeDeclaration declaration)
		{
			lock (@lock)
			{
				// TODO: try-catch when unused only failed?
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
		}

		// Queues
		public void QueueDeclare(RabbitMQQueueDeclaration declaration)
		{
			lock (@lock)
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
		}

		public void QueueBind(RabbitMQQueueDeclaration declaration, RabbitMQQueueBindingDeclaration binding)
		{
			lock (@lock)
			{
				if (binding.NoWait)
				{
					channel.QueueBindNoWait(
						declaration.Name,
						binding.ExchangeDeclaration.Name,
						binding.RoutingKey ?? declaration.Name,
						binding.Arguments);
				}
				else
				{
					channel.QueueBind(
						declaration.Name,
						binding.ExchangeDeclaration.Name,
						binding.RoutingKey ?? declaration.Name,
						binding.Arguments);
				}
			}
		}

		public void QueueUnbind(RabbitMQQueueDeclaration declaration, RabbitMQQueueBindingDeclaration binding)
		{
			lock (@lock)
			{
				channel.QueueUnbind(
					declaration.Name,
					binding.ExchangeDeclaration.Name,
					binding.RoutingKey ?? declaration.Name,
					binding.Arguments);
			}
		}

		public void QueueDelete(RabbitMQQueueDeclaration declaration)
		{
			lock (@lock)
			{
				try
				{
					if (declaration.NoWait)
					{
						channel.QueueDeleteNoWait(
							declaration.Name,
							declaration.UnusedOnly,
							declaration.EmptyOnly);
					}
					else
					{
						channel.QueueDelete(
							declaration.Name,
							declaration.UnusedOnly,
							declaration.EmptyOnly);
					}
				}
				catch (OperationInterruptedException) when (declaration.UnusedOnly || declaration.EmptyOnly)
				{
					// RabbitMQ.Client does not ignore PRECONDITION_FAILED
					// Means that queue is used or not empty, so just ignore exception
					// TODO: Informative logging
					logger.LogWarning($"Unable to delete '{declaration.Name}' queue");
				}
			}
		}

		// Consumers
		public void BasicConsume(
			IServiceProvider serviceProvider,
			RabbitMQOptions options,
			RabbitMQQueueDeclaration queue,
			RabbitMQConsumerDeclaration declaration,
			CancellationToken cancellationToken)
		{
			lock (@lock)
			{
				for (var index = 0; index < declaration.Count; index++)
				{
					channel.BasicConsume(
						queue: queue.Name,
						autoAck: declaration.AutoAck,
						consumerTag: declaration.Tag is null
							? string.Empty
							: $"{declaration.Tag}_{index}",
						noLocal: declaration.NoLocal,
						exclusive: declaration.Exclusive,
						arguments: declaration.Arguments,
						consumer: new RabbitMQConsumer(channel, options, serviceProvider, declaration, cancellationToken));
				}
			}
		}

		public void BasicQos(RabbitMQConsumerDeclaration declaration)
		{
			lock (@lock)
			{
				// PrefetchSize != 0 is not implemented for now
				channel.BasicQos(0, declaration.PrefetchCount, declaration.Global);
			}
		}

		// Producers
		public void BasicPublish(RabbitMQProducerDeclaration declaration, byte[] payload)
		{
			var properties = CreateBasicProperties(declaration);

			lock (@lock)
			{
				channel.BasicPublish(
					declaration.ExchangeDeclaration.Name,
					declaration.RoutingKey ?? declaration.ExchangeDeclaration.Name,
					declaration.Mandatory,
					properties,
					payload);
			}
		}

		public IBasicPublishBatch CreateBasicPublishBatch(
			RabbitMQProducerDeclaration declaration,
			IEnumerable<byte[]> payloads)
		{
			var batch = channel.CreateBasicPublishBatch();
			var properties = CreateBasicProperties(declaration);

			foreach (var payload in payloads)
			{
				batch.Add(
					declaration.ExchangeDeclaration.Name,
					declaration.RoutingKey ?? declaration.ExchangeDeclaration.Name,
					declaration.Mandatory,
					properties,
					payload);
			}

			return batch;
		}

		public IBasicProperties CreateBasicProperties(RabbitMQProducerDeclaration declaration)
		{
			var properties = channel.CreateBasicProperties();

			foreach (var property in declaration.Properties)
			{
				property(properties);
			}

			return properties;
		}

		// Channels
		public void TxSelect()
		{
			lock (@lock)
			{
				channel.TxSelect();
			}
		}

		public void TxCommit()
		{
			lock (@lock)
			{
				channel.TxCommit();
			}
		}

		public void TxRollback()
		{
			lock (@lock)
			{
				channel.TxRollback();
			}
		}

		public void ConfirmSelect()
		{
			lock (@lock)
			{
				channel.ConfirmSelect();
			}
		}

		public bool WaitForConfirms(RabbitMQProducerDeclaration declaration)
		{
			lock (@lock)
			{
				if (declaration.Die)
				{
					if (declaration.Timeout is null)
					{
						channel.WaitForConfirmsOrDie();
					}
					else
					{
						channel.WaitForConfirmsOrDie(declaration.Timeout.Value);
					}

					return true;
				}

				return declaration.Timeout is null
					? channel.WaitForConfirms()
					: channel.WaitForConfirms(declaration.Timeout.Value);
			}
		}

		public void Dispose()
		{
			channel?.Dispose();
		}

		public event EventHandler<EventArgs> BasicRecoverOk
		{
			add => channel.BasicRecoverOk += value;
			remove => channel.BasicRecoverOk -= value;
		}

		public event EventHandler<CallbackExceptionEventArgs> CallbackException
		{
			add => channel.CallbackException += value;
			remove => channel.CallbackException -= value;
		}

		public event EventHandler<FlowControlEventArgs> FlowControl
		{
			add => channel.FlowControl += value;
			remove => channel.FlowControl -= value;
		}

		public event EventHandler<ShutdownEventArgs> ModelShutdown
		{
			add => channel.ModelShutdown += value;
			remove => channel.ModelShutdown -= value;
		}
	}
}