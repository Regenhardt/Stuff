using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Regenhardt
{
	public class MultiProcessRunner:IDisposable
	{
		#region [ Fields ]

		private Dictionary<Process, Action> processes; 

		#endregion

		#region [ Properties ]

		/// <summary>
		/// Count of stored processes.
		/// </summary>
		public int TotalProcesses => processes.Count;

		/// <summary>
		/// Count of active, stored processes.
		/// </summary>
		public int ActiveProcesses => processes.Where(p => !p.Key.HasExited).Count();

		/// <summary>
		/// Whether or not to automatically remove processes from the list when they exit.
		/// </summary>
		public bool AutoClean { get; set; }

		#endregion

		#region [ Initialization ]

		public MultiProcessRunner(bool autoClean = true)
		{
			AutoClean = autoClean;
			processes = new Dictionary<Process, Action>();
		}

		#endregion

		#region [ API ]

		/// <summary>
		/// Starts a new process.
		/// </summary>
		/// <param name="path">The application to execute.</param>
		/// <param name="runAsAdmin">Whether to run the process with elevated rights. This requires elevated rights or will trigger UAC.</param>
		/// <returns>Whether or not the process was successfully started.</returns>
		public bool AddPocess(string path, Action onExitHandler = null, bool runAsAdmin = false)
		{
			try
			{
				var p = new Process();
				p.StartInfo = new ProcessStartInfo(path);
				if (runAsAdmin)
					p.StartInfo.Verb = "runas";
				p.Exited += OnProcessExited;
				if (p.Start())
				{
					processes.Add(p, onExitHandler);
					return true;
				}
				return false;
			}
			catch (Exception)
			{
				return false;
			}
		}

		/// <summary>
		/// Remove all exited processes from the list.
		/// </summary>
		/// <returns>Number of cleaned up processes.</returns>
		public int Cleanup()
		{
			int i = 0;
			foreach (Process process in processes.Keys.Where(p => p.HasExited))
			{
				i++;
				processes.Remove(process);
			}
			return i;
		}

		/// <summary>
		/// Disposes of this instance. ALso kills and disposes of all stored processes.
		/// </summary>
		public void Dispose()
		{
			foreach (Process process in processes.Keys.Where(p => !p.HasExited))
			{
				process.Kill();
				process.Dispose();
			}
			processes.Clear();
			GC.SuppressFinalize(this);
		}

		~MultiProcessRunner()
		{
			foreach (Process process in processes.Keys.Where(p => !p.HasExited))
			{
				process.Kill();
				process.Dispose();
			}
			processes.Clear();
		}

		#endregion

		#region [ Internlas ]

		/// <summary>
		/// Remove process if set to autoclean.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void OnProcessExited(object sender, EventArgs args)
		{
			if(sender is Process p &&
				AutoClean)
			{
				processes[p]?.BeginInvoke(null, null);
				processes.Remove(p);
			}
		}

		#endregion

	}
}
