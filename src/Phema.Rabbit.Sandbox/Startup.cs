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
		
		protected override ushort Prefetch => 800;
		protected override string Name => "TestModelConsumer";
	}

	public class TestPayloadProducer : RabbitProducer<TestPayload, DirectRabbitExchange>
	{
		public Task SendAsync(string name)
		{
			return ProduceAsync(new TestPayload
			{
				Name = name
			});
		}
	}

	public class TestPayloadQueue : RabbitQueue<TestPayload>
	{
		protected override string Name => "TestPayloadQueue";
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
					consumers.AddConsumer<TestPayload, TestPayloadConsumer, TestPayloadQueue>())
				.AddDirectProducers(producers =>
					producers.AddProducer<TestPayload, TestPayloadProducer, TestPayloadQueue>());
		}

		public void Configure(IApplicationBuilder app)
		{
			app.Use(async (context, next) =>
			{
				var producer = context.RequestServices.GetRequiredService<TestPayloadProducer>();

				for (var i = 0; i < 10_000; i++)
				{
					await producer.SendAsync(context.Request.Headers["Message"] + i);
				}
			});
		}
	}
}