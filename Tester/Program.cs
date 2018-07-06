using System;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Regenhardt
{
	class Program
	{
		static void Main(string[] args)
		{
			using (var runner = new MultiProcessRunner())
			{
				var task = new Task(() => { });
				runner.AddPocess("cmd.exe", task, true);
				Console.WriteLine("Now waiting");

				task.Wait();
				Console.WriteLine("Other process exited.");

				Console.ReadKey();
			}
		}
	}
}
