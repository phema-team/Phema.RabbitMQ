using System;

namespace Phema.Rabbit
{
	public static class RabbitBuilderExtensions
	{
		public static IRabbitBuilder AddConsumers(this IRabbitBuilder builder, Action<IConsumersConfiguration> action)
		{
			return builder;
		}
	}
}