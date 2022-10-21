namespace CellScanner.API
{
	using System.Runtime.InteropServices;
	using System.Runtime.Serialization;

	[DataContract]
	[StructLayout(LayoutKind.Sequential)]
	public struct TScannerFrequency
	{
		/// <summary>
		/// optional information; this value will be reported as part of the measurement results
		/// </summary>
		[DataMember]
		public int Band;

		/// <summary>
		/// optional information; this value will be reported as part of the measurement results
		/// </summary>
		[DataMember]
		public int ChannelNumber;

		/// <summary>
		/// center frequency (for 5G corresponds to the SSB frequency)
		/// </summary>
		[DataMember]
		public double FreqMHz;

		/// <summary>
		/// 
		/// </summary>
		[DataMember]
		public TCellScannerTechnology Tech;

		/// <summary>
		/// information required for LTE channels
		/// </summary>
		[DataMember]
		public TDuplexingMode LTE_DuplexMode;

		/// <summary>
		/// information required for 5G channels
		/// </summary>
		[DataMember]
		public T_5GNR_SCS NR5G_SCS;
	};
}