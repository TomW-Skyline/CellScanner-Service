namespace CellScannerService.Threading
{
	using System;
	using System.Threading;

	internal class WorkItem
	{
		private readonly Action _action;
		private readonly ManualResetEvent _mre = new ManualResetEvent(false);

		public WorkItem(Action action)
		{
			_action = action;
		}

		public void Execute()
		{
			_action();
			_mre.Set();
		}

		public void Wait()
		{
			_mre.WaitOne();
		}
	}
}