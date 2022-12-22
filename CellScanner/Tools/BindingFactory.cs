namespace CellScanner.Tools
{
	using System;
	using System.ServiceModel;
	using System.ServiceModel.Channels;

	public static class BindingFactory
	{
		public static Binding GetBinding()
		{
			var binding = new NetNamedPipeBinding
			{
				OpenTimeout = TimeSpan.FromMinutes(15),
				SendTimeout = TimeSpan.FromMinutes(15),
				CloseTimeout = TimeSpan.FromMinutes(15),
				ReceiveTimeout = TimeSpan.MaxValue,
				MaxConnections = 200,
				MaxBufferSize = Int32.MaxValue,
				MaxReceivedMessageSize = Int32.MaxValue,
				MaxBufferPoolSize = Int32.MaxValue,
				
			};

			return binding;
		}
	}
}
