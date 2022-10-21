namespace CellScanner.API
{
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public class TCellScanner_5GNR_Mea : CellScanner_Mea_Base
	{
		public int SCS; // sub-carrier spacing
		public int PCI;
		public int tot_SSB;

		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = CellScanner.MAX_5GNR_SSB)]
		public T_5GNR_SSB_MeaInfo[] SSB;

		public override string ToString()
		{
			return $"SCS= {SCS}  PCI= {PCI}  tot_SSB= {tot_SSB}";
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct T_5GNR_SSB_MeaInfo
	{
		public int iSSB;
		public double ss_rsrp;     // dBm
		public double ss_rsrq;     // dB
		public double ss_sinr;     // dB
		public double ps_rsrp;     // dBm
		public double ps_sinr;     // dB
		public double ss_rssi;     // dBm
		public double noise_power; // dBm

		public override string ToString()
		{
			return $"index={iSSB}  ss_rsrp= {ss_rsrp}  ss_rsrq= {ss_rsrq}  ss_sinr= {ss_sinr}  ps_rsrp= {ps_rsrp}  ps_sinr= {ps_sinr}  ss_rssi= {ss_rssi}  noise_power= {noise_power}";
		}
	}
}