using RabbitMQ.Client;
using RabbitMQ.Client.Impl;

namespace Phema.RabbitMQ
{
	public class RabbitMQProtocol : IProtocol
	{
		private readonly object @lock = new object();
		private readonly IProtocol protocol;

		public RabbitMQProtocol(IProtocol protocol)
		{
			this.protocol = protocol;
		}

		public IConnection CreateConnection(IConnectionFactory factory, bool insist, IFrameHandler frameHandler)
		{
			lock (@lock)
			{
				return protocol.CreateConnection(factory, insist, frameHandler);
			}
		}

		public IConnection CreateConnection(
			ConnectionFactory factory,
			IFrameHandler frameHandler,
			bool automaticRecoveryEnabled)
		{
			lock (@lock)
			{
				return protocol.CreateConnection(factory, frameHandler, automaticRecoveryEnabled);
			}
		}

		public IConnection CreateConnection(
			IConnectionFactory factory,
			bool insist,
			IFrameHandler frameHandler,
			string clientProvidedName)
		{
			lock (@lock)
			{
				return protocol.CreateConnection(factory, insist, frameHandler, clientProvidedName);
			}
		}

		public IConnection CreateConnection(
			ConnectionFactory factory,
			IFrameHandler frameHandler,
			bool automaticRecoveryEnabled,
			string clientProvidedName)
		{
			lock (@lock)
			{
				return protocol.CreateConnection(factory, frameHandler, automaticRecoveryEnabled, clientProvidedName);
			}
		}

		public IModel CreateModel(ISession session)
		{
			lock (@lock)
			{
				return protocol.CreateModel(session);
			}
		}

		public string ApiName
		{
			get
			{
				lock (@lock)
				{
					return protocol.ApiName;
				}
			}
		}

		public int DefaultPort
		{
			get
			{
				lock (@lock)
				{
					return protocol.DefaultPort;
				}
			}
		}

		public int MajorVersion
		{
			get
			{
				lock (@lock)
				{
					return protocol.MajorVersion;
				}
			}
		}

		public int MinorVersion
		{
			get
			{
				lock (@lock)
				{
					return protocol.MinorVersion;
				}
			}
		}

		public int Revision
		{
			get
			{
				lock (@lock)
				{
					return protocol.Revision;
				}
			}
		}
	}
}