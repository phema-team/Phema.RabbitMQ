using System;

namespace Phema.RabbitMQ
{
	public static class RabbitMQQueueExtensions
	{
		public static IRabbitMQBuilder AddQueues(
			this IRabbitMQBuilder builder,
			Action<IRabbitMQQueuesBuilder> options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));

			options.Invoke(new RabbitMQQueuesBuilder(builder.Services));
			return builder;
		}
	}
}