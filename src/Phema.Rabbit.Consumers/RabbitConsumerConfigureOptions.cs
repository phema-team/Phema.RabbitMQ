using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Phema.Rabbit
{
	/// <summary>
	/// Used for configuring <see cref="RabbitConsumerOptions"/> to use in <see cref="RabbitConsumersHostedService"/>
	/// </summary>
	internal class RabbitConsumerConfigureOptions<TPayload, TRabbitConsumer, TRabbitQueue> : IConfigureOptions<RabbitConsumerOptions>
		where TRabbitConsumer : RabbitConsumer<TPayload>
		where TRabbitQueue : RabbitQueue<TPayload>
	{
		public void Configure(RabbitConsumerOptions options)
		{
			options.ConsumerActions.Add((provider, connection) =>
			{
				var consumer = provider.GetRequiredService<TRabbitConsumer>();

				var channel = connection.CreateModel();
				
				var queue = provider.GetRequiredService<TRabbitQueue>();

				channel.QueueDeclare(
					queue: queue.Name,
					durable: queue.Durable,
					exclusive: queue.Exclusive,
					autoDelete: queue.AutoDelete,
					arguments: queue.Arguments);

				channel.BasicQos(0, consumer.Prefetch, false);

				for (var index = 0; index < consumer.Parallelism; index++)
				{
					channel.BasicConsume(
						queue: queue.Name,
						autoAck: consumer.AutoAck,
						consumerTag: $"{consumer.Name}_{index}",
						noLocal: consumer.NoLocal,
						exclusive: consumer.Exclusive,
						arguments: consumer.Arguments,
						consumer: new RabbitBasicConsumer<TPayload, TRabbitConsumer>(provider, channel));
				}
			});
		}
	}
}