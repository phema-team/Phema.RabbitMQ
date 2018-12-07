using System;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Phema.Rabbit
{
	public static class RabbitBuilderExtensions
	{
		public static IRabbitBuilder AddDirectProducers(
			this IRabbitBuilder builder, 
			Action<IProducersConfiguration<DirectRabbitExchange>> action)
		{
			builder.Services.TryAddSingleton<DirectRabbitExchange, DefaultDirectRabbitExchange>();
			action(new ProducersConfiguration<DirectRabbitExchange>(builder.Services));
			return builder;
		}
		
		public static IRabbitBuilder AddDirectProducers<TDirectRabbitExchange>(
			this IRabbitBuilder builder, 
			Action<IProducersConfiguration<TDirectRabbitExchange>> action)
			where TDirectRabbitExchange : DirectRabbitExchange
		{
			builder.Services.TryAddSingleton<TDirectRabbitExchange>();
			action(new ProducersConfiguration<TDirectRabbitExchange>(builder.Services));
			return builder;
		}
		
		public static IRabbitBuilder AddFanoutProducers<TPayload>(
			this IRabbitBuilder builder, 
			Action<IProducersConfiguration<TPayload, FanoutRabbitExchange<TPayload>>> action)
		{
			builder.Services.TryAddSingleton<FanoutRabbitExchange<TPayload>, DefaultFanoutRabbitExchange<TPayload>>();
			return builder.AddFanoutProducers<TPayload, FanoutRabbitExchange<TPayload>>(action);
		}
		
		public static IRabbitBuilder AddFanoutProducers<TPayload, TFanoutRabbitExchange>(
			this IRabbitBuilder builder, 
			Action<IProducersConfiguration<TPayload, TFanoutRabbitExchange>> action)
			where TFanoutRabbitExchange : FanoutRabbitExchange<TPayload>
		{
			builder.Services.TryAddSingleton<TFanoutRabbitExchange>();
			action(new ProducersConfiguration<TPayload, TFanoutRabbitExchange>(
				new ProducersConfiguration<TFanoutRabbitExchange>(builder.Services)));
			
			return builder;
		}
		
		public static IRabbitBuilder AddHeadersProducers(
			this IRabbitBuilder builder, 
			Action<IProducersConfiguration<HeadersRabbitExchange>> action)
		{
			builder.Services.TryAddSingleton<HeadersRabbitExchange, DefaultHeadersRabbitExchange>();
			action(new ProducersConfiguration<HeadersRabbitExchange>(builder.Services));
			return builder;
		}
		
		public static IRabbitBuilder AddHeadersProducers<THeadersRabbitExchange>(
			this IRabbitBuilder builder, 
			Action<IProducersConfiguration<THeadersRabbitExchange>> action)
			where THeadersRabbitExchange : HeadersRabbitExchange
		{
			builder.Services.TryAddSingleton<THeadersRabbitExchange>();
			action(new ProducersConfiguration<THeadersRabbitExchange>(builder.Services));
			return builder;
		}
		
		public static IRabbitBuilder AddTopicProducers(
			this IRabbitBuilder builder, 
			Action<IProducersConfiguration<TopicRabbitExchange>> action)
		{
			builder.Services.TryAddSingleton<TopicRabbitExchange, DefaultTopicRabbitExchange>();
			action(new ProducersConfiguration<TopicRabbitExchange>(builder.Services));
			return builder;
		}
		
		public static IRabbitBuilder AddTopicProducers<TTopicRabbitExchange>(
			this IRabbitBuilder builder, 
			Action<IProducersConfiguration<TTopicRabbitExchange>> action)
			where TTopicRabbitExchange : TopicRabbitExchange
		{
			builder.Services.TryAddSingleton<TTopicRabbitExchange>();
			action(new ProducersConfiguration<TTopicRabbitExchange>(builder.Services));
			return builder;
		}
	}
}