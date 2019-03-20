using System;

namespace Phema.RabbitMQ
{
	public class RabbitMQConsumerException : Exception
	{
		public RabbitMQConsumerException(string message, Exception exception)
			: base(message, exception)
		{
		}
	}
}