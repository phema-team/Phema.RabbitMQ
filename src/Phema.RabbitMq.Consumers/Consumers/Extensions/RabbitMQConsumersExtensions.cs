using System;
using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public static class RabbitMQConsumersExtensions
	{
		public static IRabbitMQBuilder AddConsumers(
			this IRabbitMQBuilder builder,
			Action<IRabbitMQConsumersBuilder> options)
		{
			if (options is null)
				throw new ArgumentNullException(nameof(options));

			options.Invoke(new RabbitMQConsumersBuilder(builder.Services));

			builder.Services.AddHostedService<RabbitMQConsumersHostedService>();

			return builder;
		}
	}
}