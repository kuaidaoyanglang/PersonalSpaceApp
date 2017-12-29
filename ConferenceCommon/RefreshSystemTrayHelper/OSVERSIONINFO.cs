using System;
using System.Runtime.InteropServices;
namespace ConferenceCommon.RefreshSystemTrayHelper
{
	public struct OSVERSIONINFO
	{
		public uint dwOSVersionInfoSize;
		public uint dwMajorVersion;
		public uint dwMinorVersion;
		public uint dwBuildNumber;
		public uint dwPlatformId;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string szCSDVersion;
		public short wServicePackMajor;
		public short wServicePackMinor;
		public short wSuiteMask;
		public byte wProductType;
		public byte wReserved;
	}
}
