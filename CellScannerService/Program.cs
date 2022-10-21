namespace CellScannerService
{
	using System;
	using System.Diagnostics;
	using System.ServiceModel;
	using System.ServiceModel.Description;

	using Behaviors;

	using CellScanner;
	using CellScanner.Threading;

	public class Program
	{
		public static void Main(string[] args)
		{
			string token;
			int parentPid;

			if (args.Length >= 2)
			{
				token = args[0];
				parentPid = Int32.Parse(args[1]);
			}
			else
			{
				token = Guid.NewGuid().ToString();
				parentPid = -1;
			}

			var watchdog = new Watchdog(TimeSpan.FromMinutes(2));

			StartService(token, watchdog);
			MonitorParentProcess(parentPid);

			// keep the program active
			MessageLoop.Run();
		}

		private static void StartService(string token, Watchdog watchdog)
		{
			var uri = new Uri($"net.pipe://localhost/CellScannerService/{token}");

			var thread = new Threading.CellScannerThread();
			var service = new Service(thread, watchdog);
			var serviceHost = new ServiceHost(service, uri);
			serviceHost.Faulted += (sender, args) =>
			{
				Console.Error.WriteLine("Service host is in faulted state");
				Environment.Exit(10);
			};

			EnableIncludeExceptionDetailInFaults(serviceHost);

			var endpoint = serviceHost.AddServiceEndpoint(typeof(IService), new NetNamedPipeBinding(), "/");
			endpoint.Behaviors.Add(new ErrorHandlerBehavior());
			endpoint.Behaviors.Add(new MaxFaultSizeBehavior(Int32.MaxValue));

			serviceHost.Open();
			
			Console.WriteLine($"Service started (endpoint={endpoint.Address.Uri})");
		}

		private static void MonitorParentProcess(int pid)
		{
			if (pid < 0)
			{
				return;
			}

			var process = Process.GetProcessById(pid);

			process.EnableRaisingEvents = true;
			process.Exited += (sender, args) =>
			{
				Console.Error.WriteLine("Parent process has exited");
				Environment.Exit(12);
			};
		}

		private static void EnableIncludeExceptionDetailInFaults(ServiceHost host)
		{
			var debugBehavior = host.Description.Behaviors.Find<ServiceDebugBehavior>();

			if (debugBehavior == null)
			{
				debugBehavior = new ServiceDebugBehavior();
				host.Description.Behaviors.Add(debugBehavior);
			}

			debugBehavior.IncludeExceptionDetailInFaults = true;
		}
	}
}
