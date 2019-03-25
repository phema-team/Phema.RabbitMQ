using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Phema.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Phema.RabbitMQ
{
	internal sealed class RabbitMQBasicConsumer<TPayload, TPayloadConsumer> : AsyncEventingBasicConsumer
		where TPayloadConsumer : IRabbitMQConsumer<TPayload>
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
				var model = serializer.Deserialize<TPayload>(body);

				try
				{
					await scope.ServiceProvider
						.GetRequiredService<TPayloadConsumer>()
						.Consume(model)
						.ConfigureAwait(false);
				}
				catch (Exception)
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