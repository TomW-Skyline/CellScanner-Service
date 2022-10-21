namespace CellScannerService
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.ServiceModel;

	using CellScanner;
	using CellScanner.API;
	using CellScanner.Data;

	using Threading;

	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class Service : IService
	{
		private readonly CellScannerThread _thread;
		private readonly Watchdog _watchdog;

		private readonly ConcurrentQueue<Event> _events = new ConcurrentQueue<Event>();
		private readonly ConcurrentQueue<Measurement> _measurements = new ConcurrentQueue<Measurement>();

		public Service(CellScannerThread thread, Watchdog watchdog)
		{
			_thread = thread ?? throw new ArgumentNullException(nameof(thread));
			_watchdog = watchdog ?? throw new ArgumentNullException(nameof(watchdog));

			// CellScanner actions need to run on a thread (on which a Dispatcher runs)
			_thread.Invoke(() =>
			{
				CellScanner.DefineExternalLogMsg(x => Log_CallBack(x, EventSeverity.Information));
				CellScanner.DefineExternalShowErrorMsg(x => Log_CallBack(x, EventSeverity.Error));
				CellScanner.DefineExternalGetMeasurement(GetMeasurement_CallBack);

				CellScanner.SetMeasurementInterval(5000);
			});
		}

		public bool Ping()
		{
			_watchdog.Signal();
			return true;
		}

		public int Get_DLL_Version()
		{
			return _thread.Invoke(CellScanner.Get_DLL_Version);
		}

		public int RestartDevice()
		{
			return _thread.Invoke(CellScanner.RestartDevice);
		}

		public int Set_IP_Addr(string ip)
		{
			return _thread.Invoke(() => CellScanner.Set_IP_Addr(ip));
		}

		public int Set_GPS(bool state)
		{
			return _thread.Invoke(() => CellScanner.Set_GPS(state));
		}

		public int SetFrequencies(TScannerFreqList freqList)
		{
			return _thread.Invoke(() => CellScanner.SetFrequencies(freqList));
		}

		public int StartMeasurement()
		{
			return _thread.Invoke(CellScanner.StartMeasurement);
		}

		public int StopMeasurement()
		{
			return _thread.Invoke(CellScanner.StopMeasurement);
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

			Log_CallBack("Measurement (CB): " + meas.CommonData + " - " + meas.MeaInfo , EventSeverity.Information);
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