namespace CellScanner.Data
{
	using System.Runtime.Serialization;

	using CellScanner.API;

	[DataContract]
	public class Measurement
	{
		public Measurement(TCellScannerMeasurement mea)
		{
			CommonData = mea.CommonData;
		}

		[DataMember]
		public TMeasurementCommonData CommonData;

	}
}
