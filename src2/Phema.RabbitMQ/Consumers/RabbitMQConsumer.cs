using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQConsumer : AsyncDefaultBasicConsumer
	{
		private readonly RabbitMQOptions options;
		private readonly IServiceProvider serviceProvider;
		private readonly RabbitMQConsumerDeclaration declaration;
		private readonly CancellationToken token;

		public RabbitMQConsumer(
			IModel channel,
			RabbitMQOptions options,
			IServiceProvider serviceProvider,
			RabbitMQConsumerDeclaration declaration,
			CancellationToken token) : base(channel)
		{
			this.options = options;
			this.declaration = declaration;
			this.token = token;
			this.serviceProvider = serviceProvider;
		}

		public override async Task HandleBasicDeliver(
			string consumerTag,
			ulong deliveryTag,
			bool redelivered,
			string exchange,
			string routingKey,
			IBasicProperties properties,
			byte[] body)
		{
			var payload = await Deserialize(body).ConfigureAwait(false);

			using (var scope = serviceProvider.CreateScope())
			{
				var logger = scope.ServiceProvider.GetService<ILogger>();

				try
				{
					await declaration.Consumer(scope, payload, token).ConfigureAwait(false);
				}
				catch (Exception exception)
				{
					if (!declaration.AutoAck)
					{
						Model.BasicNack(deliveryTag, declaration.Multiple, !redelivered && declaration.Requeue);
					}

					logger?.LogError(exception, $"Consumer tag: {declaration.Tag}");

					throw;
				}

				if (!declaration.AutoAck)
				{
					Model.BasicAck(deliveryTag, declaration.Multiple);
				}
			}
		}

		private ValueTask<object> Deserialize(byte[] body)
		{
			using (var stream = new MemoryStream(body))
			{
				return JsonSerializer.DeserializeAsync(
					stream,
					declaration.Type,
					options.JsonSerializerOptions,
					token);
			}
		}
	}
}