using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQProducerGroupBuilder
	{
		/// <summary>
		///   Add new <see cref="IRabbitMQProducer{TPayload}"/>
		/// </summary>
		IRabbitMQProducerBuilder AddProducer<TPayload>(string exchangeName);
	}
}

namespace Phema.RabbitMQ.Internal
{
	internal sealed class RabbitMQProducerGroupBuilder : IRabbitMQProducerGroupBuilder
	{
		private readonly string groupName;
		private readonly IServiceCollection services;

		public RabbitMQProducerGroupBuilder(IServiceCollection services, string groupName)
		{
			this.services = services;
			this.groupName = groupName;
		}

		public IRabbitMQProducerBuilder AddProducer<TPayload>(string exchangeName)
		{
			if (exchangeName is null)
				throw new ArgumentNullException(nameof(exchangeName));

			var declaration = new RabbitMQProducerDeclaration<TPayload>(groupName, exchangeName);

			services.TryAddSingleton(CreateProducer<TPayload>);

			services.Configure<RabbitMQProducersOptions>(o => o.Declarations.Add(declaration));

			return new RabbitMQProducerBuilder(declaration);
		}

		private IRabbitMQProducer<TPayload> CreateProducer<TPayload>(IServiceProvider provider)
		{
			var producerFactory = provider.GetRequiredService<IRabbitMQProducerFactory>();
			var connectionFactory = provider.GetRequiredService<IRabbitMQConnectionFactory>();

			var channel = connectionFactory.CreateConnection(groupName).CreateModel();

			var options = provider.GetRequiredService<IOptions<RabbitMQProducersOptions>>().Value;

			// TODO: Multiple producers for TPayload
			var declaration = options.Declarations
				.OfType<RabbitMQProducerDeclaration<TPayload>>()
				.Single();

			return producerFactory.CreateProducer<TPayload>(channel, declaration);
		}
	}
}