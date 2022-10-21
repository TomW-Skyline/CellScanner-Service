namespace CellScannerService.Behaviors
{
	using System;
	using System.ServiceModel;
	using System.ServiceModel.Channels;
	using System.ServiceModel.Description;
	using System.ServiceModel.Dispatcher;

	public class ErrorHandlerBehavior : IEndpointBehavior
	{
		public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		{
			endpointDispatcher.ChannelDispatcher.ErrorHandlers.Add(new ErrorHandler());
		}

		public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
			// empty
		}

		public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
			// empty
		}

		public void Validate(ServiceEndpoint endpoint)
		{
			// empty
		}
	}

	public class ErrorHandler : IErrorHandler
	{
		public bool HandleError(Exception error)
		{
			// yes we handle this error
			return true;
		}

		public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
		{
			if (fault == null)
			{
				var faultException = new FaultException("An unhandled exception occurred: " + error.Message);
				var messageFault = faultException.CreateMessageFault();
				fault = Message.CreateMessage(version, messageFault, faultException.Action);
			}
		}
	}
}