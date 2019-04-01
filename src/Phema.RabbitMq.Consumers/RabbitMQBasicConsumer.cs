using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using ISerializer = Phema.Serialization.ISerializer;

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQBasicConsumer<TPayload, TPayloadConsumer> : AsyncEventingBasicConsumer
		where TPayloadConsumer : IRabbitMQAsyncConsumer<TPayload>
	{
		private readonly IRabbitMQConsumerDeclaration declaration;
		private readonly IServiceProvider provider;
		private readonly ISerializer serializer;

		public RabbitMQBasicConsumer(
			IServiceProvider provider,
			IModel channel,
			IRabbitMQConsumerDeclaration declaration,
			ISerializer serializer)
			: base(channel)
		{
			this.provider = provider;
			this.declaration = declaration;
			this.serializer = serializer;
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
			using (var scope = provider.CreateScope())
			{
				try
				{
					var model = serializer.Deserialize<TPayload>(body);

					await scope.ServiceProvider
						.GetRequiredService<TPayloadConsumer>()
						.Consume(model)
						.ConfigureAwait(false);
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