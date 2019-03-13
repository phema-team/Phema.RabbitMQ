using System.Threading.Tasks;

namespace Phema.RabbitMQ
{
	/// <summary>
	/// Scoped service for consuming <see cref="TPayload"/>
	/// </summary>
	/// <typeparam name="TPayload"></typeparam>
	public interface IRabbitMQConsumer<TPayload>
	{
		Task Consume(TPayload payload);
	}
}