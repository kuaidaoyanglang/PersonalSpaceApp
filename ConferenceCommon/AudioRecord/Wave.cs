using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using ConferenceCommon.LogHelper;

namespace ConferenceCommon.AudioRecord
{
    /// <summary>
    /// Wave 的摘要说明。
    /// </summary>
    public class Wave : IWaveControl
    {
        #region 字段

        private WaveInRecorder m_Recorder;
        private WaveFormat m_Format;

        System.IO.BinaryWriter bw_tmp;
        private string tmpName = "tmp.wav";
        /// <summary>
        /// 操作录音文件流
        /// </summary>
        private FileStream fs_tmp;
        private byte[] m_RecBuffer;

        FileStream fs = null;
        FileStream fs_2 = null;

        #endregion

        #region 构造函数

        public Wave()
        {

        }

        #endregion

        #region 数据接收

        /// <summary>
        /// 数据接收
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        private void DataArrived(IntPtr data, int size)
        {
            try
            {
                if (m_RecBuffer == null || m_RecBuffer.Length < size)
                {
                    m_RecBuffer = new byte[size];
                }
                System.Runtime.InteropServices.Marshal.Copy(data, m_RecBuffer, 0, size);

                bw_tmp.Write(m_RecBuffer);
                _recordSize += m_RecBuffer.Length;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
                return;
            }
        }

        #endregion

        #region 写入文件

        /// <summary>
        /// 写入文件
        /// </summary>
        private void WriteToFile()
        {
         
            BinaryReader br = null;
           
            BinaryWriter bw = null;
            bool _isFinish = false;

            try
            {
                fs = new FileStream(tmpName, FileMode.Open);
                fs.Position = 0;
                br = new BinaryReader(fs);

                fs_2 = new FileStream(SavedFile, FileMode.Create);
                //fs_2.Position = 0;
                bw = new BinaryWriter(fs_2);
                //bw.Seek(0, SeekOrigin.Begin);
                //wav头
                long chunksize = fs.Length + 36;
                WriteChars(bw, "RIFF");//格式
                bw.Write((int)chunksize);//文件长度（要加上头的36字节）
                WriteChars(bw, "WAVE");//标示
                WriteChars(bw, "fmt ");//fmt
                bw.Write((int)16);//fmt长度
                bw.Write(m_Format.wFormatTag);//压缩模式
                bw.Write(m_Format.nChannels);//声道
                bw.Write(m_Format.nSamplesPerSec);//采样率包含：32000Hz,44100Hz,48000Hz.
                bw.Write(m_Format.nAvgBytesPerSec);//每秒播放字节
                bw.Write(m_Format.nBlockAlign);//位速
                bw.Write(m_Format.wBitsPerSample);//采样大小
                WriteChars(bw, "data");//data标志
                bw.Write(fs.Length);//音频长度
                bw.Flush();

                byte[] bytes = new byte[512];
                while (true)
                {
                    if (br.Read(bytes, 0, bytes.Length) > 0)
                    {
                        bw.Write(bytes);
                    }
                    else
                    {
                        break;
                    }
                }

                _isFinish = true;

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
                OnError(ex, "文件流写入异常!");
            }
            finally
            {
                if (br != null) br.Close();
                try
                {
                    if (bw != null) bw.Close();
                }
                catch (Exception ex)
                {
                    System.Threading.Thread.Sleep(500);
                    if (File.Exists(this.SavedFile))
                    {
                        FileInfo fileInfo = new FileInfo(this.SavedFile);
                        fileInfo.Delete();
                    }

                    if (File.Exists(this.RecordTmpFile))
                    {
                        FileInfo fileInfo = new FileInfo(this.RecordTmpFile);
                        fileInfo.Delete();
                    }
                    LogManage.WriteLog(this.GetType(), ex);
                }

                if (_isFinish)
                {
                    System.Threading.Thread.Sleep(500);
                    FileInfo fi = new FileInfo(tmpName);
                    fi.Delete();
                }
            }
        }

        #endregion

        #region 流释放

        /// <summary>
        /// 流释放
        /// </summary>
        public void StreamDispose()
        {
            try
            {
                //if (fs_tmp!= null)
                //{
                //    fs_tmp.Dispose();
                //}
                if (fs != null)
                {
                    fs.Dispose();
                }
                if (fs_2 != null)
                {
                    fs_2.Dispose();
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        #endregion

        #region 字符收集

        /// <summary>
        /// 字符收集
        /// </summary>
        /// <param name="wrtr"></param>
        /// <param name="text"></param>
        private void WriteChars(BinaryWriter wrtr, string text)
        {
            try
            {
                for (int i = 0; i < text.Length; i++)
                {
                    char c = (char)text[i];
                    wrtr.Write(c);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 停止录音

        /// <summary>
        /// 停止录音
        /// </summary>
        public void Stop()
        {

            if (this.m_Recorder != null)
            {
                try
                {
                    this.bw_tmp.Close();
                    this.m_Recorder.Dispose();

                    this.WriteToFile();
                    this._recordSize = 0;
                    this._isRecord = false;
                }
                catch (Exception ex)
                {
                    LogManage.WriteLog(this.GetType(), ex);
                }
                finally
                {
                    m_Recorder = null;
                }
            }
        }

        #endregion

        #region 开始录音

        /// <summary>
        /// 开始录音
        /// </summary>
        /// <param name="callBack"></param>
        public void Start(Action<bool> callBack)
        {
            if (this.IsRecord) return;
            this.Stop();
            try
            {
                FileInfo file = new FileInfo(this.tmpName);
                if (file.Exists)
                {
                    file.Delete();
                }
                this.fs_tmp = new FileStream(this.tmpName, System.IO.FileMode.Create);
                this.bw_tmp = new BinaryWriter(this.fs_tmp);

                this.dt_Start = DateTime.Now;
                this._isRecord = true;
                //m_Format = new WaveFormat(6400, 8, 1);
                this.m_Format = new WaveFormat(16000, 16, 1);
                //if (RecordQuality == Quality.low) m_Format = new WaveFormat(32000, 8, 2);
                //else if (RecordQuality == Quality.Normal) m_Format = new WaveFormat(44100, 8, 2);
                //else m_Format = new WaveFormat(44100, 16, 2);

                m_Recorder = new WaveInRecorder(-1, m_Format, 16384, 3, new BufferDoneEventHandler(DataArrived), new Action<bool>((isSuccessed) =>
                    {
                        if (callBack != null)
                        {
                            callBack(isSuccessed);
                        }
                    }));

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
                OnError(ex, "启动录音失败!");
                Stop();
            }
        }

        #endregion

        #region IWaveRecordInfo 成员

        private string _saveFile = null;
        public string SavedFile
        {
            get
            {
                return _saveFile;
            }
            set
            {
                _saveFile = value;
            }
        }

        private Quality _RecordQuality;
        public Quality RecordQuality
        {
            get
            {
                return _RecordQuality;
            }
            set
            {
                _RecordQuality = value;
            }
        }

        public string RecordTmpFile
        {
            get
            {
                return tmpName;
            }
        }

        private long _recordSize = 36;
        public long RecordSize
        {
            get
            {
                // TODO:  添加 Wave.RecordSize getter 实现
                return _recordSize;
            }
        }

        DateTime dt_Start;
        TimeSpan ts_now;
        public TimeSpan RecordTimeSpan
        {
            get
            {
                // TODO:  添加 Wave.RecordTimeSpan getter 实现
                if (IsRecord)
                {
                    ts_now = DateTime.Now.Subtract(dt_Start);
                }

                return ts_now;
            }
        }

        private bool _isRecord = false;
        /// <summary>
        /// 是否正在录音
        /// </summary>
        public bool IsRecord
        {
            get
            {
                // TODO:  添加 Wave.IsRecord getter 实现
                return _isRecord;
            }
        }



        #endregion

        #region IWaveControl 成员

        public event ErrorEventHandle ErrorEvent;

        private void OnError(Exception e, string error)
        {
            if (ErrorEvent != null)
                ErrorEvent(e, error);
        }
        #endregion
    }
}
