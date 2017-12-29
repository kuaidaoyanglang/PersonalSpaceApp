using System;

namespace ConferenceCommon.AudioRecord
{
	/// <summary>
	/// IWaveInfo ��ժҪ˵����
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
