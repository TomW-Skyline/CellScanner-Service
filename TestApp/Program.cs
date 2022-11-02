﻿namespace TestApp
{
	using System;
	using System.Diagnostics;
	using System.Threading;

	using CellScanner.API;

	using ProcessHandling;

	internal class Program
	{
		private const string DEBUG_TOKEN = "3C877D96-2E40-4CB0-84B1-68861C8777AE";

		static void Main(string[] args)
		{
			var serviceClient = new ServiceProxy(DEBUG_TOKEN);

			var version = serviceClient.Get_DLL_Version();
			Console.WriteLine("DLL version: " + version);

			serviceClient.Set_IP_Addr("");
			serviceClient.Set_GPS(false);
			LogEvents(serviceClient);

			var freqs = BuildFrequenciesList();
			serviceClient.SetFrequencies(freqs);
			LogEvents(serviceClient);

			serviceClient.StartMeasurement();
			LogEvents(serviceClient);

			// read events and measurements during 1 minutes
			var sw = Stopwatch.StartNew();
			while (sw.Elapsed < TimeSpan.FromMinutes(1))
			{
				LogEvents(serviceClient);
				LogMeasurements(serviceClient);
				Thread.Sleep(100);
			}

			Console.WriteLine("DONE");
			Console.ReadKey();
		}

		private static void LogEvents(ServiceProxy serviceClient)
		{
			foreach (var e in serviceClient.GetNewEvents())
			{
				Console.WriteLine($"[{e.Time}] {e.Severity}: {e.Message}");
			}
		}

		private static void LogMeasurements(ServiceProxy serviceClient)
		{
			foreach (var m in serviceClient.GetNewMeasurements())
			{
				Console.WriteLine($"Measurement: {m.CommonData}");
			}
		}

		private static TScannerFreqList BuildFrequenciesList()
		{
			var freqs = new[]
			{
				new TScannerFrequency
				{
					Band = 1,
					ChannelNumber = 1,
					FreqMHz = 632.55,
					Tech = TCellScannerTechnology.CST_5GNR,
					LTE_DuplexMode = TDuplexingMode.DM_NotApplicable,
					NR5G_SCS = T_5GNR_SCS.NSCS_15kHz,
				},
				new TScannerFrequency
				{
					Band = 2,
					ChannelNumber = 2,
					FreqMHz = 751.00,
					Tech = TCellScannerTechnology.CST_LTE,
					LTE_DuplexMode = TDuplexingMode.DM_FDD,
					NR5G_SCS = T_5GNR_SCS.NSCS_15kHz,
				},
				new TScannerFrequency
				{
					Band = 3,
					ChannelNumber = 3,
					FreqMHz = 876.80,
					Tech = TCellScannerTechnology.CST_UMTS,
					LTE_DuplexMode = TDuplexingMode.DM_NotApplicable,
					NR5G_SCS = T_5GNR_SCS.NSCS_15kHz,
				},
				new TScannerFrequency
				{
					Band = 4,
					ChannelNumber = 4,
					FreqMHz = 1969.00,
					Tech = TCellScannerTechnology.CST_GSM,
					LTE_DuplexMode = TDuplexingMode.DM_NotApplicable,
					NR5G_SCS = T_5GNR_SCS.NSCS_15kHz,
				},
			};

			return new TScannerFreqList(freqs);
		}
	}
}
