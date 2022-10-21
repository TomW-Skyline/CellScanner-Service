namespace CellScanner.Threading
{
	using System;
	using System.Collections.Concurrent;
	using System.Threading;

	public class CellScannerThread
	{
		private readonly Thread _thread;
		private readonly ConcurrentQueue<WorkItem> _queue = new ConcurrentQueue<WorkItem>();

		public CellScannerThread()
		{
			_thread = new Thread(Thread) { Name = "CellScanner Helper Thread" };
			_thread.Start();
		}

		public void Invoke(Action callback)
		{
			var workItem = new WorkItem(callback);
			_queue.Enqueue(workItem);
			workItem.Wait();
		}

		public TResult Invoke<TResult>(Func<TResult> callback)
		{
			TResult result = default;

			void action()
			{
				result = callback();
			}
			Invoke(action);

			return result;
		}

		private void Thread()
		{
			while (true)
			{
				if (MessageLoop.TryPeekAndDispatchMessage())
				{
					continue;
				}

				if (_queue.TryDequeue(out var item))
				{
					item.Execute();
					continue;
				}

				System.Threading.Thread.Sleep(10);
			}
		}
	}
}
