namespace CellScannerService
{
	using System;
	using System.Timers;

	public class Watchdog
	{
		private readonly Timer _timer;

		public Watchdog(TimeSpan timeout)
		{
			Timeout = timeout;

			_timer = new Timer();
			_timer.Interval = timeout.TotalMilliseconds;
			_timer.Elapsed += Timer_Tick;
			_timer.Start();
		}

		public TimeSpan Timeout { get; }

		public DateTime LastSignaled { get; private set; }

		public void Signal()
		{
			_timer.Stop();
			_timer.Start();

			LastSignaled = DateTime.UtcNow;
		}

		private void Timer_Tick(object sender, ElapsedEventArgs e)
		{
			Environment.Exit(13);
		}
	}
}
