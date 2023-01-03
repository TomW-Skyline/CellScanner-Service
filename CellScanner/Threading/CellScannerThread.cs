namespace CellScanner.Threading
{
	using System;
	using System.Threading;
	using System.Windows.Threading;

	/// <summary>
	/// CellScanner requires a thread that processes Windows messages.
	/// Without this, some API methods will hang.
	/// </summary>
	public class CellScannerThread
	{
		private readonly Thread _thread;
		private readonly Dispatcher _dispatcher;

		public CellScannerThread()
		{
			Dispatcher dispatcher = null;
			var mre = new ManualResetEvent(false);

			void action()
			{
				dispatcher = Dispatcher.CurrentDispatcher;
				mre.Set();

				// this is a blocking call that keeps processing work items on this thread
				Dispatcher.Run();
			}

			var thread = new Thread(action);
			thread.Name = "CellScanner Thread";
			thread.Start();

			// wait until the thread runs
			mre.WaitOne();

			_thread = thread;
			_dispatcher = dispatcher;
		}

		public Dispatcher Dispatcher => _dispatcher;

		public void Invoke(Action action) => _dispatcher.Invoke(action);

		public TResult Invoke<TResult>(Func<TResult> func) => _dispatcher.Invoke(func);
	}
}