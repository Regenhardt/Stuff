using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Regenhardt.Tests
{
	[TestClass]
	public class MultiProcessRunnerTest
	{
		[TestMethod]
		public void NewRunner_IsEmpty()
		{
			var runner = new MultiProcessRunner();

			Assert.AreEqual(0, runner.ActiveProcesses);
			Assert.AreEqual(0, runner.TotalProcesses);
		}

		[TestMethod]
		public void Runner_WithoutAutoclean_KeepsProcess()
		{
			
		}
	}
}
