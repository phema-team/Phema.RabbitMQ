using System;

namespace Phema.RabbitMQ
{
	public class RabbitMQMissingDeclarationException : Exception
	{
		public RabbitMQMissingDeclarationException(Type payloadType)
			: base("Missing producer declaration")
		{
			PayloadType = payloadType;
		}
		
		public Type PayloadType { get; }
	}
}