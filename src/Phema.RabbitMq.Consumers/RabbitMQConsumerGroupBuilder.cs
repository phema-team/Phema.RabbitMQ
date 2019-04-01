using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConsumerGroupBuilder
	{
		/// <summary>
		///   Register new <see cref="IRabbitMQConsumer{TPayload}"/>
		/// </summary>
		IRabbitMQConsumerBuilder AddAsyncConsumer<TPayload, TPayloadConsumer>(string queueName)
			where TPayloadConsumer : class, IRabbitMQConsumer<TPayload>;
	}
}

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQConsumerGroupBuilder : IRabbitMQConsumerGroupBuilder
	{
		private readonly string groupName;
		private readonly IServiceCollection services;

		public RabbitMQConsumerGroupBuilder(
			IServiceCollection services,
			string groupName)
		{
			this.services = services;
			this.groupName = groupName;
		}

		public IRabbitMQConsumerBuilder AddAsyncConsumer<TPayload, TPayloadConsumer>(string queueName)
			where TPayloadConsumer : class, IRabbitMQConsumer<TPayload>
		{
			if (queueName is null)
				throw new ArgumentNullException(nameof(queueName));

			services.TryAddScoped<TPayloadConsumer>();

			var declaration = new RabbitMQConsumerDeclaration<TPayload, TPayloadConsumer>(groupName, queueName);

			services.Configure<RabbitMQConsumersOptions>(options => options.Declarations.Add(declaration));
			services.AddHostedService<RabbitMQConsumerHostedService<TPayload, TPayloadConsumer>>();

			return new RabbitMQConsumerBuilder(declaration);
		}
	}
}