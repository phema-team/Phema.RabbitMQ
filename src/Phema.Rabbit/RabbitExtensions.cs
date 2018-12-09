using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Phema.Rabbit
{
	public static class RabbitExtensions
	{
		/// <summary>
		/// Adds rabbit services
		/// </summary>
		public static IRabbitBuilder AddRabbit(this IServiceCollection services)
		{
			services.TryAddSingleton(provider =>
			{
				var options = provider.GetRequiredService<IOptions<RabbitOptions>>().Value;
				var factory = new ConnectionFactory {DispatchConsumersAsync = true};

				if (options.HostName != null)
					factory.HostName = options.HostName;

				if (options.Endpoint != null)
					factory.Endpoint = options.Endpoint;

				if (options.Password != null)
					factory.Password = options.Password;

				if (options.Port != null)
					factory.Port = options.Port.Value;

				if (options.Protocol != null)
					factory.Protocol = options.Protocol;

				if (options.Ssl != null)
					factory.Ssl = options.Ssl;

				if (options.Uri != null)
					factory.Uri = options.Uri;

				if (options.AuthMechanisms != null)
					factory.AuthMechanisms = options.AuthMechanisms;

				if (options.ClientProperties != null)
					factory.ClientProperties = options.ClientProperties;

				if (options.ContinuationTimeout != null)
					factory.ContinuationTimeout = options.ContinuationTimeout.Value;

				if (options.RequestedHeartbeat != null)
					factory.RequestedHeartbeat = options.RequestedHeartbeat.Value;

				if (options.UserName != null)
					factory.UserName = options.UserName;

				if (options.VirtualHost != null)
					factory.VirtualHost = options.VirtualHost;

				if (options.AutomaticRecoveryEnabled != null)
					factory.AutomaticRecoveryEnabled = options.AutomaticRecoveryEnabled.Value;

				if (options.EndpointResolverFactory != null)
					factory.EndpointResolverFactory = options.EndpointResolverFactory;

				if (options.HandshakeContinuationTimeout != null)
					factory.HandshakeContinuationTimeout = options.HandshakeContinuationTimeout.Value;

				if (options.NetworkRecoveryInterval != null)
					factory.NetworkRecoveryInterval = options.NetworkRecoveryInterval.Value;

				if (options.RequestedChannelMax != null)
					factory.RequestedChannelMax = options.RequestedChannelMax.Value;

				if (options.RequestedConnectionTimeout != null)
					factory.RequestedConnectionTimeout = options.RequestedConnectionTimeout.Value;

				if (options.RequestedFrameMax != null)
					factory.RequestedFrameMax = options.RequestedFrameMax.Value;

				if (options.SocketReadTimeout != null)
					factory.SocketReadTimeout = options.SocketReadTimeout.Value;

				if (options.SocketWriteTimeout != null)
					factory.SocketWriteTimeout = options.SocketWriteTimeout.Value;

				if (options.TopologyRecoveryEnabled != null)
					factory.TopologyRecoveryEnabled = options.TopologyRecoveryEnabled.Value;

				if (options.AmqpUriSslProtocols != null)
					factory.AmqpUriSslProtocols = options.AmqpUriSslProtocols.Value;

				if (options.UseBackgroundThreadsForIO != null)
					factory.UseBackgroundThreadsForIO = options.UseBackgroundThreadsForIO.Value;

				return options.InstanceName == null
					? factory.CreateConnection()
					: factory.CreateConnection(options.InstanceName);
			});

			return new RabbitBuilder(services);
		}

		/// <summary>
		/// Adds rabbit services with <see cref="RabbitOptions"/> configuration
		/// </summary>
		public static IRabbitBuilder AddRabbit(this IServiceCollection services, Action<RabbitOptions> action)
		{
			services.Configure(action);
			return services.AddRabbit();
		}
	}
}