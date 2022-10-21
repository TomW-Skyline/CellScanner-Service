namespace CellScanner.API
{
	using System;
	using System.Runtime.InteropServices;

	public class TCellScannerMeasurement
	{
		private TCellScannerMeasurement()
		{

		}

		public static TCellScannerMeasurement FromPointer(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				throw new ArgumentException(nameof(ptr));
			}

			var commonData = Marshal.PtrToStructure<TMeasurementCommonData>(ptr);

			return new TCellScannerMeasurement()
			{
				CommonData = commonData,
				MeaInfo = ReadMeasurementInfo(commonData, ptr + Marshal.SizeOf(commonData)),
			};
		}

		public TMeasurementCommonData CommonData;

		public CellScanner_Mea_Base MeaInfo;

		public T GetMeasurementInfo<T>() where T : CellScanner_Mea_Base
		{
			if (MeaInfo is T meaInfo)
			{
				return meaInfo;
			}

			throw new InvalidOperationException($"Invalid generic type '{typeof(T).Name}', expected {MeaInfo.GetType().Name}");
		}

		private static CellScanner_Mea_Base ReadMeasurementInfo(TMeasurementCommonData commonData, IntPtr ptr)
		{
			switch (commonData.Tech)
			{
				case TCellScannerTechnology.CST_LTE: return Marshal.PtrToStructure<TCellScanner_LTE_Mea>(ptr);
				case TCellScannerTechnology.CST_UMTS: return Marshal.PtrToStructure<TCellScanner_UMTS_Mea>(ptr);
				case TCellScannerTechnology.CST_GSM: return Marshal.PtrToStructure<TCellScanner_GSM_Mea>(ptr);
				case TCellScannerTechnology.CST_5GNR: return Marshal.PtrToStructure<TCellScanner_5GNR_Mea>(ptr);
				default: throw new ArgumentOutOfRangeException($"Unknown technology: {commonData.Tech}");
			}
		}
	}
}