namespace CellScanner.API
{
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public class TCellScanner_GSM_Mea : CellScanner_Mea_Base
	{
		public int cell_id;
		public double bsic;
		public double rssi; // dBm
		public double ber;  // percentage (%)
		public double CI;   // dB

		public override string ToString()
		{
			return $"cell_id= {cell_id}  bsic= {bsic}  rssi= {rssi}  ber= {ber}  CI= {CI}";
		}
	}
}