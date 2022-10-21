namespace CellScanner.Data
{
	using System;
	using System.Runtime.Serialization;

	[DataContract]
	public class Event
	{
		[DataMember] 
		public EventSeverity Severity { get; set; }

		[DataMember]
		public DateTime Time { get; set; }

		[DataMember] 
		public string Message { get; set; }
	}

	[DataContract]
	public enum EventSeverity
	{
		[EnumMember] Information,
		[EnumMember] Error,
	}
}
