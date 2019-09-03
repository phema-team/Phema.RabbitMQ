using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Impl;

namespace Phema.RabbitMQ
{
	public sealed class RabbitMQConnection : IDisposable
	{
		private readonly object @lock = new object();

		private readonly IConnection connection;
		private readonly ILogger<RabbitMQChannel> channelLogger;

		public RabbitMQConnection(
			ILogger<RabbitMQChannel> channelLogger,
			RabbitMQConnectionDeclaration connectionDeclaration,
			IConnection connection)
		{
			ConnectionDeclaration = connectionDeclaration;
			this.channelLogger = channelLogger;
			this.connection = connection;
		}

		public string ClientProvidedName => connection.ClientProvidedName;
		public RabbitMQConnectionDeclaration ConnectionDeclaration { get; }

		public Task<RabbitMQChannel> CreateChannelAsync()
		{
			lock (@lock)
			{
				// TODO: Hack https://github.com/rabbitmq/rabbitmq-dotnet-client/issues/650
				return Task.Run(() => new RabbitMQChannel(
					ConnectionDeclaration,
					(IFullModel)connection.CreateModel(),
					channelLogger));
			}
		}

		public void Dispose()
		{
			connection?.Dispose();
		}

		public event EventHandler<CallbackExceptionEventArgs> CallbackException
		{
			add => connection.CallbackException += value;
			remove => connection.CallbackException -= value;
		}

		public event EventHandler<EventArgs> RecoverySucceeded
		{
			add => connection.RecoverySucceeded += value;
			remove => connection.RecoverySucceeded -= value;
		}

		public event EventHandler<ConnectionRecoveryErrorEventArgs> ConnectionRecoveryError
		{
			add => connection.ConnectionRecoveryError += value;
			remove => connection.ConnectionRecoveryError -= value;
		}

		public event EventHandler<ConnectionBlockedEventArgs> ConnectionBlocked
		{
			add => connection.ConnectionBlocked += value;
			remove => connection.ConnectionBlocked -= value;
		}

		public event EventHandler<ShutdownEventArgs> ConnectionShutdown
		{
			add => connection.ConnectionShutdown += value;
			remove => connection.ConnectionShutdown -= value;
		}

		public event EventHandler<EventArgs> ConnectionUnblocked
		{
			add => connection.ConnectionUnblocked += value;
			remove => connection.ConnectionUnblocked -= value;
		}
	}
}