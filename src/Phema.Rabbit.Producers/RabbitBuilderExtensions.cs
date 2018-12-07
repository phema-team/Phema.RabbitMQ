using System;

namespace Phema.Rabbit
{
	public static class RabbitBuilderExtensions
	{
		public static IRabbitBuilder AddDirectProducers(
			this IRabbitBuilder builder, 
			Action<IProducersConfiguration<DirectRabbitExchange>> action)
		{
			return builder.AddDirectProducers<DirectRabbitExchange>(action);
		}
		
		public static IRabbitBuilder AddDirectProducers<TDirectRabbitExchange>(
			this IRabbitBuilder builder, 
			Action<IProducersConfiguration<TDirectRabbitExchange>> action)
			where TDirectRabbitExchange : DirectRabbitExchange
		{
			action(new ProducersConfiguration<TDirectRabbitExchange>(builder.Services));
			return builder;
		}
		
		public static IRabbitBuilder AddFanoutProducers<TPayload>(
			this IRabbitBuilder builder, 
			Action<IProducersConfiguration<TPayload, FanoutRabbitExchange<TPayload>>> action)
		{
			return builder.AddFanoutProducers<TPayload, FanoutRabbitExchange<TPayload>>(action);
		}
		
		public static IRabbitBuilder AddFanoutProducers<TPayload, TDirectRabbitExchange>(
			this IRabbitBuilder builder, 
			Action<IProducersConfiguration<TPayload, FanoutRabbitExchange<TPayload>>> action)
			where TDirectRabbitExchange : FanoutRabbitExchange<TPayload>
		{
			action(new ProducersConfiguration<TPayload, FanoutRabbitExchange<TPayload>>(
				new ProducersConfiguration<FanoutRabbitExchange<TPayload>>(builder.Services)));
			
			return builder;
		}
		
		public static IRabbitBuilder AddHeadersProducers(
			this IRabbitBuilder builder, 
			Action<IProducersConfiguration<HeadersRabbitExchange>> action)
		{
			return builder.AddHeadersProducers<HeadersRabbitExchange>(action);
		}
		
		public static IRabbitBuilder AddHeadersProducers<THeadersRabbitExchange>(
			this IRabbitBuilder builder, 
			Action<IProducersConfiguration<THeadersRabbitExchange>> action)
			where THeadersRabbitExchange : HeadersRabbitExchange
		{
			action(new ProducersConfiguration<THeadersRabbitExchange>(builder.Services));
			return builder;
		}
		
		public static IRabbitBuilder AddTopicProducers(
			this IRabbitBuilder builder, 
			Action<IProducersConfiguration<TopicRabbitExchange>> action)
		{
			return builder.AddTopicProducers<TopicRabbitExchange>(action);
		}
		
		public static IRabbitBuilder AddTopicProducers<TTopicRabbitExchange>(
			this IRabbitBuilder builder, 
			Action<IProducersConfiguration<TTopicRabbitExchange>> action)
			where TTopicRabbitExchange : TopicRabbitExchange
		{
			action(new ProducersConfiguration<TTopicRabbitExchange>(builder.Services));
			return builder;
		}
	}
}