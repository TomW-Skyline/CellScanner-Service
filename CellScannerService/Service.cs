namespace CellScannerService
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.ServiceModel;

	using CellScanner;
	using CellScanner.API;
	using CellScanner.Data;
	using CellScanner.Threading;

	[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
	public class Service : IService
	{
		private readonly CellScannerThread _thread;

		private readonly ConcurrentQueue<Event> _events = new ConcurrentQueue<Event>();
		private readonly ConcurrentQueue<Measurement> _measurements = new ConcurrentQueue<Measurement>();

		private readonly CellScanner.TApplicationLogMsg _pin_Log_CallBack;
		private readonly CellScanner.TApplicationLogMsg _pin_Error_CallBack;
		private readonly CellScanner.TExternalGetMeasurement _pin_GetMeasurement_CallBack;

		public Service(CellScannerThread thread)
		{
			_thread = thread;

			// important to keep a reference to these delegates, to make sure that CellScanner can keep calling them
			_pin_Log_CallBack = Log_CallBack;
			_pin_Error_CallBack = Error_CallBack;
			_pin_GetMeasurement_CallBack = GetMeasurement_CallBack;

			// CellScanner actions need to run on a thread (on which a Dispatcher runs)
			_thread.Invoke(() =>
			{

				CellScanner.DefineExternalLogMsg(_pin_Log_CallBack);
				CellScanner.DefineExternalShowErrorMsg(_pin_Error_CallBack);
				CellScanner.DefineExternalGetMeasurement(_pin_GetMeasurement_CallBack);

				CellScanner.SetMeasurementInterval(5000);
			});
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
			return _thread.Invoke(CellScanner.Get_DLL_Version);
		}

		public int RestartDevice()
		{
			return _thread.Invoke(CellScanner.RestartDevice);
		}

		public int Set_IP_Addr(string ip)
		{
			if (String.IsNullOrWhiteSpace(ip)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(ip));

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

			LogEvent("Measurement (CB): " + meas.CommonData + " - " + meas.MeaInfo, EventSeverity.Information);
		}

		private void Log_CallBack(string msg)
		{
			LogEvent(msg, EventSeverity.Information);
		}

		private void Error_CallBack(string msg)
		{
			LogEvent(msg, EventSeverity.Error);
		}

		private void LogEvent(string msg, EventSeverity severity)
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