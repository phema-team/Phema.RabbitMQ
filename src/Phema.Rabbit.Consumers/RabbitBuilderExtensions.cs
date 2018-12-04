using System;
using Microsoft.Extensions.DependencyInjection;

namespace Phema.Rabbit
{
	public static class RabbitBuilderExtensions
	{
		public static IRabbitBuilder AddConsumers(this IRabbitBuilder builder, Action<IConsumersConfiguration> action)
		{
			action(new ConsumersConfiguration(builder.Services));

			return builder;
		}
	}
}