using System;
using System.Runtime.InteropServices;
using System.Security;
namespace ConferenceCommon.RefreshSystemTrayHelper
{
	public static class Message
	{
		public static IntPtr MakeLParam(int LoWord, int HiWord)
		{
			return (IntPtr)(HiWord << 16 | (LoWord & 65535));
		}
		[DllImport("User32.dll")]
		public static extern int SendMessage(int hWnd, int Msg, int wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);
		[DllImport("User32.dll")]
		public static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);
		[DllImport("User32.dll")]
		public static extern int SendMessage(int hWnd, int Msg, int wParam, uint lParam);
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern int SendMessage(int hWnd, int msg, int wParam, IntPtr lParam);
		[DllImport("User32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, ref CopyDataStruct lParam);
		[SuppressUnmanagedCodeSecurity]
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern bool PeekMessage(out Msg msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);
	}
}
