namespace CellScanner.API
{
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public class TCellScanner_LTE_Mea : CellScanner_Mea_Base
	{
		public int PCI;
		public double RSSI_wb;     // dBm
		public double RSSI_crs;    // dBm
		public double RSSI_mib;    // dBm
		public double RSRP;        // dBm
		public double RSRQ;        // dB
		public double noise_power; // dBm
		public double CINR;        // dB
		public TDuplexingMode DuplexMode;

		public override string ToString()
		{
			return $"PCI= {PCI}  RSSI_wb= {RSSI_wb}  RSSI_crs= {RSSI_crs} RSSI_mib= {RSSI_mib}  RSRP= {RSRP}  RSRQ= {RSRQ}  NoisePower= {noise_power}  CINR= {CINR}  DuplexMode= {DuplexMode}";
		}
	}
}