namespace Phema.RabbitMQ
{
	public sealed class RabbitMQConnectionDeclaration
	{
		public RabbitMQConnectionDeclaration(string name)
		{
			Name = name;
		}

		public string Name { get; }
	}
}