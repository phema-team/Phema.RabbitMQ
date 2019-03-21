using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Phema.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Impl;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducerGroupBuilder
	{
		/// <summary>
		///   Add new producer
		/// </summary>
		IRabbitMQProducerBuilder AddProducer<TPayload>(string exchangeName, string queueName = null);
	}

	internal sealed class RabbitMQProducerGroupBuilder : IRabbitMQProducerGroupBuilder
	{
		private readonly IConnection connection;
		private readonly IServiceCollection services;

		public RabbitMQProducerGroupBuilder(IServiceCollection services, IConnection connection)
		{
			this.services = services;
			this.connection = connection;
		}

		public IRabbitMQProducerBuilder AddProducer<TPayload>(string exchangeName, string queueName = null)
		{
			if (exchangeName is null)
				throw new ArgumentNullException(nameof(exchangeName));

			var producer = new RabbitMQProducerMetadata(exchangeName, queueName);

			services.TryAddSingleton<IRabbitMQProducer<TPayload>>(provider =>
			{
				var channel = (IFullModel) connection.CreateModel();

				var exchange = provider.GetRequiredService<IOptions<RabbitMQExchangesOptions>>()
					.Value
					.Exchanges
					.FirstOrDefault(ex => ex.Name == producer.ExchangeName);

				// It can be default exchange or already declared
				// So no reason to write configuration for it
				if (exchange != null)
				{
					if (exchange.Deleted)
					{
						EnsureExchangeDeleted(channel, exchange);
					}
					else
					{
						DeclareExchange(channel, exchange);

						foreach (var binding in exchange.ExchangeBindings)
						{
							DeclareExchangeBinding(channel, exchange, binding);
						}
					}
				}

				if (producer.QueueName != null)
				{
					if (producer.Mandatory)
					{
						EnsureQueueDeclared(channel, producer);
					}

					// Should bind queue with exchange when not declared,
					// because of default or already declared
					BindQueue(channel, producer);
				}

				EnsureRoutingKeyOrQueueName(producer);

				var serializer = provider.GetRequiredService<ISerializer>();

				var properties = CreateBasicProperties(channel, producer);

				if (producer.WaitForConfirms)
				{
					channel.ConfirmSelect();
				}

				return new RabbitMQProducer<TPayload>(channel, serializer, producer, properties);
			});

			return new RabbitMQProducerBuilder(producer);
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

		private static void EnsureQueueDeclared(IModel channel, IRabbitMQProducerMetadata producer)
		{
			try
			{
				channel.QueueDeclarePassive(producer.QueueName);
			}
			catch (OperationInterruptedException exception)
			{
				throw new RabbitMQProducerException(
					$"Producer is mandatory, but exchange '{producer.ExchangeName}' " +
					$"or queue '{producer.QueueName}' does not declared in broker",
					exception);
			}
		}

		private static void BindQueue(IModel channel, IRabbitMQProducerMetadata producer)
		{
			try
			{
				channel.QueueBind(
					queue: producer.QueueName,
					exchange: producer.ExchangeName,
					routingKey: producer.RoutingKey ?? producer.QueueName,
					arguments: producer.Arguments);
			}
			catch (OperationInterruptedException exception)
			{
				throw new RabbitMQProducerException(
					$"Exchange '{producer.ExchangeName}' or queue '{producer.QueueName}' does not declared in broker",
					exception);
			}
		}

		private static void EnsureRoutingKeyOrQueueName(IRabbitMQProducerMetadata producer)
		{
			if ((producer.RoutingKey ?? producer.QueueName) is null)
			{
				throw new RabbitMQProducerException(
					$"'{nameof(producer.RoutingKey)}' or '{nameof(producer.QueueName)}' for producer should be declared");
			}
		}

		private static IBasicProperties CreateBasicProperties(IModel channel, IRabbitMQProducerMetadata producer)
		{
			var properties = channel.CreateBasicProperties();
			
			foreach (var property in producer.Properties)
			{
				property(properties);
			}

			return properties;
		}
	}
}