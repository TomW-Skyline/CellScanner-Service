namespace CellScanner.API
{
	using System.Runtime.InteropServices;
	using System.Runtime.Serialization;

	[DataContract]
	[StructLayout(LayoutKind.Sequential)]
	public struct TMeasurementCommonData
	{
		[DataMember]
		public double latitude;

		[DataMember]
		public double longitude;

		[DataMember]
		public SystemTime timeStamp;

		[DataMember]
		public TCellScannerTechnology Tech;

		/// <summary>
		/// optional information; it contains the value configured in the TScannerFreqList
		/// </summary>
		[DataMember]
		public int Band;

		/// <summary>
		/// optional information; it contains the value configured in the TScannerFreqList
		/// </summary>
		[DataMember]
		public int ChannelNumber;

		/// <summary>
		/// center frequency (for 5G corresponds to the SSB frequency); it contains the value configured in the ScannerFreqList
		/// </summary>
		[DataMember]
		public double FreqMHz;

		/// <summary>
		/// // this data is for internal use (it can be ignored)
		/// </summary>
		[DataMember]
		public int BlockCnt;

		public override string ToString()
		{
			return $"Tech= {Tech}  Lat= {latitude}  Long= {longitude}  Time= {timeStamp}  Band= {Band}  CN= {ChannelNumber}  FreqMHz= {FreqMHz}  BlockCnt= {BlockCnt}";
		}
	}
}