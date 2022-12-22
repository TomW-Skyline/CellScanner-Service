namespace CellScannerService
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.ServiceModel;
	using System.Threading;
	using System.Windows.Threading;

	using CellScanner;
	using CellScanner.API;
	using CellScanner.Data;

	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single, UseSynchronizationContext = false)]
	public class Service : IService
	{
		private readonly Dispatcher _dispatcher;

		private readonly ConcurrentQueue<Event> _events = new ConcurrentQueue<Event>();
		private readonly ConcurrentQueue<Measurement> _measurements = new ConcurrentQueue<Measurement>();

		public Service()
		{
			_dispatcher = CreateDispatcherThread();

			// CellScanner actions need to run on a thread (on which a Dispatcher runs)
			_dispatcher.Invoke(() =>
			{
				CellScanner.DefineExternalLogMsg(x => Log_CallBack(x, EventSeverity.Information));
				CellScanner.DefineExternalShowErrorMsg(x => Log_CallBack(x, EventSeverity.Error));
				CellScanner.DefineExternalGetMeasurement(GetMeasurement_CallBack);

				CellScanner.SetMeasurementInterval(5000);
			});
		}

		public Dispatcher Dispatcher => _dispatcher;

		/// <summary>
		/// CellScanner requires a thread that processes Windows messages.
		/// Without this, some API methods will hang.
		/// </summary>
		/// <returns></returns>
		private static Dispatcher CreateDispatcherThread()
		{
			Dispatcher dispatcher = null;

			var mre = new ManualResetEvent(false);

			var thread = new Thread(() =>
			{
				dispatcher = Dispatcher.CurrentDispatcher;
				SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(dispatcher));

				mre.Set();
				Dispatcher.Run();
			});
			thread.Name = "CellScanner Thread";
			thread.Start();

			// wait until the thread runs
			mre.WaitOne();

			return dispatcher;
		}

		public bool Ping()
		{
			return true;
		}

		public string GetServiceInfo()
		{
			var user = $"{Environment.UserDomainName}/{Environment.UserName}";

			return $"Service is running as {user}";
		}

		public int Get_DLL_Version()
		{
			return _dispatcher.Invoke(CellScanner.Get_DLL_Version);
		}

		public int RestartDevice()
		{
			return _dispatcher.Invoke(CellScanner.RestartDevice);
		}

		public int Set_IP_Addr(string ip)
		{
			if (String.IsNullOrWhiteSpace(ip)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(ip));

			return _dispatcher.Invoke(() => CellScanner.Set_IP_Addr(ip));
		}

		public int Set_GPS(bool state)
		{
			return _dispatcher.Invoke(() => CellScanner.Set_GPS(state));
		}

		public int SetFrequencies(TScannerFreqList freqList)
		{
			return _dispatcher.Invoke(() => CellScanner.SetFrequencies(freqList));
		}

		public int StartMeasurement()
		{
			return _dispatcher.Invoke(CellScanner.StartMeasurement);
		}

		public int StopMeasurement()
		{
			return _dispatcher.Invoke(CellScanner.StopMeasurement);
		}

		public ICollection<Measurement> GetNewMeasurements()
		{
			var list = new List<Measurement>();

			while (_measurements.TryDequeue(out var e))
			{
				list.Add(e);
			}

			return list;
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

		public void TestExternalGetMeasurement()
		{
			CellScanner.TestExternalGetMeasurement();
		}

		private void GetMeasurement_CallBack(IntPtr meas_ptr)
		{
			var meas = TCellScannerMeasurement.FromPointer(meas_ptr);

			Log_CallBack("Measurement (CB): " + meas.CommonData + " - " + meas.MeaInfo, EventSeverity.Information);
		}

		private void Log_CallBack(string msg, EventSeverity severity)
		{
			if (String.IsNullOrWhiteSpace(msg))
			{
				return;
			}

			Console.WriteLine(msg);

			_events.Enqueue(new Event { Message = msg, Time = DateTime.UtcNow, Severity = severity });
		}
	}
}