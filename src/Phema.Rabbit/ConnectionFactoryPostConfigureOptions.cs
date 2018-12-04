using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	public class ConnectionFactoryPostConfigureOptions : IPostConfigureOptions<ConnectionFactory>
	{
		private readonly RabbitOptions options;

		public ConnectionFactoryPostConfigureOptions(IOptions<RabbitOptions> options)
		{
			this.options = options.Value;
		}

		public void PostConfigure(string name, ConnectionFactory options)
		{
			options.DispatchConsumersAsync = true;
			
			if (this.options.HostName != null)
				options.HostName = this.options.HostName;

			if (this.options.Endpoint != null)
				options.Endpoint = this.options.Endpoint;
			
			if (this.options.Password != null)
				options.Password = this.options.Password;
			
			if (this.options.Port != null)
				options.Port = this.options.Port.Value;
			
			if (this.options.Protocol != null)
				options.Protocol = this.options.Protocol;
			
			if (this.options.Ssl != null)
				options.Ssl = this.options.Ssl;
			
			if (this.options.Uri != null)
				options.Uri = this.options.Uri;
			
			if (this.options.AuthMechanisms != null)
				options.AuthMechanisms = this.options.AuthMechanisms;
			
			if (this.options.ClientProperties != null)
				options.ClientProperties = this.options.ClientProperties;
			
			if (this.options.ContinuationTimeout != null)
				options.ContinuationTimeout = this.options.ContinuationTimeout.Value;
			
			if (this.options.RequestedHeartbeat != null)
				options.RequestedHeartbeat = this.options.RequestedHeartbeat.Value;
			
			if (this.options.UserName != null)
				options.UserName = this.options.UserName;

			if (this.options.VirtualHost != null)
				options.VirtualHost = this.options.VirtualHost;
			
			if (this.options.AutomaticRecoveryEnabled != null)
				options.AutomaticRecoveryEnabled = this.options.AutomaticRecoveryEnabled.Value;
			
			if (this.options.EndpointResolverFactory != null)
				options.EndpointResolverFactory = this.options.EndpointResolverFactory;
			
			if (this.options.HandshakeContinuationTimeout != null)
				options.HandshakeContinuationTimeout = this.options.HandshakeContinuationTimeout.Value;
			
			if (this.options.NetworkRecoveryInterval != null)
				options.NetworkRecoveryInterval = this.options.NetworkRecoveryInterval.Value;
			
			if (this.options.RequestedChannelMax != null)
				options.RequestedChannelMax = this.options.RequestedChannelMax.Value;
			
			if (this.options.RequestedConnectionTimeout != null)
				options.RequestedConnectionTimeout = this.options.RequestedConnectionTimeout.Value;

			if (this.options.RequestedFrameMax != null)
				options.RequestedFrameMax = this.options.RequestedFrameMax.Value;

			if (this.options.SocketReadTimeout != null)
				options.SocketReadTimeout = this.options.SocketReadTimeout.Value;
			
			if (this.options.SocketWriteTimeout != null)
				options.SocketWriteTimeout = this.options.SocketWriteTimeout.Value;
			
			if (this.options.TopologyRecoveryEnabled != null)
				options.TopologyRecoveryEnabled = this.options.TopologyRecoveryEnabled.Value;
			
			if (this.options.AmqpUriSslProtocols != null)
				options.AmqpUriSslProtocols = this.options.AmqpUriSslProtocols.Value;
			
			if (this.options.UseBackgroundThreadsForIO != null)
				options.UseBackgroundThreadsForIO = this.options.UseBackgroundThreadsForIO.Value;
		}
	}
}