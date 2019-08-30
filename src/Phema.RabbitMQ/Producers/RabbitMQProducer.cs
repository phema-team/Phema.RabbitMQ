using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducer
	{
		Task<bool> Produce<TPayload>(TPayload payload);
		Task<bool> BatchProduce<TPayload>(IEnumerable<TPayload> payloads);
	}

	internal sealed class RabbitMQProducer : IRabbitMQProducer
	{
		private readonly ILogger logger;
		private readonly RabbitMQOptions options;
		private readonly IRabbitMQChannelCache channelCache;

		public RabbitMQProducer(
			IServiceProvider serviceProvider,
			IOptions<RabbitMQOptions> options,
			IRabbitMQChannelCache channelCache)
		{
			logger = serviceProvider.GetService<ILogger<RabbitMQProducer>>();
			this.options = options.Value;
			this.channelCache = channelCache;
		}

		public async Task<bool> Produce<TPayload>(TPayload payload)
		{
			// TODO: Dictionary?
			var declaration = options.ProducerDeclarations.First(d => d.Type == typeof(TPayload));

			var channel = channelCache.FromDeclaration(declaration);
			var properties = CreateBasicProperties(channel, declaration);
			var body = await Serialize(payload).ConfigureAwait(false);

			var routingKey = declaration.RoutingKey ?? declaration.Exchange.Name;
			
			try
			{
				channel.BasicPublish(
					declaration.Exchange.Name,
					routingKey,
					declaration.Mandatory,
					properties,
					body);

				if (declaration.Transactional)
				{
					channel.TxCommit();
				}
				
				return !declaration.WaitForConfirms || WaitForConfirms(channel, declaration);
			}
			catch (Exception exception)
			{
				if (declaration.Transactional)
				{
					channel.TxRollback();
				}

				logger?.LogError(exception, $"Routing key: {routingKey}");
				throw;
			}
		}

		public async Task<bool> BatchProduce<TPayload>(IEnumerable<TPayload> payloads)
		{
			var declaration = options.ProducerDeclarations.Single(d => d.Type == typeof(TPayload));

			var channel = channelCache.FromDeclaration(declaration);

			var batch = await CreateBasicPublishBatch(channel, declaration, payloads).ConfigureAwait(false);

			try
			{
				batch.Publish();

				if (declaration.Transactional)
				{
					channel.TxCommit();
				}

				return !declaration.WaitForConfirms || WaitForConfirms(channel, declaration);
			}
			catch (Exception exception)
			{
				if (declaration.Transactional)
				{
					channel.TxRollback();
				}

				logger?.LogError(exception, $"Routing key: {declaration.RoutingKey ?? declaration.Exchange.Name}");
				throw;
			}
		}

		private bool WaitForConfirms(IModel channel, RabbitMQProducerDeclaration declaration)
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

		private async Task<IBasicPublishBatch> CreateBasicPublishBatch<TPayload>(
			IModel channel,
			RabbitMQProducerDeclaration declaration,
			IEnumerable<TPayload> payloads)
		{
			var batch = channel.CreateBasicPublishBatch();
			var properties = CreateBasicProperties(channel, declaration);

			var routingKey = declaration.RoutingKey ?? declaration.Exchange.Name;
			
			foreach (var payload in payloads)
			{
				batch.Add(
					declaration.Exchange.Name,
					routingKey,
					declaration.Mandatory,
					properties,
					await Serialize(payload).ConfigureAwait(false));
			}

			return batch;
		}

		private IBasicProperties CreateBasicProperties(IModel channel, RabbitMQProducerDeclaration declaration)
		{
			var properties = channel.CreateBasicProperties();

			foreach (var property in declaration.Properties)
			{
				property(properties);
			}

			return properties;
		}

		private async Task<byte[]> Serialize<TPayload>(TPayload payload)
		{
			await using (var stream = new MemoryStream())
			{
				await JsonSerializer.SerializeAsync(stream, payload, options.JsonSerializerOptions).ConfigureAwait(false);

				return stream.ToArray();
			}
		}
	}
}