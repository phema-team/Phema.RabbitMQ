using System;
using System.Collections.Generic;
using System.Text.Json;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public sealed class RabbitMQOptions
	{
		public RabbitMQOptions()
		{
			ConnectionFactory = new ConnectionFactory
			{
				DispatchConsumersAsync = true,
				AutomaticRecoveryEnabled = false
			};

			Serializer = payload => JsonSerializer.SerializeToUtf8Bytes(payload);
			Deserializer = (bytes, type) => JsonSerializer.Deserialize(bytes, type);

			ConnectionDeclarations = new List<RabbitMQConnectionDeclaration>();
			ExchangeDeclarations = new List<RabbitMQExchangeDeclaration>();
			QueueDeclarations = new List<RabbitMQQueueDeclaration>();
			ConsumerDeclarations = new List<RabbitMQConsumerDeclaration>();
			ProducerDeclarations = new Dictionary<Type, RabbitMQProducerDeclaration>();
		}

		internal ConnectionFactory ConnectionFactory { get; }

		internal Func<object, byte[]> Serializer { get; set; }
		internal Func<byte[], Type, object> Deserializer { get; set; }

		// ReSharper disable once CollectionNeverQueried.Global
		internal IList<RabbitMQConnectionDeclaration> ConnectionDeclarations { get; }
		internal IList<RabbitMQExchangeDeclaration> ExchangeDeclarations { get; }
		internal IList<RabbitMQQueueDeclaration> QueueDeclarations { get; }
		internal IList<RabbitMQConsumerDeclaration> ConsumerDeclarations { get; }
		internal IDictionary<Type, RabbitMQProducerDeclaration> ProducerDeclarations { get; }
	}
}