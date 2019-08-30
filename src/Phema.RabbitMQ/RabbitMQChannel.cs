using System;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Impl;

namespace Phema.RabbitMQ
{
	public class RabbitMQChannel : IFullModel
	{
		private readonly object @lock = new object();

		private readonly IFullModel model;

		public RabbitMQChannel(IFullModel model)
		{
			this.model = model;
		}

		public void Dispose()
		{
			lock (@lock)
			{
				model.Dispose();
			}
		}

		public void Abort()
		{
			lock (@lock)
			{
				model.Abort();
			}
		}

		public void Abort(ushort replyCode, string replyText)
		{
			lock (@lock)
			{
				model.Abort(replyCode, replyText);
			}
		}

		public void BasicAck(ulong deliveryTag, bool multiple)
		{
			lock (@lock)
			{
				model.BasicAck(deliveryTag, multiple);
			}
		}

		public void BasicCancel(string consumerTag)
		{
			lock (@lock)
			{
				model.BasicCancel(consumerTag);
			}
		}

		public string BasicConsume(
			string queue,
			bool autoAck,
			string consumerTag,
			bool noLocal,
			bool exclusive,
			IDictionary<string, object> arguments,
			IBasicConsumer consumer)
		{
			lock (@lock)
			{
				return model.BasicConsume(queue, autoAck, consumerTag, noLocal, exclusive, arguments, consumer);
			}
		}

		public BasicGetResult BasicGet(string queue, bool autoAck)
		{
			lock (@lock)
			{
				return model.BasicGet(queue, autoAck);
			}
		}

		public void BasicNack(ulong deliveryTag, bool multiple, bool requeue)
		{
			lock (@lock)
			{
				model.BasicNack(deliveryTag, multiple, requeue);
			}
		}

		public void BasicPublish(
			string exchange,
			string routingKey,
			bool mandatory,
			IBasicProperties basicProperties,
			byte[] body)
		{
			lock (@lock)
			{
				model.BasicPublish(exchange, routingKey, mandatory, basicProperties, body);
			}
		}

		public void BasicQos(uint prefetchSize, ushort prefetchCount, bool global)
		{
			lock (@lock)
			{
				model.BasicQos(prefetchSize, prefetchCount, global);
			}
		}

		public void BasicRecover(bool requeue)
		{
			lock (@lock)
			{
				model.BasicRecover(requeue);
			}
		}

		public void BasicRecoverAsync(bool requeue)
		{
			lock (@lock)
			{
				model.BasicRecoverAsync(requeue);
			}
		}

		public void BasicReject(ulong deliveryTag, bool requeue)
		{
			lock (@lock)
			{
				model.BasicReject(deliveryTag, requeue);
			}
		}

		public void Close()
		{
			lock (@lock)
			{
				model.Close();
			}
		}

		public void Close(ushort replyCode, string replyText)
		{
			lock (@lock)
			{
				model.Close(replyCode, replyText);
			}
		}

		public void ConfirmSelect()
		{
			lock (@lock)
			{
				model.ConfirmSelect();
			}
		}

		public IBasicPublishBatch CreateBasicPublishBatch()
		{
			lock (@lock)
			{
				return model.CreateBasicPublishBatch();
			}
		}

		public IBasicProperties CreateBasicProperties()
		{
			lock (@lock)
			{
				return model.CreateBasicProperties();
			}
		}

		public void ExchangeBind(
			string destination,
			string source,
			string routingKey,
			IDictionary<string, object> arguments)
		{
			lock (@lock)
			{
				model.ExchangeBind(destination, source, routingKey, arguments);
			}
		}

		public void ExchangeBindNoWait(
			string destination,
			string source,
			string routingKey,
			IDictionary<string, object> arguments)
		{
			lock (@lock)
			{
				model.ExchangeBindNoWait(destination, source, routingKey, arguments);
			}
		}

		public void ExchangeDeclare(
			string exchange,
			string type,
			bool durable,
			bool autoDelete,
			IDictionary<string, object> arguments)
		{
			lock (@lock)
			{
				model.ExchangeDeclare(exchange, type, durable, autoDelete, arguments);
			}
		}

		public void ExchangeDeclareNoWait(
			string exchange,
			string type,
			bool durable,
			bool autoDelete,
			IDictionary<string, object> arguments)
		{
			lock (@lock)
			{
				model.ExchangeDeclareNoWait(exchange, type, durable, autoDelete, arguments);
			}
		}

		public void ExchangeDeclarePassive(string exchange)
		{
			lock (@lock)
			{
				model.ExchangeDeclarePassive(exchange);
			}
		}

		public void ExchangeDelete(string exchange, bool ifUnused)
		{
			lock (@lock)
			{
				model.ExchangeDelete(exchange, ifUnused);
			}
		}

		public void ExchangeDeleteNoWait(string exchange, bool ifUnused)
		{
			lock (@lock)
			{
				model.ExchangeDeleteNoWait(exchange, ifUnused);
			}
		}

		public void ExchangeUnbind(
			string destination,
			string source,
			string routingKey,
			IDictionary<string, object> arguments)
		{
			lock (@lock)
			{
				model.ExchangeUnbind(destination, source, routingKey, arguments);
			}
		}

		public void ExchangeUnbindNoWait(
			string destination,
			string source,
			string routingKey,
			IDictionary<string, object> arguments)
		{
			lock (@lock)
			{
				model.ExchangeUnbindNoWait(destination, source, routingKey, arguments);
			}
		}

		public void QueueBind(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
		{
			lock (@lock)
			{
				model.QueueBind(queue, exchange, routingKey, arguments);
			}
		}

		public void QueueBindNoWait(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
		{
			lock (@lock)
			{
				model.QueueBindNoWait(queue, exchange, routingKey, arguments);
			}
		}

		public QueueDeclareOk QueueDeclare(
			string queue,
			bool durable,
			bool exclusive,
			bool autoDelete,
			IDictionary<string, object> arguments)
		{
			lock (@lock)
			{
				return model.QueueDeclare(queue, durable, exclusive, autoDelete, arguments);
			}
		}

		public void QueueDeclareNoWait(
			string queue,
			bool durable,
			bool exclusive,
			bool autoDelete,
			IDictionary<string, object> arguments)
		{
			lock (@lock)
			{
				model.QueueDeclareNoWait(queue, durable, exclusive, autoDelete, arguments);
			}
		}

		public QueueDeclareOk QueueDeclarePassive(string queue)
		{
			lock (@lock)
			{
				return model.QueueDeclarePassive(queue);
			}
		}

		public uint MessageCount(string queue)
		{
			lock (@lock)
			{
				return model.MessageCount(queue);
			}
		}

		public uint ConsumerCount(string queue)
		{
			lock (@lock)
			{
				return model.ConsumerCount(queue);
			}
		}

		public uint QueueDelete(string queue, bool ifUnused, bool ifEmpty)
		{
			lock (@lock)
			{
				return model.QueueDelete(queue, ifUnused, ifEmpty);
			}
		}

		public void QueueDeleteNoWait(string queue, bool ifUnused, bool ifEmpty)
		{
			lock (@lock)
			{
				model.QueueDeleteNoWait(queue, ifUnused, ifEmpty);
			}
		}

		public uint QueuePurge(string queue)
		{
			lock (@lock)
			{
				return model.QueuePurge(queue);
			}
		}

		public void QueueUnbind(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
		{
			lock (@lock)
			{
				model.QueueUnbind(queue, exchange, routingKey, arguments);
			}
		}

		public void TxCommit()
		{
			lock (@lock)
			{
				model.TxCommit();
			}
		}

		public void TxRollback()
		{
			lock (@lock)
			{
				model.TxRollback();
			}
		}

		public void TxSelect()
		{
			lock (@lock)
			{
				model.TxSelect();
			}
		}

		public bool WaitForConfirms()
		{
			lock (@lock)
			{
				return model.WaitForConfirms();
			}
		}

		public bool WaitForConfirms(TimeSpan timeout)
		{
			lock (@lock)
			{
				return model.WaitForConfirms(timeout);
			}
		}

		public bool WaitForConfirms(TimeSpan timeout, out bool timedOut)
		{
			lock (@lock)
			{
				return model.WaitForConfirms(timeout, out timedOut);
			}
		}

		public void WaitForConfirmsOrDie()
		{
			lock (@lock)
			{
				model.WaitForConfirmsOrDie();
			}
		}

		public void WaitForConfirmsOrDie(TimeSpan timeout)
		{
			lock (@lock)
			{
				model.WaitForConfirmsOrDie(timeout);
			}
		}

		public int ChannelNumber
		{
			get
			{
				lock (@lock)
				{
					return model.ChannelNumber;
				}
			}
		}

		public ShutdownEventArgs CloseReason
		{
			get
			{
				lock (@lock)
				{
					return model.CloseReason;
				}
			}
		}

		public IBasicConsumer DefaultConsumer
		{
			get
			{
				lock (@lock)
				{
					return model.DefaultConsumer;
				}
			}
			set
			{
				lock (@lock)
				{
					model.DefaultConsumer = value;
				}
			}
		}

		public bool IsClosed
		{
			get
			{
				lock (@lock)
				{
					return model.IsClosed;
				}
			}
		}

		public bool IsOpen
		{
			get
			{
				lock (@lock)
				{
					return model.IsOpen;
				}
			}
		}

		public ulong NextPublishSeqNo
		{
			get
			{
				lock (@lock)
				{
					return model.NextPublishSeqNo;
				}
			}
		}

		public TimeSpan ContinuationTimeout
		{
			get
			{
				lock (@lock)
				{
					return model.ContinuationTimeout;
				}
			}
			set
			{
				lock (@lock)
				{
					model.ContinuationTimeout = value;
				}
			}
		}

		public event EventHandler<BasicAckEventArgs> BasicAcks
		{
			add => model.BasicAcks += value;
			remove => model.BasicAcks -= value;
		}

		public event EventHandler<BasicNackEventArgs> BasicNacks
		{
			add => model.BasicNacks += value;
			remove => model.BasicNacks -= value;
		}

		public event EventHandler<EventArgs> BasicRecoverOk
		{
			add => model.BasicRecoverOk += value;
			remove => model.BasicRecoverOk -= value;
		}

		public event EventHandler<BasicReturnEventArgs> BasicReturn
		{
			add => model.BasicReturn += value;
			remove => model.BasicReturn -= value;
		}

		public event EventHandler<CallbackExceptionEventArgs> CallbackException
		{
			add => model.CallbackException += value;
			remove => model.CallbackException -= value;
		}

		public event EventHandler<FlowControlEventArgs> FlowControl
		{
			add => model.FlowControl += value;
			remove => model.FlowControl -= value;
		}

		public event EventHandler<ShutdownEventArgs> ModelShutdown
		{
			add => model.ModelShutdown += value;
			remove => model.ModelShutdown -= value;
		}

		public void ConnectionTuneOk(ushort channelMax, uint frameMax, ushort heartbeat)
		{
			lock (@lock)
			{
				model.ConnectionTuneOk(channelMax, frameMax, heartbeat);
			}
		}

		public void HandleBasicAck(ulong deliveryTag, bool multiple)
		{
			lock (@lock)
			{
				model.HandleBasicAck(deliveryTag, multiple);
			}
		}

		public void HandleBasicCancel(string consumerTag, bool nowait)
		{
			lock (@lock)
			{
				model.HandleBasicCancel(consumerTag, nowait);
			}
		}

		public void HandleBasicCancelOk(string consumerTag)
		{
			lock (@lock)
			{
				model.HandleBasicCancelOk(consumerTag);
			}
		}

		public void HandleBasicConsumeOk(string consumerTag)
		{
			lock (@lock)
			{
				model.HandleBasicConsumeOk(consumerTag);
			}
		}

		public void HandleBasicDeliver(
			string consumerTag,
			ulong deliveryTag,
			bool redelivered,
			string exchange,
			string routingKey,
			IBasicProperties basicProperties,
			byte[] body)
		{
			lock (@lock)
			{
				model.HandleBasicDeliver(consumerTag, deliveryTag, redelivered, exchange, routingKey, basicProperties, body);
			}
		}

		public void HandleBasicGetEmpty()
		{
			lock (@lock)
			{
				model.HandleBasicGetEmpty();
			}
		}

		public void HandleBasicGetOk(
			ulong deliveryTag,
			bool redelivered,
			string exchange,
			string routingKey,
			uint messageCount,
			IBasicProperties basicProperties,
			byte[] body)
		{
			lock (@lock)
			{
				model.HandleBasicGetOk(deliveryTag, redelivered, exchange, routingKey, messageCount, basicProperties, body);
			}
		}

		public void HandleBasicNack(ulong deliveryTag, bool multiple, bool requeue)
		{
			lock (@lock)
			{
				model.HandleBasicNack(deliveryTag, multiple, requeue);
			}
		}

		public void HandleBasicRecoverOk()
		{
			lock (@lock)
			{
				model.HandleBasicRecoverOk();
			}
		}

		public void HandleBasicReturn(
			ushort replyCode,
			string replyText,
			string exchange,
			string routingKey,
			IBasicProperties basicProperties,
			byte[] body)
		{
			lock (@lock)
			{
				model.HandleBasicReturn(replyCode, replyText, exchange, routingKey, basicProperties, body);
			}
		}

		public void HandleChannelClose(ushort replyCode, string replyText, ushort classId, ushort methodId)
		{
			lock (@lock)
			{
				model.HandleChannelClose(replyCode, replyText, classId, methodId);
			}
		}

		public void HandleChannelCloseOk()
		{
			lock (@lock)
			{
				model.HandleChannelCloseOk();
			}
		}

		public void HandleChannelFlow(bool active)
		{
			lock (@lock)
			{
				model.HandleChannelFlow(active);
			}
		}

		public void HandleConnectionBlocked(string reason)
		{
			lock (@lock)
			{
				model.HandleConnectionBlocked(reason);
			}
		}

		public void HandleConnectionClose(ushort replyCode, string replyText, ushort classId, ushort methodId)
		{
			lock (@lock)
			{
				model.HandleConnectionClose(replyCode, replyText, classId, methodId);
			}
		}

		public void HandleConnectionOpenOk(string knownHosts)
		{
			lock (@lock)
			{
				model.HandleConnectionOpenOk(knownHosts);
			}
		}

		public void HandleConnectionSecure(byte[] challenge)
		{
			lock (@lock)
			{
				model.HandleConnectionSecure(challenge);
			}
		}

		public void HandleConnectionStart(
			byte versionMajor,
			byte versionMinor,
			IDictionary<string, object> serverProperties,
			byte[] mechanisms,
			byte[] locales)
		{
			lock (@lock)
			{
				model.HandleConnectionStart(versionMajor, versionMinor, serverProperties, mechanisms, locales);
			}
		}

		public void HandleConnectionTune(ushort channelMax, uint frameMax, ushort heartbeat)
		{
			lock (@lock)
			{
				model.HandleConnectionTune(channelMax, frameMax, heartbeat);
			}
		}

		public void HandleConnectionUnblocked()
		{
			lock (@lock)
			{
				model.HandleConnectionUnblocked();
			}
		}

		public void HandleQueueDeclareOk(string queue, uint messageCount, uint consumerCount)
		{
			lock (@lock)
			{
				model.HandleQueueDeclareOk(queue, messageCount, consumerCount);
			}
		}

		public void _Private_BasicCancel(string consumerTag, bool nowait)
		{
			lock (@lock)
			{
				model._Private_BasicCancel(consumerTag, nowait);
			}
		}

		public void _Private_BasicConsume(
			string queue,
			string consumerTag,
			bool noLocal,
			bool autoAck,
			bool exclusive,
			bool nowait,
			IDictionary<string, object> arguments)
		{
			lock (@lock)
			{
				model._Private_BasicConsume(queue, consumerTag, noLocal, autoAck, exclusive, nowait, arguments);
			}
		}

		public void _Private_BasicGet(string queue, bool autoAck)
		{
			lock (@lock)
			{
				model._Private_BasicGet(queue, autoAck);
			}
		}

		public void _Private_BasicPublish(
			string exchange,
			string routingKey,
			bool mandatory,
			IBasicProperties basicProperties,
			byte[] body)
		{
			lock (@lock)
			{
				model._Private_BasicPublish(exchange, routingKey, mandatory, basicProperties, body);
			}
		}

		public void _Private_BasicRecover(bool requeue)
		{
			lock (@lock)
			{
				model._Private_BasicRecover(requeue);
			}
		}

		public void _Private_ChannelClose(ushort replyCode, string replyText, ushort classId, ushort methodId)
		{
			lock (@lock)
			{
				model._Private_ChannelClose(replyCode, replyText, classId, methodId);
			}
		}

		public void _Private_ChannelCloseOk()
		{
			lock (@lock)
			{
				model._Private_ChannelCloseOk();
			}
		}

		public void _Private_ChannelFlowOk(bool active)
		{
			lock (@lock)
			{
				model._Private_ChannelFlowOk(active);
			}
		}

		public void _Private_ChannelOpen(string outOfBand)
		{
			lock (@lock)
			{
				model._Private_ChannelOpen(outOfBand);
			}
		}

		public void _Private_ConfirmSelect(bool nowait)
		{
			lock (@lock)
			{
				model._Private_ConfirmSelect(nowait);
			}
		}

		public void _Private_ConnectionClose(ushort replyCode, string replyText, ushort classId, ushort methodId)
		{
			lock (@lock)
			{
				model._Private_ConnectionClose(replyCode, replyText, classId, methodId);
			}
		}

		public void _Private_ConnectionCloseOk()
		{
			lock (@lock)
			{
				model._Private_ConnectionCloseOk();
			}
		}

		public void _Private_ConnectionOpen(string virtualHost, string capabilities, bool insist)
		{
			lock (@lock)
			{
				model._Private_ConnectionOpen(virtualHost, capabilities, insist);
			}
		}

		public void _Private_ConnectionSecureOk(byte[] response)
		{
			lock (@lock)
			{
				model._Private_ConnectionSecureOk(response);
			}
		}

		public void _Private_ConnectionStartOk(
			IDictionary<string, object> clientProperties,
			string mechanism,
			byte[] response,
			string locale)
		{
			lock (@lock)
			{
				model._Private_ConnectionStartOk(clientProperties, mechanism, response, locale);
			}
		}

		public void _Private_ExchangeBind(
			string destination,
			string source,
			string routingKey,
			bool nowait,
			IDictionary<string, object> arguments)
		{
			lock (@lock)
			{
				model._Private_ExchangeBind(destination, source, routingKey, nowait, arguments);
			}
		}

		public void _Private_ExchangeDeclare(
			string exchange,
			string type,
			bool passive,
			bool durable,
			bool autoDelete,
			bool @internal,
			bool nowait,
			IDictionary<string, object> arguments)
		{
			lock (@lock)
			{
				model._Private_ExchangeDeclare(exchange, type, passive, durable, autoDelete, @internal, nowait, arguments);
			}
		}

		public void _Private_ExchangeDelete(string exchange, bool ifUnused, bool nowait)
		{
			lock (@lock)
			{
				model._Private_ExchangeDelete(exchange, ifUnused, nowait);
			}
		}

		public void _Private_ExchangeUnbind(
			string destination,
			string source,
			string routingKey,
			bool nowait,
			IDictionary<string, object> arguments)
		{
			lock (@lock)
			{
				model._Private_ExchangeUnbind(destination, source, routingKey, nowait, arguments);
			}
		}

		public void _Private_QueueBind(
			string queue,
			string exchange,
			string routingKey,
			bool nowait,
			IDictionary<string, object> arguments)
		{
			lock (@lock)
			{
				model._Private_QueueBind(queue, exchange, routingKey, nowait, arguments);
			}
		}

		public void _Private_QueueDeclare(
			string queue,
			bool passive,
			bool durable,
			bool exclusive,
			bool autoDelete,
			bool nowait,
			IDictionary<string, object> arguments)
		{
			lock (@lock)
			{
				model._Private_QueueDeclare(queue, passive, durable, exclusive, autoDelete, nowait, arguments);
			}
		}

		public uint _Private_QueueDelete(string queue, bool ifUnused, bool ifEmpty, bool nowait)
		{
			lock (@lock)
			{
				return model._Private_QueueDelete(queue, ifUnused, ifEmpty, nowait);
			}
		}

		public uint _Private_QueuePurge(string queue, bool nowait)
		{
			lock (@lock)
			{
				return model._Private_QueuePurge(queue, nowait);
			}
		}
	}
}