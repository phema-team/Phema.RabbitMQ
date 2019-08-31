using System;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public static class RabbitMQOptionsExtensions
	{
		public static RabbitMQOptions UseSerialization(
			this RabbitMQOptions options,
			Func<object, byte[]> serializer,
			Func<byte[], Type, object> deserializer)
		{
			options.Serializer = serializer;
			options.Deserializer = deserializer;

			return options;
		}

		public static RabbitMQOptions UseConnectionFactory(
			this RabbitMQOptions options,
			Action<ConnectionFactory> factory)
		{
			factory(options.ConnectionFactory);

			return options;
		}

		public static RabbitMQOptions UseConnectionUrl(
			this RabbitMQOptions options,
			string url)
		{
			return options.UseConnectionFactory(factory => factory.Uri = new Uri(url));
		}

		public static RabbitMQOptions UseClientProvidedName(
			this RabbitMQOptions options,
			string clientProvidedName)
		{
			return options.UseConnectionFactory(factory => factory.ClientProvidedName = clientProvidedName);
		}
	}
}