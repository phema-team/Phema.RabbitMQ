using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Phema.Rabbit.Sandbox
{
	public class TestPayload
	{
		public string Name { get; set; }
	}
	
	public class TestModelConsumer : RabbitConsumer<TestPayload>
	{
		protected override int Parallelism => 5;
		protected override string Tag => "TestModelConsumer";

		protected override async Task Consume(TestPayload payload)
		{
			await Task.Delay(10_000);
			Console.WriteLine(payload.Name);
		}
	}
	
	public class TestPayloadProducer : RabbitProducer<TestPayload>
	{
		public void Send(TestPayload testPayload)
		{
			Produce(testPayload);
		}
	}
	
	public class TestModelQueue : DurableRabbitQueue<TestPayload>
	{
		protected override string Name => "TestModelQueue";
	}
	
	public class TestModelExchange : DirectRabbitExchange
	{
		public override string Name => "TestModelExchange";
	}
	
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

			services.AddRabbit(options =>
				{
					options.HostName = "92.63.96.49";
					options.Port = 4000;
				})
				.AddConsumers(consumers =>
					consumers.AddConsumer<TestPayload, TestModelConsumer, TestModelQueue>())
				.AddProducers(producers =>
					producers.AddProducer<TestPayload, TestPayloadProducer, TestModelQueue, TestModelExchange>());
		}

		public void Configure(IApplicationBuilder app)
		{
			app.Use((context, next) =>
			{
				var producer = context.RequestServices.GetRequiredService<TestPayloadProducer>();

				for (var i = 0; i < 100_000; i++)
				{
					producer.Send(new TestPayload
					{
						Name = context.Request.Headers["Message"]
					});
				}
				
				return Task.CompletedTask;
			});
		}
	}
}