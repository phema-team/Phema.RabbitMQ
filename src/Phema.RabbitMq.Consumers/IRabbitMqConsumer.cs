using System.Threading.Tasks;

namespace Phema.RabbitMQ
{
	public interface IRabbitMQConsumer<TPayload>
	{
		Task Consume(TPayload payload);
	}
}