using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Phema.Rabbit
{
	public interface IConsumersConfiguration
	{
		IConsumersConfiguration AddConsumer<TPayload, TRabbitConsumer, TRabbitQueue>()
			where TRabbitConsumer : RabbitConsumer<TPayload>
			where TRabbitQueue : RabbitQueue<TPayload>;
	}

	internal class ConsumersConfiguration : IConsumersConfiguration
	{
		private readonly IServiceCollection services;

		public ConsumersConfiguration(IServiceCollection services)
		{
			this.services = services;
		}

		public IConsumersConfiguration AddConsumer<TPayload, TRabbitConsumer, TRabbitQueue>()
			where TRabbitConsumer : RabbitConsumer<TPayload>
			where TRabbitQueue : RabbitQueue<TPayload>
		{
			services.TryAddScoped<TRabbitConsumer>();
			services.TryAddSingleton<TRabbitQueue>();
			services.ConfigureOptions<RabbitConsumerConfigureOptions<TPayload, TRabbitConsumer, TRabbitQueue>>();

			return this;
		}
	}
}