using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQChannelProvider
	{
		/// <summary>
		/// Get cached thread-safe channel for producer declaration
		/// </summary>
		ValueTask<RabbitMQChannel> FromDeclaration(RabbitMQProducerDeclaration declaration);
	}

	internal sealed class RabbitMQChannelProvider : IRabbitMQChannelProvider
	{
		private readonly ILogger<RabbitMQChannel> logger;
		private readonly IRabbitMQConnectionProvider connectionProvider;
		private readonly ConcurrentDictionary<(Type, string, string), RabbitMQChannel> channels;

		public RabbitMQChannelProvider(IServiceProvider serviceProvider, IRabbitMQConnectionProvider connectionProvider)
		{
			this.connectionProvider = connectionProvider;
			logger = serviceProvider.GetService<ILogger<RabbitMQChannel>>();
			channels = new ConcurrentDictionary<(Type, string, string), RabbitMQChannel>();
		}

		public async ValueTask<RabbitMQChannel> FromDeclaration(RabbitMQProducerDeclaration declaration)
		{
			var key = (declaration.Type, declaration.ConnectionDeclaration.Name, declaration.ExchangeDeclaration.Name);
			var connection = connectionProvider.FromDeclaration(declaration.ConnectionDeclaration);

			if (!channels.TryGetValue(key, out var channel))
			{
				channel = await connection.CreateChannelAsync();

				EnsureLogging(channel);

				channels.TryAdd(key, channel);

				if (declaration.WaitForConfirms)
				{
					channel.ConfirmSelect();
				}

				if (declaration.Transactional)
				{
					channel.TxSelect();
				}
			}

			return channel;
		}

		private void EnsureLogging(RabbitMQChannel channel)
		{
			channel.CallbackException += (sender, args) =>
				logger?.LogError(
					args.Exception,
					$"Channel '{channel.ChannelNumber}' exception. Connection: '{channel.ConnectionDeclaration.Name}'");

			channel.FlowControl += (sender, args) =>
				logger?.LogWarning(
					$"Channel '{channel.ChannelNumber}' flow control. Connection: '{channel.ConnectionDeclaration.Name}', active: {args.Active}");

			channel.BasicRecoverOk += (sender, args) =>
				logger?.LogInformation(
					$"Channel '{channel.ChannelNumber}' recovery. Connection: '{channel.ConnectionDeclaration.Name}'");

			channel.ModelShutdown += (sender, args) =>
				logger?.LogError(
					$"Channel '{channel.ChannelNumber}' shutdown. Connection: '{channel.ConnectionDeclaration.Name}', reason: '{args.ReplyText}'");
		}
	}
}