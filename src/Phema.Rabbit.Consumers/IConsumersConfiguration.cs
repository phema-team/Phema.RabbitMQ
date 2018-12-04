namespace Phema.Rabbit
{
	public interface IConsumersConfiguration
	{
		IConsumersConfiguration AddConsumer<TModel, TRabbitConsumer>()
			where TRabbitConsumer : IRabbitConsumer<TModel>;
	}
}