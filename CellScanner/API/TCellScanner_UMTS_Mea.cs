namespace CellScanner.API
{
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Sequential)]
	public class TCellScanner_UMTS_Mea : CellScanner_Mea_Base
	{
		public int cell_id;
		public int psc;
		public double rscp;         // dBm
		public double eci0;         // dB
		public double rssi;         // dBm
		public double delay_spread; // ns

		public override string ToString()
		{
			return $"cell_id= {cell_id}  psc= {psc}  rscp= {rscp}  eci0= {eci0}  rssi= {rssi}  delay_spread= {delay_spread}";
		}
	}
}