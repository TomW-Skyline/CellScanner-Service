namespace CellScannerService
{
	using System;
	using System.Diagnostics;
	using System.ServiceModel;
	using System.ServiceModel.Description;
	using System.Windows.Threading;

	using Behaviors;

	using CellScanner;
	using CellScanner.Threading;
	using CellScanner.Tools;

	public class Program
	{
		private const string DEBUG_TOKEN = "3C877D96-2E40-4CB0-84B1-68861C8777AE";

		private static Service _service;
		private static ServiceHost _serviceHost;

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
				token = DEBUG_TOKEN;
				parentPid = -1;
			}

			var user = $"{Environment.UserDomainName}/{Environment.UserName}";
			Console.WriteLine($"Service is running as {user}");

			StartService(token);
			MonitorParentProcess(parentPid);

			// keep the program active
			Dispatcher.Run();
		}

		private static void StartService(string token)
		{
			var uri = new Uri($"net.pipe://localhost/CellScannerService/{token}");

			var thread = new CellScannerThread();

			_service = new Service(thread);
			_serviceHost = new ServiceHost(_service);

			_serviceHost.Faulted += (sender, args) =>
			{
				Console.WriteLine("Service host is in faulted state");
				Environment.Exit(10);
			};
			_serviceHost.Closed += (sender, args) =>
			{
				Console.WriteLine("Service host is closed");
				Environment.Exit(10);
			};

			EnableIncludeExceptionDetailInFaults(_serviceHost);

			var endpoint = _serviceHost.AddServiceEndpoint(typeof(IService), BindingFactory.GetBinding(), uri + "/");
			endpoint.Behaviors.Add(new ErrorHandlerBehavior());
			endpoint.Behaviors.Add(new MaxFaultSizeBehavior(Int32.MaxValue));

			_serviceHost.Open();

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
				Console.WriteLine("Parent process has exited");
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
