using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQChannelProvider
	{
		/// <summary>
		/// Get cached thread-safe channel for producer declaration
		/// </summary>
		IModel FromDeclaration(RabbitMQProducerDeclaration declaration);

		/// <summary>
		/// Get cached thread-safe channel for consumer declaration
		/// </summary>
		IModel FromDeclaration(RabbitMQConsumerDeclaration declaration);

		/// <summary>
		/// Get thread-safe channel for exchange declaration
		/// </summary>
		IModel FromDeclaration(RabbitMQExchangeDeclaration declaration);

		/// <summary>
		/// Get thread-safe channel for queue declaration
		/// </summary>
		IModel FromDeclaration(RabbitMQQueueDeclaration declaration);
	}

	internal sealed class RabbitMQChannelProvider : IRabbitMQChannelProvider
	{
		private readonly ILogger<RabbitMQChannel> logger;
		private readonly IRabbitMQConnectionProvider connectionProvider;
		private readonly ConcurrentDictionary<(Type, string, string), IModel> channels;

		public RabbitMQChannelProvider(IServiceProvider serviceProvider)
		{
			logger = serviceProvider.GetService<ILogger<RabbitMQChannel>>();
			connectionProvider = serviceProvider.GetRequiredService<IRabbitMQConnectionProvider>();
			channels = new ConcurrentDictionary<(Type, string, string), IModel>();
		}

		public IModel FromDeclaration(RabbitMQProducerDeclaration declaration)
		{
			var key = (declaration.Type, declaration.Connection.Name, declaration.Exchange.Name);
			var connection = connectionProvider.FromDeclaration(declaration.Connection);

			return channels.GetOrAdd(key, _ =>
			{
				var channel = connection.CreateModel();

				EnsureLogging(channel, declaration.Connection);

				return channel;
			});
		}

		public IModel FromDeclaration(RabbitMQConsumerDeclaration declaration)
		{
			var connection = connectionProvider.FromDeclaration(declaration.Connection);

			var channel = connection.CreateModel();

			EnsureLogging(channel, declaration.Connection);

			return channel;
		}

		public IModel FromDeclaration(RabbitMQExchangeDeclaration declaration)
		{
			return connectionProvider.FromDeclaration(declaration.Connection).CreateModel();
		}

		public IModel FromDeclaration(RabbitMQQueueDeclaration declaration)
		{
			return connectionProvider.FromDeclaration(declaration.Connection).CreateModel();
		}

		private void EnsureLogging(IModel channel, RabbitMQConnectionDeclaration declaration)
		{
			channel.CallbackException += (sender, args) =>
				logger?.LogError(
					args.Exception,
					$"Channel '{channel.ChannelNumber}' exception. Connection: '{declaration.Name}'");

			channel.FlowControl += (sender, args) =>
				logger?.LogWarning(
					$"Channel '{channel.ChannelNumber}' flow control. Connection: '{declaration.Name}', active: {args.Active}");

			channel.BasicRecoverOk += (sender, args) =>
				logger?.LogInformation($"Channel '{channel.ChannelNumber}' recovery. Connection: '{declaration.Name}'");

			channel.ModelShutdown += (sender, args) =>
				logger?.LogError(
					$"Channel '{channel.ChannelNumber}' shutdown. Connection: '{declaration.Name}', reason: '{args.ReplyText}'");
		}
	}
}