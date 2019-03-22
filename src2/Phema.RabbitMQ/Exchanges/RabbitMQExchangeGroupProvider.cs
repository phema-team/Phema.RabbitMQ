using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Impl;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQExchangeGroupProvider
	{
		IRabbitMQExchangeGroupProvider Exchange(string type, string exchangeName, Action<IRabbitMQExchangeProvider> exchange);
	}
	
	internal sealed class RabbitMQExchangeGroupProvider : IRabbitMQExchangeGroupProvider
	{
		private readonly string groupName;
		private readonly IConnection connection;

		public RabbitMQExchangeGroupProvider(string groupName, IRabbitMQConnectionFactory factory)
		{
			this.groupName = groupName;
			connection = factory.CreateConnection(groupName);
		}

		public IRabbitMQExchangeGroupProvider Exchange(string type, string exchangeName, Action<IRabbitMQExchangeProvider> exchange)
		{
			if (exchangeName is null)
				throw new ArgumentNullException(nameof(exchangeName));

			var metadata = new RabbitMQExchangeMetadata(
				type,
				groupName == RabbitMQDefaults.DefaultGroupName
					? exchangeName
					: $"{groupName}.{exchangeName}");

			exchange.Invoke(new RabbitMQExchangeProvider(metadata));

			using (var channel = (IFullModel) connection.CreateModel())
			{
				if (metadata.Deleted)
				{
					EnsureExchangeDeleted(channel, metadata);
				}
				else
				{
					DeclareExchange(channel, metadata);

					// Should i move bindings to bindinds provider?
					foreach (var binding in metadata.ExchangeBindings)
					{
						DeclareExchangeBinding(channel, metadata, binding);
					}
				}
			}

			return this;
		}
		
		private static void EnsureExchangeDeleted(IFullModel channel, IRabbitMQExchangeMetadata exchange)
		{
			channel._Private_ExchangeDelete(exchange.Name, exchange.IfUnused, exchange.NoWait);
		}
		
		private static void DeclareExchange(IFullModel channel, IRabbitMQExchangeMetadata exchange)
		{
			channel._Private_ExchangeDeclare(
				exchange: exchange.Name,
				type: exchange.Type,
				passive: false,
				durable: exchange.Durable,
				autoDelete: exchange.AutoDelete,
				@internal: exchange.Internal,
				nowait: exchange.NoWait,
				arguments: exchange.Arguments);
		}

		private static void DeclareExchangeBinding(
			IFullModel channel,
			IRabbitMQExchangeMetadata exchange,
			IRabbitMQExchangeBindingMetadata binding)
		{
			channel._Private_ExchangeBind(
				destination: binding.ExchangeName,
				source: exchange.Name,
				routingKey: binding.RoutingKey ?? binding.ExchangeName,
				nowait: binding.NoWait,
				arguments: binding.Arguments);
		}
	}
}