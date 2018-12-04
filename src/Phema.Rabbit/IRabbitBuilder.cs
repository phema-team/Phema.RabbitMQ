using Microsoft.Extensions.DependencyInjection;

namespace Phema.Rabbit
{
	public interface IRabbitBuilder
	{
		IServiceCollection Services { get; }
	}
}