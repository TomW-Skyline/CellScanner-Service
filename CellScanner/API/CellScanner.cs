namespace CellScanner.API
{
	using System;
	using System.Runtime.InteropServices;

	public static class CellScanner
	{
		public const int MAX_SCANNER_FREQS = 512;
		public const int MAX_5GNR_SSB = 64;

		[UnmanagedFunctionPointer(CallingConvention.StdCall)] 
		public delegate void TExternalGetMeasurement(IntPtr measurement);

		[UnmanagedFunctionPointer(CallingConvention.StdCall)] 
		public delegate void TApplicationLogMsg(string msg);

		#region Main Functions

		[DllImport(@"CellScanner64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern int Set_IP_Addr([MarshalAs(UnmanagedType.LPStr)] string ip);

		[DllImport(@"CellScanner64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern int Set_GPS(bool state);

		[DllImport(@"CellScanner64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern int SetFrequencies(TScannerFreqList freqList);

		[DllImport(@"CellScanner64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern int StartMeasurement();

		[DllImport(@"CellScanner64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern int StopMeasurement();

		[DllImport(@"CellScanner64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern GetMeasurementResult GetMeasurement(ref TCellScannerMeasurement meas, int timeout);

		#endregion

		#region Support Functions

		[DllImport(@"CellScanner64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern int Get_DLL_Version();

		[DllImport(@"CellScanner64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern IntPtr GetLastErrorString();

		[DllImport(@"CellScanner64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern int RestartDevice();

		[DllImport(@"CellScanner64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern void DefineExternalGetMeasurement(TExternalGetMeasurement log);

		[DllImport(@"CellScanner64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern void DefineExternalLogMsg(TApplicationLogMsg log);

		[DllImport(@"CellScanner64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern void DefineExternalShowErrorMsg(TApplicationLogMsg log);

		[DllImport(@"CellScanner64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern int SetMeasurementInterval(int ms);

		[DllImport(@"CellScanner64.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern void TestExternalGetMeasurement();

		#endregion
	}
}
