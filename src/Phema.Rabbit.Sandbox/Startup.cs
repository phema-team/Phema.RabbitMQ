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
	
	public class TestPayloadConsumer : RabbitConsumer<TestPayload>
	{
		protected override string Name => "TestModelConsumer";
		protected override int Parallelism => 32;
		protected override ushort? Prefetch => 10;
		
		private readonly RandomScoped scoped;

		public TestPayloadConsumer(RandomScoped scoped)
		{
			this.scoped = scoped;
		}
		
		protected override Task Consume(TestPayload payload)
		{
			Console.WriteLine(payload.Name + " - " + scoped.Type);

			return Task.CompletedTask;
		}
	}
	
	public class TestPayloadProducer : RabbitProducer<TestPayload>
	{
		public void Send(string name)
		{
			Produce(new TestPayload { Name = name });
		}
	}
	
	public class TestModelQueue : RabbitQueue<TestPayload>
	{
		protected override string Name => "TestModelQueue";
	}
	
	public class TestModelExchange : DirectRabbitExchange
	{
		public override string Name => "TestModelExchange";
	}

	public class RandomScoped
	{
		public RandomScoped()
		{
			Type = Guid.NewGuid();
		}
		
		public Guid Type { get; set; }
	}
	
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

			services.AddScoped<RandomScoped>();

			services.AddRabbit(options =>
				{
					options.HostName = "92.63.96.49";
					options.Port = 4000;
					options.InstanceName = "Phema.Rabbit.Sandbox";
				})
				.AddConsumers(consumers =>
					consumers.AddConsumer<TestPayload, TestPayloadConsumer, TestModelQueue>())
				.AddProducers<TestModelExchange>(producers =>
					producers.AddProducer<TestPayload, TestPayloadProducer, TestModelQueue>());
		}

		public void Configure(IApplicationBuilder app)
		{
			app.Use((context, next) =>
			{
				var producer = context.RequestServices.GetRequiredService<TestPayloadProducer>();

				for (var i = 0; i < 100_000; i++)
				{
					producer.Send(context.Request.Headers["Message"] + i);
				}
				
				return Task.CompletedTask;
			});
		}
	}
}