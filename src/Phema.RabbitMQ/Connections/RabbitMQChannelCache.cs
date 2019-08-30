using System;
using System.Collections.Concurrent;
using RabbitMQ.Client;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQChannelCache
	{
		IModel FromDeclaration(RabbitMQProducerDeclaration declaration);
	}

	internal sealed class RabbitMQChannelCache : IRabbitMQChannelCache
	{
		private readonly IRabbitMQConnectionCache connectionCache;
		private readonly ConcurrentDictionary<(Type, string, string), IModel> channels;

		public RabbitMQChannelCache(IRabbitMQConnectionCache connectionCache)
		{
			this.connectionCache = connectionCache;
			channels = new ConcurrentDictionary<(Type, string, string), IModel>();
		}

		public IModel FromDeclaration(RabbitMQProducerDeclaration declaration)
		{
			var key = (declaration.Type, declaration.Connection.Name, declaration.Exchange.Name);
			var connection = connectionCache.FromDeclaration(declaration.Connection);

			return channels.GetOrAdd(key, _ =>
			{
				var channel = connection.CreateModel();

				if (declaration.WaitForConfirms)
				{
					channel.ConfirmSelect();
				}

				if (declaration.Transactional)
				{
					channel.TxSelect();
				}

				return channel;
			});
		}
	}
}