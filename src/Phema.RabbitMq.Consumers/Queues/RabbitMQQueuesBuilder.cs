using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQQueuesBuilder
	{
		/// <summary>
		///   Register new queue
		/// </summary>
		IRabbitMQQueueBuilder AddQueue(string queueName);
	}

	internal sealed class RabbitMQQueuesBuilder : IRabbitMQQueuesBuilder
	{
		private readonly IServiceCollection services;
		private readonly string groupName;

		public RabbitMQQueuesBuilder(IServiceCollection services, string groupName)
		{
			this.services = services;
			this.groupName = groupName;
		}

		public IRabbitMQQueueBuilder AddQueue(string queueName)
		{
			if (queueName is null)
				throw new ArgumentNullException(nameof(queueName));

			queueName = groupName == RabbitMQDefaults.DefaultGroupName
				? queueName
				: $"{groupName}.{queueName}";
			
			var declaration = new RabbitMQQueueDeclaration(queueName);

			services.Configure<RabbitMQQueuesOptions>(options =>
			{
				if (options.Queues.Any(q => q.Name == queueName))
					throw new ArgumentException($"Queue {queueName} already registered", nameof(queueName));

				options.Queues.Add(declaration);
			});

			return new RabbitMQQueueBuilder(declaration);
		}
	}
}