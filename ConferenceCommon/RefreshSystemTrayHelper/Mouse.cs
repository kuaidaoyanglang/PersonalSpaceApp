using System;
using System.Runtime.InteropServices;
namespace ConferenceCommon.RefreshSystemTrayHelper
{
	public static class Mouse
	{
		[DllImport("User32.dll")]
		public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, IntPtr dwExtraInfo);
		[DllImport("User32.dll")]
		public static extern int ShowCursor(bool bShow);
		[DllImport("User32.dll")]
		public static extern void SetCursorPos(int x, int y);
		[DllImport("User32.dll")]
		public static extern bool GetCursorPos(out HPoint p);
	}
}
