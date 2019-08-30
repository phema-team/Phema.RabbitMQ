using System;
using Microsoft.Extensions.DependencyInjection;

namespace Phema.RabbitMQ
{
	public static class RabbitMQBuilderExtensions
	{
		public static IRabbitMQBuilder AddConnection(
			this IRabbitMQBuilder builder,
			string name,
			Action<IRabbitMQConnectionBuilder> connection)
		{
			var declaration = new RabbitMQConnectionDeclaration(name);

			builder.Services
				.Configure<RabbitMQOptions>(options => options.ConnectionDeclarations.Add(declaration));

			connection.Invoke(new RabbitMQConnectionBuilder(builder.Services, declaration));

			return builder;
		}
	}
}