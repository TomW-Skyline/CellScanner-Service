namespace CellScanner.Threading
{
	using System;
	using System.Runtime.InteropServices;

	public static class Win32
	{
		public const int PM_NOREMOVE = 0x0000;
		public const int PM_REMOVE = 0x0001;
		public const int PM_NOYIELD = 0x0002;

		[DllImport("user32.dll")]
		public static extern bool GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);

		[DllImport("user32.dll")]
		public static extern bool PeekMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax, uint wRemoveMsg);

		[DllImport("user32.dll")]
		public static extern bool TranslateMessage(ref MSG lpMsg);

		[DllImport("user32.dll")]
		public static extern bool DispatchMessage(ref MSG lpMsg);

		[StructLayout(LayoutKind.Sequential)]

		public struct MSG
		{
			public IntPtr hwnd;
			public uint message;
			public UIntPtr wParam;
			public IntPtr lParam;
			public int time;
			public POINT pt;
			public int lPrivate;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int X;
			public int Y;
		}


	}
}
