using System;
using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConnectionProvider
	{
		/// <summary>
		/// Get cached thread-safe connection
		/// </summary>
		IConnection FromDeclaration(RabbitMQConnectionDeclaration connection);
	}

	internal sealed class RabbitMQConnectionProvider : IRabbitMQConnectionProvider
	{
		private readonly RabbitMQOptions options;
		private readonly ILogger<RabbitMQConnection> logger;
		private readonly ConcurrentDictionary<string, IConnection> connections;

		public RabbitMQConnectionProvider(IServiceProvider serviceProvider)
		{
			connections = new ConcurrentDictionary<string, IConnection>();
			logger = serviceProvider.GetService<ILogger<RabbitMQConnection>>();
			options = serviceProvider.GetRequiredService<IOptions<RabbitMQOptions>>().Value;
		}

		public IConnection FromDeclaration(RabbitMQConnectionDeclaration declaration)
		{
			return connections.GetOrAdd(declaration.Name, _ =>
			{
				var connection = new RabbitMQConnection(
					options.ConnectionFactory.ClientProvidedName is null
						? options.ConnectionFactory.CreateConnection(declaration.Name)
						: options.ConnectionFactory.CreateConnection(
							$"{options.ConnectionFactory.ClientProvidedName}.{declaration.Name}"));

				logger?.LogInformation($"Connection '{connection.ClientProvidedName}' established");

				EnsureLogging(connection, declaration);

				return connection;
			});
		}

		private void EnsureLogging(IConnection connection, RabbitMQConnectionDeclaration declaration)
		{
			connection.CallbackException += (sender, args) =>
				logger?.LogError(args.Exception, $"Connection: {declaration.Name}");

			connection.ConnectionBlocked += (sender, args) =>
				logger?.LogError($"Connection '{declaration.Name}' blocked: {args.Reason}");

			connection.ConnectionShutdown += (sender, args) =>
				logger?.LogInformation($"Connection '{declaration.Name}' shutdown: {args.Cause}");

			connection.ConnectionUnblocked += (sender, args) =>
				logger?.LogInformation($"Connection '{declaration.Name}' unblocked");

			connection.RecoverySucceeded += (sender, args) =>
				logger?.LogInformation($"Connection '{declaration.Name}' recovered");

			connection.ConnectionRecoveryError += (sender, args) =>
				logger?.LogError(args.Exception, $"Connection '{declaration.Name}' recovery error");
		}
	}
}