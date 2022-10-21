namespace CellScanner.API
{
	public enum GetMeasurementResult
	{
		Success = 0,
		Failed = 2,
	}

	public enum TCellScannerTechnology
	{
		CST_LTE,
		CST_UMTS,
		CST_GSM,
		CST_5GNR,
	}

	public enum TDuplexingMode
	{
		DM_FDD,
		DM_TDD,
		DM_NotApplicable,
	}

	public enum T_5GNR_SCS
	{
		NSCS_15kHz,
		NSCS_30kHz,
		NSCS_120kHz,
		NSCS_240kHz,
	}
}
