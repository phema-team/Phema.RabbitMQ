using System;

namespace Phema.Rabbit
{
	public static class RabbitBuilderExtensions
	{
		public static IRabbitBuilder AddProducers(this IRabbitBuilder builder, Action<IProducersConfiguration> action)
		{
			action(new ProducersConfiguration(builder.Services));
			return builder;
		}
	}
}