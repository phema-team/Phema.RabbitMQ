using System;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Phema.RabbitMQ
{
	internal static class RabbitMQConsumerLoggerExtensions
	{
		public static void LogConsumerException<TPayload>(
			this ILogger logger,
			RabbitMQConsumerMetadata consumer,
			Exception exception,
			byte[] body,
			bool redelivered)
		{
			var message = new StringBuilder()
				.Append($"Cannot process '{typeof(TPayload).Name}' payload ")
				.Append($"in '{consumer.QueueName}' queue ")
				.Append($"with base64 body '{Convert.ToBase64String(body)}'");

			var status = consumer.AutoAck || redelivered || !consumer.Requeue
				? "deleted"
				: "redelivared";

			message.AppendLine($"message will be {status}.")
				.AppendLine($"Exception message: {exception.Message}")
				.AppendLine($"Exception stack trace: {exception.StackTrace}");

			logger.LogError(message.ToString());
		}
	}
}