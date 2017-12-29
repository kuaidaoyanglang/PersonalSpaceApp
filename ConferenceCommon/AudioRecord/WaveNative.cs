

using ConferenceCommon.LogHelper;
using System;
using System.Runtime.InteropServices;

namespace ConferenceCommon.AudioRecord
{
	public enum WaveFormats
	{
		Pcm = 1,
		Float = 3
	}
    public enum Quality
    {
        low = 1,
        Normal = 0,
        Height = 2
    }

	[StructLayout(LayoutKind.Sequential)] 
	public class WaveFormat
	{
		public short wFormatTag;
		public short nChannels;
		public int nSamplesPerSec;
		public int nAvgBytesPerSec;
		public short nBlockAlign;
		public short wBitsPerSample;
		public short cbSize;

		public WaveFormat(int rate, int bits, int channels)
		{
            try
            {
			wFormatTag = (short)WaveFormats.Pcm;
			nChannels = (short)channels;
			nSamplesPerSec = rate;
			wBitsPerSample = (short)bits;
			cbSize = 0;
               
			nBlockAlign = (short)(channels * (bits / 8));
			nAvgBytesPerSec = nSamplesPerSec * nBlockAlign;
                 }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
		}
	}

	internal class WaveNative
	{
		// consts
		public const int MMSYSERR_NOERROR = 0; // no error

		public const int MM_WOM_OPEN = 0x3BB;
		public const int MM_WOM_CLOSE = 0x3BC;
		public const int MM_WOM_DONE = 0x3BD;

		public const int MM_WIM_OPEN = 0x3BE;
		public const int MM_WIM_CLOSE = 0x3BF;
		public const int MM_WIM_DATA = 0x3C0;

		public const int CALLBACK_FUNCTION = 0x00030000;    // dwCallback is a FARPROC 

        public const int TIME_MS = 0x0001;  // 时间以毫秒为单位 
        public const int TIME_SAMPLES = 0x0002;  // 波的样本数 
        public const int TIME_BYTES = 0x0004;  // 当前的字节偏移量 

		// callbacks
		public delegate void WaveDelegate(IntPtr hdrvr, int uMsg, int dwUser, ref WaveHdr wavhdr, int dwParam2);

		// structs 

		[StructLayout(LayoutKind.Sequential)] public struct WaveHdr
		{
            public IntPtr lpData; // 数据缓冲区指针的锁
            public int dwBufferLength; // 数据缓冲区的长度
            public int dwBytesRecorded; // 用于输入
            public IntPtr dwUser; // 用户的使用
			public int dwFlags; // assorted flags (see defines)
			public int dwLoops; // 循环控制计数器
			public IntPtr lpNext; // 放音(用于驱动)
            public int reserved; // 用于驱动
		}

		private const string mmdll = "winmm.dll";

		// WaveOut calls
		[DllImport(mmdll)]
		public static extern int waveOutGetNumDevs();
		[DllImport(mmdll)]
		public static extern int waveOutPrepareHeader(IntPtr hWaveOut, ref WaveHdr lpWaveOutHdr, int uSize);
		[DllImport(mmdll)]
		public static extern int waveOutUnprepareHeader(IntPtr hWaveOut, ref WaveHdr lpWaveOutHdr, int uSize);
		[DllImport(mmdll)]
		public static extern int waveOutWrite(IntPtr hWaveOut, ref WaveHdr lpWaveOutHdr, int uSize);
		[DllImport(mmdll)]
		public static extern int waveOutOpen(out IntPtr hWaveOut, int uDeviceID, WaveFormat lpFormat, WaveDelegate dwCallback, int dwInstance, int dwFlags);
		[DllImport(mmdll)]
		public static extern int waveOutReset(IntPtr hWaveOut);
		[DllImport(mmdll)]
		public static extern int waveOutClose(IntPtr hWaveOut);
		[DllImport(mmdll)]
		public static extern int waveOutPause(IntPtr hWaveOut);
		[DllImport(mmdll)]
		public static extern int waveOutRestart(IntPtr hWaveOut);
		[DllImport(mmdll)]
		public static extern int waveOutGetPosition(IntPtr hWaveOut, out int lpInfo, int uSize);
		[DllImport(mmdll)]
		public static extern int waveOutSetVolume(IntPtr hWaveOut, int dwVolume);
		[DllImport(mmdll)]
		public static extern int waveOutGetVolume(IntPtr hWaveOut, out int dwVolume);

		// WaveIn calls
		[DllImport(mmdll)]
		public static extern int waveInGetNumDevs();
		[DllImport(mmdll)]
		public static extern int waveInAddBuffer(IntPtr hwi, ref WaveHdr pwh, int cbwh);
		[DllImport(mmdll)]
		public static extern int waveInClose(IntPtr hwi);
		[DllImport(mmdll)]
		public static extern int waveInOpen(out IntPtr phwi, int uDeviceID, WaveFormat lpFormat, WaveDelegate dwCallback, int dwInstance, int dwFlags);
		[DllImport(mmdll)]
		public static extern int waveInPrepareHeader(IntPtr hWaveIn, ref WaveHdr lpWaveInHdr, int uSize);
		[DllImport(mmdll)]
		public static extern int waveInUnprepareHeader(IntPtr hWaveIn, ref WaveHdr lpWaveInHdr, int uSize);
		[DllImport(mmdll)]
		public static extern int waveInReset(IntPtr hwi);
		[DllImport(mmdll)]
		public static extern int waveInStart(IntPtr hwi);
		[DllImport(mmdll)]
		public static extern int waveInStop(IntPtr hwi);
	}
}
