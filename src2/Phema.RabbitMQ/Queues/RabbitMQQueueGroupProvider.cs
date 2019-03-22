using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Impl;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueueGroupProvider
	{
		IRabbitMQQueueGroupProvider Queue(string queueName, Action<IRabbitMQQueueProvider> queue);
	}

	internal sealed class RabbitMQQueueGroupProvider : IRabbitMQQueueGroupProvider
	{
		private readonly string groupName;
		private readonly IConnection connection;

		public RabbitMQQueueGroupProvider(string groupName, IRabbitMQConnectionFactory factory)
		{
			this.groupName = groupName;
			connection = factory.CreateConnection(groupName);
		}

		public IRabbitMQQueueGroupProvider Queue(string queueName, Action<IRabbitMQQueueProvider> queue)
		{
			if (queueName is null)
				throw new ArgumentNullException(nameof(queueName));

			var metadata = new RabbitMQQueueMetadata(
				groupName == RabbitMQDefaults.DefaultGroupName
					? queueName
					: $"{groupName}.{queueName}");

			queue.Invoke(new RabbitMQQueueProvider(metadata));

			using (var channel = (IFullModel) connection.CreateModel())
			{
				if (metadata.Purged)
				{
					EnsureQueuePurged(channel, metadata);
				}

				if (metadata.Deleted)
				{
					EnsureQueueDeleted(channel, metadata);
				}
				else
				{
					DeclareQueue(channel, metadata);
				}
			}

			return this;
		}

		private static void EnsureQueuePurged(IFullModel channel, IRabbitMQQueueMetadata queue)
		{
			try
			{
				channel._Private_QueuePurge(queue.Name, queue.NoWait);
			}
			catch (OperationInterruptedException)
			{
				// Means that queue does not declared, so just ignore exception
			}
		}

		private static void EnsureQueueDeleted(IFullModel channel, IRabbitMQQueueMetadata metadata)
		{
			try
			{
				channel._Private_QueueDelete(metadata.Name, metadata.IfUnused, metadata.IfEmpty, metadata.NoWait);
			}
			catch (OperationInterruptedException) when (metadata.IfUnused || metadata.IfEmpty)
			{
				// RabbitMQ.Client does not ignore PRECONDITION_FAILED
				// Means that queue is used or not empty, so just ignore exception
			}
		}

		private static void DeclareQueue(IModel channel, IRabbitMQQueueMetadata metadata)
		{
			if (metadata.NoWait)
			{
				channel.QueueDeclareNoWait(
					queue: metadata.Name,
					durable: metadata.Durable,
					exclusive: metadata.Exclusive,
					autoDelete: metadata.AutoDelete,
					arguments: metadata.Arguments);
			}
			else
			{
				channel.QueueDeclare(
					queue: metadata.Name,
					durable: metadata.Durable,
					exclusive: metadata.Exclusive,
					autoDelete: metadata.AutoDelete,
					arguments: metadata.Arguments);
			}
		}
	}
}