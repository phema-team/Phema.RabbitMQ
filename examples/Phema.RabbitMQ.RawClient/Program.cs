using System;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Phema.RabbitMQ.RawClient
{
	class Program
	{
		static void Main()
		{
			// Change to DispatchConsumersAsync = false 
			using (var connection = new ConnectionFactory { DispatchConsumersAsync = true }.CreateConnection())
			{
				using (var channel1 = connection.CreateModel())
				{
					//                   queue1 -> produce click to queue2
					// click -> exchange
					//                   queue2 -> consume click from queue1
					channel1.ExchangeDeclare("exchange", ExchangeType.Direct, autoDelete: true);
					channel1.QueueDeclare("queue1");
					channel1.QueueBind("queue1", "exchange", "queue1");
					channel1.QueueDeclare("queue2");
					channel1.QueueBind("queue2", "exchange", "queue2");

					// Change to EventingBasicConsumer
					var consumer1 = new AsyncEventingBasicConsumer(channel1);
					consumer1.Received += async (sender, args) =>
					{
						try
						{
							// Issue with opening channel in async consumers
							using (var channel2 = connection.CreateModel())
							{
								await Console.Out.WriteLineAsync("Click requested");
								channel2.BasicPublish("exchange", "queue2");
							}
						}
						catch (TimeoutException e)
						{
							Console.WriteLine(e);
							throw;
						}
					};
					channel1.BasicConsume(
						"queue1",
						autoAck: true,
						consumer1);

					// Change to EventingBasicConsumer
					var consumer2 = new AsyncEventingBasicConsumer(channel1);
					consumer2.Received += async (sender, args) =>
					{
						await Console.Out.WriteLineAsync("Click processed");
					};
					channel1.BasicConsume(
						"queue2",
						autoAck: true,
						consumer2);

					while (true)
					{
						channel1.BasicPublish(
							"exchange",
							"queue1");

						Console.WriteLine("Clicked");

						Thread.Sleep(1000);
					}
				}
			}

			// ReSharper disable once FunctionNeverReturns
		}
	}
}