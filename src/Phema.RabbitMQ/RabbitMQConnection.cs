using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Impl;

namespace Phema.RabbitMQ
{
	public class RabbitMQConnection : IConnection
	{
		private readonly object @lock = new object();

		private readonly IConnection connection;

		public RabbitMQConnection(IConnection connection)
		{
			this.connection = connection;
		}

		public int LocalPort
		{
			get
			{
				lock (@lock)
				{
					return connection.LocalPort;
				}
			}
		}

		public int RemotePort
		{
			get
			{
				lock (@lock)
				{
					return connection.RemotePort;
				}
			}
		}

		public void Dispose()
		{
			lock (@lock)
			{
				connection.Dispose();
			}
		}

		public void Abort()
		{
			lock (@lock)
			{
				connection.Abort();
			}
		}

		public void Abort(ushort reasonCode, string reasonText)
		{
			lock (@lock)
			{
				connection.Abort(reasonCode, reasonText);
			}
		}

		public void Abort(int timeout)
		{
			lock (@lock)
			{
				connection.Abort(timeout);
			}
		}

		public void Abort(ushort reasonCode, string reasonText, int timeout)
		{
			lock (@lock)
			{
				connection.Abort(reasonCode, reasonText, timeout);
			}
		}

		public void Close()
		{
			lock (@lock)
			{
				connection.Close();
			}
		}

		public void Close(ushort reasonCode, string reasonText)
		{
			lock (@lock)
			{
				connection.Close(reasonCode, reasonText);
			}
		}

		public void Close(int timeout)
		{
			lock (@lock)
			{
				connection.Close(timeout);
			}
		}

		public void Close(ushort reasonCode, string reasonText, int timeout)
		{
			lock (@lock)
			{
				connection.Close(reasonCode, reasonText, timeout);
			}
		}

		public IModel CreateModel()
		{
			lock (@lock)
			{
				return new RabbitMQChannel((IFullModel)connection.CreateModel());
			}
		}

		public void HandleConnectionBlocked(string reason)
		{
			lock (@lock)
			{
				connection.HandleConnectionBlocked(reason);
			}
		}

		public void HandleConnectionUnblocked()
		{
			lock (@lock)
			{
				connection.HandleConnectionUnblocked();
			}
		}

		public ushort ChannelMax
		{
			get
			{
				lock (@lock)
				{
					return connection.ChannelMax;
				}
			}
		}

		public IDictionary<string, object> ClientProperties
		{
			get
			{
				lock (@lock)
				{
					return connection.ClientProperties;
				}
			}
		}

		public ShutdownEventArgs CloseReason
		{
			get
			{
				lock (@lock)
				{
					return connection.CloseReason;
				}
			}
		}

		public AmqpTcpEndpoint Endpoint
		{
			get
			{
				lock (@lock)
				{
					return connection.Endpoint;
				}
			}
		}

		public uint FrameMax
		{
			get
			{
				lock (@lock)
				{
					return connection.FrameMax;
				}
			}
		}

		public ushort Heartbeat
		{
			get
			{
				lock (@lock)
				{
					return connection.Heartbeat;
				}
			}
		}

		public bool IsOpen
		{
			get
			{
				lock (@lock)
				{
					return connection.IsOpen;
				}
			}
		}

		public AmqpTcpEndpoint[] KnownHosts
		{
			get
			{
				lock (@lock)
				{
					return connection.KnownHosts;
				}
			}
		}

		public IProtocol Protocol
		{
			get
			{
				lock (@lock)
				{
					return connection.Protocol;
				}
			}
		}

		public IDictionary<string, object> ServerProperties
		{
			get
			{
				lock (@lock)
				{
					return connection.ServerProperties;
				}
			}
		}

		public IList<ShutdownReportEntry> ShutdownReport
		{
			get
			{
				lock (@lock)
				{
					return connection.ShutdownReport;
				}
			}
		}

		public string ClientProvidedName
		{
			get
			{
				lock (@lock)
				{
					return connection.ClientProvidedName;
				}
			}
		}

		// TODO: Thread-safe?
		public ConsumerWorkService ConsumerWorkService
		{
			get
			{
				lock (@lock)
				{
					return connection.ConsumerWorkService;
				}
			}
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