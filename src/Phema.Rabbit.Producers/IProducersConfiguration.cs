namespace Phema.Rabbit
{
	public interface IProducersConfiguration
	{
		IProducersConfiguration AddProducer<TModel, TRabbitProducer>()
			where TRabbitProducer : RabbitProducer<TModel>;
	}
}