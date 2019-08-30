using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Phema.RabbitMQ.Web
{
	public class Startup
	{
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddRabbitMQ("instance")
				.AddConnection("name", connection =>
				{
					var exchange = connection.AddDirectExchange("exchange")
						.Durable()
						.AutoDelete();

					var queue = connection.AddQueue<string>("queue")
						.Durable()
						.AutoDelete()
						.BoundTo(exchange, binding => binding.NoWait());

					connection.AddConsumer(queue, Consumer)
						.Count(2)
						.Requeue()
						.AutoAck();

					connection.AddProducer<string>(exchange)
						.Persistent()
						.Transactional();
				});
		}

		private static ValueTask Consumer(IServiceScope scope, string payload)
		{
			return new ValueTask();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapGet("/", async context =>
				{
					await context.Response.WriteAsync("Hello World!");
				});
			});
		}
	}
}