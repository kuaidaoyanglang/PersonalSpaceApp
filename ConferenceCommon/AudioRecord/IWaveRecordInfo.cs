using System;

namespace ConferenceCommon.AudioRecord
{
	/// <summary>
	/// IWaveInfo 的摘要说明。
	/// </summary>
	public interface IWaveRecordInfo
	{
		string RecordTmpFile{get;}
		string SavedFile{get;set;}
		Quality RecordQuality{get;set;}
		long RecordSize{get;}
		TimeSpan RecordTimeSpan{get;}
		bool IsRecord{get;}
		
	}
}
