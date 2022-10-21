namespace CellScanner.Threading
{
	using System;

	public static class MessageLoop
	{
		public static void Run()
		{
			while (Win32.GetMessage(out var msg, IntPtr.Zero, 0, 0))
			{
				Win32.TranslateMessage(ref msg);
				Win32.DispatchMessage(ref msg);
			}
		}

		public static bool TryPeekAndDispatchMessage()
		{
			if (Win32.PeekMessage(out var msg, IntPtr.Zero, 0, 0, Win32.PM_REMOVE))
			{
				Win32.TranslateMessage(ref msg);
				Win32.DispatchMessage(ref msg);

				return true;
			}

			return false;
		}
	}
}
