using System;
using System.Threading;

namespace Regenhardt
{
	class Program
	{
		static void Main(string[] args)
		{

			var runner = new MultiProcessRunner();

			object mutex = new object();
			Monitor.Enter(mutex);

			runner.AddPocess("cmd.exe", () => Monitor.Exit(mutex));
			Console.WriteLine("Now waiting");

			lock (mutex)
			{
				Console.WriteLine("Other process exited.");
			}

			Console.ReadKey();
		}
	}
}
