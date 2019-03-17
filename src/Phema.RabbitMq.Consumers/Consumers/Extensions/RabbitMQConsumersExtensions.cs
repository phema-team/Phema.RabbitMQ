using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

			var services = builder.Services;

			options.Invoke(new RabbitMQConsumersBuilder(services));

			services.AddHostedService<RabbitMQConsumersHostedService>();
			services.TryAddScoped<IRabbitMQConsumerFactory, RabbitMQConsumerFactory>();

			return builder;
		}
	}
}