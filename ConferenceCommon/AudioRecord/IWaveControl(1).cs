using System;

namespace ConferenceCommon.AudioRecord
{
	public delegate void ErrorEventHandle(Exception e,string error);
	/// <summary>
	/// IWaveControl 的摘要说明。
	/// </summary>
	public interface IWaveControl : IWaveRecordInfo
	{
		void Stop();
        void Start(Action<bool> callBack);

        void StreamDispose();

		event ErrorEventHandle ErrorEvent;
	}
}
