using System.Threading.Tasks;

namespace Phema.Rabbit
{
	public interface IRabbitConsumer<TModel>
	{
		Task Consume(TModel model);
	}
}