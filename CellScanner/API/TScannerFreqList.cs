namespace CellScanner.API
{
	using System;
	using System.Linq;
	using System.Runtime.InteropServices;
	using System.Runtime.Serialization;

	[DataContract]
	[StructLayout(LayoutKind.Sequential)]
	public struct TScannerFreqList : IEquatable<TScannerFreqList>
	{
		public TScannerFreqList(TScannerFrequency[] freqs) : this()
		{
			if (freqs.Length > CellScanner.MAX_SCANNER_FREQS) throw new ArgumentException(nameof(freqs));

			TotalFreqs = freqs.Length;

			Freqs = new TScannerFrequency[CellScanner.MAX_SCANNER_FREQS];
			Array.Copy(freqs, Freqs, TotalFreqs);
		}

		[DataMember]
		[MarshalAs(UnmanagedType.I4)]
		public int TotalFreqs;

		[DataMember]
		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = CellScanner.MAX_SCANNER_FREQS)]
		public readonly TScannerFrequency[] Freqs;

		public bool Equals(TScannerFreqList other)
		{
			if (TotalFreqs != other.TotalFreqs)
				return false;

			if (Freqs == null && other.Freqs == null)
				return true;

			if (Freqs == null || other.Freqs == null) 
				return false;

			return Freqs.SequenceEqual(other.Freqs);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int hash = TotalFreqs;

				foreach (var f in Freqs)
				{
					hash += (13 * hash) + f.GetHashCode();
				}
				
				return hash;
			}
		}

		public static bool operator ==(TScannerFreqList x, TScannerFreqList y)
		{
			return x.Equals(y);
		}

		public static bool operator !=(TScannerFreqList x, TScannerFreqList y)
		{
			return !(x == y);
		}
	}
}