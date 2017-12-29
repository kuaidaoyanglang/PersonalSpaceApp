using System;
namespace ConferenceCommon.RefreshSystemTrayHelper
{
	public struct Msg
	{
		public IntPtr hWnd;
		public uint msg;
		public IntPtr wParam;
		public IntPtr lParam;
		public uint time;
	}
}
