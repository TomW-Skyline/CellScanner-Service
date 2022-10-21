namespace ProcessHandling
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;

	using CellScanner.Data;

	public class CellScannerServiceProcess : IDisposable
	{
		private bool _isDisposed;
		private Process _process;

		private readonly ConcurrentQueue<Event> _events = new ConcurrentQueue<Event>();

		public CellScannerServiceProcess(string token)
		{
			if (String.IsNullOrWhiteSpace(token)) throw new ArgumentException("Token cannot be null or whitespace.");
			Token = token;
		}

		public string Token { get; }

		public int? Id => _process?.Id;

		public void StartProcess()
		{
			StopProcess();

			var parentPid = Process.GetCurrentProcess().Id;

			try
			{
				CheckRequiredFiles();

				var startInfo = new ProcessStartInfo
				{
					FileName = "CellScannerService.exe",
					Arguments = String.Join(" ", Token, parentPid),
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true,
				};

				_process = new Process { StartInfo = startInfo };
				_process.OutputDataReceived += Process_OnOutputDataReceived;
				_process.ErrorDataReceived += Process_OnErrorDataReceived;

				_process.Start();
				_process.BeginOutputReadLine();
				_process.BeginErrorReadLine();
			}
			catch (Exception)
			{
				_process = null;
				throw;
			}
		}

		public void StopProcess()
		{
			if (_process == null)
			{
				return;
			}

			try
			{
				_process.Kill();
			}
			catch (Exception)
			{
				// ignore
			}
			finally
			{
				_process.Dispose();
				_process = null;
			}
		}

		public bool HasExited(out int exitCode)
		{
			if (_process == null)
			{
				exitCode = -1;
				return false;
			}

			if (_process.HasExited)
			{
				exitCode = _process.ExitCode;
				return true;
			}
			else
			{
				exitCode = -1;
				return false;
			}
		}

		public ICollection<Event> GetNewEvents()
		{
			var list = new List<Event>();

			while (_events.TryDequeue(out var e))
			{
				list.Add(e);
			}

			return list;
		}

		private void CheckRequiredFiles()
		{
			var files = new[] { "CellScanner.dll", "CellScannerService.exe", "CellScanner64.dll" };

			foreach (string file in files)
			{
				if (!File.Exists(file))
				{
					throw new FileNotFoundException($"Couldn't find file '{file}'");
				}
			}
		}

		private void Process_OnOutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (String.IsNullOrWhiteSpace(e.Data))
			{
				return;
			}

			_events.Enqueue(new Event { Message = $"> {e.Data}", Severity = EventSeverity.Information, Time = DateTime.UtcNow });
		}

		private void Process_OnErrorDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (String.IsNullOrWhiteSpace(e.Data))
			{
				return;
			}

			_events.Enqueue(new Event { Message = $"> {e.Data}", Severity = EventSeverity.Error, Time = DateTime.UtcNow });
		}

		#region IDisposable

		~CellScannerServiceProcess()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed)
			{
				return;
			}

			StopProcess();

			_isDisposed = true;
		}

		#endregion
	}
}
