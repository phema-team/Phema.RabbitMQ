using System;

namespace Phema.Rabbit
{
	public static class RabbitBuilderExtensions
	{
		public static IRabbitBuilder AddProducers<TRabbitExchange>(this IRabbitBuilder builder, Action<IProducersConfiguration<TRabbitExchange>> action)
			where TRabbitExchange : RabbitExchange
		{
			action(new ProducersConfiguration<TRabbitExchange>(builder.Services));
			return builder;
		}
	}
}