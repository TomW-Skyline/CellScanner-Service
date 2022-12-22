namespace TestApp
{
	using System;
	using System.Collections.Generic;
	using System.ServiceModel;

	using CellScanner;
	using CellScanner.API;
	using CellScanner.Data;
	using CellScanner.Tools;

	public class ServiceProxy : ClientBase<IService>, IService
	{
		private string _ipAddress;
		private bool? _gpsState;
		private bool _measurementState;
		private TScannerFreqList _currentFreqList;

		public ServiceProxy(string token)
			: base(BindingFactory.GetBinding(), new EndpointAddress($"net.pipe://localhost/CellScannerService/{token}"))
		{
		}

		public bool Ping()
		{
			return Channel.Ping();
		}

		public string GetServiceInfo()
		{
			return Channel.GetServiceInfo();
		}

		public ICollection<Event> GetNewEvents()
		{
			return Channel.GetNewEvents();
		}

		public int Get_DLL_Version()
		{
			return Channel.Get_DLL_Version();
		}

		public int RestartDevice()
		{
			return Channel.RestartDevice();
		}

		public int Set_IP_Addr(string ip)
		{
			if (!String.Equals(_ipAddress, ip))
			{
				_ipAddress = ip;
				return Channel.Set_IP_Addr(ip);
			}
			return 0;
		}

		public int Set_GPS(bool state)
		{
			if (_gpsState != state)
			{
				_gpsState = state;
				return Channel.Set_GPS(state);
			}

			return 0;
		}

		public int SetFrequencies(TScannerFreqList freqList)
		{
			if (freqList != _currentFreqList)
			{
				_currentFreqList = freqList;
				return Channel.SetFrequencies(freqList);
			}

			return 0;
		}

		public int StartMeasurement()
		{
			if (!_measurementState)
			{
				_measurementState = true;
				return Channel.StartMeasurement();
			}

			return 0;
		}

		public int StopMeasurement()
		{
			if (_measurementState)
			{
				_measurementState = false;
				return Channel.StopMeasurement();
			}

			return 0;
		}

		public ICollection<Measurement> GetNewMeasurements()
		{
			return Channel.GetNewMeasurements();
		}

		public void TestExternalGetMeasurement()
		{
			Channel.TestExternalGetMeasurement();
		}
	}
}
