using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	public class RabbitOptions
	{
		public RabbitOptions()
		{
			Encoding = Encoding.UTF8;
			SerializerSettings = new JsonSerializerSettings();
		}

		public Encoding Encoding { get; set; }
		public JsonSerializerSettings SerializerSettings { get; set; }
		
		public string HostName { get; set; }
		public AmqpTcpEndpoint Endpoint { get; set; }
		public string Password { get; set; }
		public int? Port { get; set; }
		public IProtocol Protocol { get; set; }
		public SslOption Ssl { get; set; }
		public Uri Uri { get; set; }
		public IList<AuthMechanismFactory> AuthMechanisms { get; set; }
		public IDictionary<string, object> ClientProperties { get; set; }
		public TimeSpan? ContinuationTimeout { get; set; }
		public ushort? RequestedHeartbeat { get; set; }
		public string UserName { get; set; }
		public string VirtualHost { get; set; }
		public bool? AutomaticRecoveryEnabled { get; set; }
		public Func<IEnumerable<AmqpTcpEndpoint>, IEndpointResolver> EndpointResolverFactory { get; set; }
		public TimeSpan? HandshakeContinuationTimeout { get; set; }
		public TimeSpan? NetworkRecoveryInterval { get; set; }
		public ushort? RequestedChannelMax { get; set; }
		public int? RequestedConnectionTimeout { get; set; }
		public uint? RequestedFrameMax { get; set; }
		public int? SocketReadTimeout { get; set; }
		public int? SocketWriteTimeout { get; set; }
		public bool? TopologyRecoveryEnabled { get; set; }
		public SslProtocols? AmqpUriSslProtocols { get; set; }
		public bool? UseBackgroundThreadsForIO { get; set; }
		public string InstanceName { get; set; }
	}
}