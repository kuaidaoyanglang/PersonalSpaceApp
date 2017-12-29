using System;
using System.Runtime.InteropServices;
namespace ConferenceCommon.RefreshSystemTrayHelper
{
	public static class Windows
	{
		[DllImport("User32.dll")]
		public static extern int FindWindow(string strClassName, string strWindowName);
		[DllImport("User32.dll")]
		public static extern int FindWindowEx(int hwndParent, int hwndChildAfter, string strClassName, string strWindowName);
		[DllImport("User32.dll", SetLastError = true)]
		public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
		[DllImport("User32.dll")]
		public static extern bool GetWindowRect(HandleRef hwnd, out HRect rect);
		[DllImport("User32.dll")]
		public static extern bool GetClientRect(IntPtr hWnd, out HRect lpRect);
		[DllImport("User32.dll")]
		public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);
	}
}
