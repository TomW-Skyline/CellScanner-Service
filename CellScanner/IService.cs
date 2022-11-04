namespace CellScanner
{
	using System.Collections.Generic;
	using System.ServiceModel;

	using CellScanner.API;
	using CellScanner.Data;

	[ServiceContract]
	public interface IService
	{
		[OperationContract]
		bool Ping();

		[OperationContract]
		string GetServiceInfo();

		[OperationContract]
		int Get_DLL_Version();

		[OperationContract]
		int RestartDevice();

		[OperationContract] 
		int Set_IP_Addr(string ip);

		[OperationContract]
		int Set_GPS(bool state);

		[OperationContract]
		int SetFrequencies(TScannerFreqList freqList);

		[OperationContract] 
		int StartMeasurement();

		[OperationContract]
		int StopMeasurement();

		[OperationContract]
		ICollection<Measurement> GetNewMeasurements();

		[OperationContract]
		ICollection<Event> GetNewEvents();

		[OperationContract]
		void TestExternalGetMeasurement();
	}
}
