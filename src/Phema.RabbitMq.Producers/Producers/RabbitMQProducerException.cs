using System;

namespace Phema.RabbitMQ
{
	public class RabbitMQProducerException : Exception
	{
		public RabbitMQProducerException(string message, Exception exception = null)
			: base(message, exception)
		{
			
		}
	}
}