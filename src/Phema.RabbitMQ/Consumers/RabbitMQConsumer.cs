using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQConsumer : AsyncDefaultBasicConsumer
	{
		private readonly RabbitMQOptions options;
		private readonly IServiceProvider serviceProvider;
		private readonly RabbitMQConsumerDeclaration declaration;
		private readonly CancellationToken cancellationToken;

		public RabbitMQConsumer(
			IModel channel,
			RabbitMQOptions options,
			IServiceProvider serviceProvider,
			RabbitMQConsumerDeclaration declaration,
			CancellationToken cancellationToken) : base(channel)
		{
			this.options = options;
			this.declaration = declaration;
			this.cancellationToken = cancellationToken;
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
			var payload = options.Deserializer(body, declaration.Type);

			using (var scope = serviceProvider.CreateScope())
			{
				try
				{
					foreach (var subscription in declaration.Subscriptions)
					{
						await subscription(scope, payload, cancellationToken).ConfigureAwait(false);
					}
				}
				catch
				{
					if (!declaration.AutoAck)
					{
						Model.BasicNack(deliveryTag, declaration.Multiple, !redelivered && declaration.Requeue);
					}

					throw;
				}

				if (!declaration.AutoAck)
				{
					Model.BasicAck(deliveryTag, declaration.Multiple);
				}
			}
		}
	}
}