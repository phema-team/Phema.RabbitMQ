using BenchmarkDotNet.Running;

namespace Phema.RabbitMQ.Benchmarks
{
	public class Program
	{
		public static void Main()
		{
			BenchmarkRunner.Run(typeof(Program).Assembly);
		}
	}
}