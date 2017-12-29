using System;
using System.Runtime.InteropServices;
namespace ConferenceCommon.RefreshSystemTrayHelper
{
	public struct CopyDataStruct
	{
		public IntPtr dwData;
		public int cbData;
		[MarshalAs(UnmanagedType.LPStr)]
		public string lpData;
	}
}
