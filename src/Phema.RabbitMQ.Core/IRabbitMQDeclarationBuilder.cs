namespace Phema.RabbitMQ
{
	public interface IRabbitMQDeclarationBuilder<TDeclaration>
	{
		TDeclaration Declaration { get; }
	}
}