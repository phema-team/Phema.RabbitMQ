using System.Threading.Tasks;

namespace Phema.RabbitMQ
{
	/// <summary>
	///   Scoped service for consuming <see cref="TPayload" />
	/// </summary>
	public interface IRabbitMQAsyncConsumer<TPayload>
	{
		Task Consume(TPayload payload);
	}
}