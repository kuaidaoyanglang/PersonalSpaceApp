using System;
namespace ConferenceCommon.RefreshSystemTrayHelper
{
	[Flags]
	public enum MouseEventFlags
	{
		Move = 1,
		LeftDown = 2,
		LeftUp = 4,
		RightDown = 8,
		RightUp = 16,
		MiddleDown = 32,
		MiddleUp = 64,
		Wheel = 2048,
		Absolute = 32768
	}
}
