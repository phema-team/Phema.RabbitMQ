using System;
using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueueGroupBuilder
	{
		/// <summary>
		///   Declare queue
		/// </summary>
		IRabbitMQQueueBuilder AddQueue(string queueName);
	}

	internal sealed class RabbitMQQueueGroupBuilder : IRabbitMQQueueGroupBuilder
	{
		private readonly string groupName;
		private readonly IServiceCollection services;

		public RabbitMQQueueGroupBuilder(IServiceCollection services, string groupName)
		{
			this.services = services;
			this.groupName = groupName;
		}

		public IRabbitMQQueueBuilder AddQueue(string queueName)
		{
			if (queueName is null)
				throw new ArgumentNullException(nameof(queueName));

			var declaration = new RabbitMQQueueDeclaration(groupName, queueName);

			services.Configure<RabbitMQQueuesOptions>(options => options.Declarations.Add(declaration));

			return new RabbitMQQueueBuilder(declaration);
		}
	}
}