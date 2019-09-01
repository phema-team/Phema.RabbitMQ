using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConnectionProvider
	{
		/// <summary>
		/// Get cached thread-safe connection
		/// </summary>
		RabbitMQConnection FromDeclaration(RabbitMQConnectionDeclaration connection);
	}

	internal sealed class RabbitMQConnectionProvider : IRabbitMQConnectionProvider
	{
		private readonly RabbitMQOptions options;
		private readonly ILogger<RabbitMQConnection> connectionLogger;
		private readonly ConcurrentDictionary<string, RabbitMQConnection> connections;
		private readonly ILogger<RabbitMQChannel> channelLogger;

		public RabbitMQConnectionProvider(IServiceProvider serviceProvider)
		{
			connections = new ConcurrentDictionary<string, RabbitMQConnection>();
			connectionLogger = serviceProvider.GetService<ILogger<RabbitMQConnection>>();
			channelLogger = serviceProvider.GetService<ILogger<RabbitMQChannel>>();
			options = serviceProvider.GetRequiredService<IOptions<RabbitMQOptions>>().Value;
		}

		public RabbitMQConnection FromDeclaration(RabbitMQConnectionDeclaration declaration)
		{
			return connections.GetOrAdd(declaration.Name, _ =>
			{
				var connection = new RabbitMQConnection(
					channelLogger,
					declaration,
					options.ConnectionFactory.ClientProvidedName is null
						? options.ConnectionFactory.CreateConnection(declaration.Name)
						: options.ConnectionFactory.CreateConnection(
							$"{options.ConnectionFactory.ClientProvidedName}.{declaration.Name}"));

				EnsureLogging(connection);

				return connection;
			});
		}

		private void EnsureLogging(RabbitMQConnection connection)
		{
			connectionLogger?.LogInformation($"Connection '{connection.ClientProvidedName}' established");

			connection.CallbackException += (sender, args) =>
				connectionLogger?.LogError(
					args.Exception,
					$"Connection: {connection.ConnectionDeclaration.Name}");

			connection.ConnectionBlocked += (sender, args) =>
				connectionLogger?.LogError(
					$"Connection '{connection.ConnectionDeclaration.Name}' blocked: {args.Reason}");

			connection.ConnectionShutdown += (sender, args) =>
				connectionLogger?.LogInformation(
					$"Connection '{connection.ConnectionDeclaration.Name}' shutdown: {args.Cause}");

			connection.ConnectionUnblocked += (sender, args) =>
				connectionLogger?.LogInformation(
					$"Connection '{connection.ConnectionDeclaration.Name}' unblocked");

			connection.RecoverySucceeded += (sender, args) =>
				connectionLogger?.LogInformation(
					$"Connection '{connection.ConnectionDeclaration.Name}' recovered");

			connection.ConnectionRecoveryError += (sender, args) =>
				connectionLogger?.LogError(args.Exception,
					$"Connection '{connection.ConnectionDeclaration.Name}' recovery error");
		}
	}
}