using System;
using System.Threading.Tasks;

namespace Regenhardt
{
	public class Program
	{
		public static void Main(string[] args)
		{
			using (var runner = new MultiProcessRunner())
			{
				var task = new Task(() => { });
				runner.AddPocess("cmd.exe", task);
				Console.WriteLine("Now waiting");

				task.Wait();
				Console.WriteLine("Other process exited.");

				Console.ReadKey();
			}
		}
	}
}
