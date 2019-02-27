using System.Threading.Tasks;

namespace Phema.RabbitMq
{
	public interface IRabbitMqConsumer<TPayload>
	{
		ValueTask Consume(TPayload payload);
	}
}