using ConferenceCommon.EnumHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceCommon.EntityHelper
{
    [Serializable]
    /// <summary>
    /// 适用于用户本地适用（惯性）
    /// </summary>
    public class SettingEntity
    {

        #region 设备参数
        
        int videoDevice_Index;
        /// <summary>
        /// 摄像头（视频设备,索引）
        /// </summary>
        public int VideoDevice_Index
        {
            get { return videoDevice_Index; }
            set { videoDevice_Index = value; }
        }

        string videoDevice_Name =string.Empty;
        /// <summary>
        /// 摄像头名称
        /// </summary>
        public string VideoDevice_Name
        {
            get { return videoDevice_Name; }
            set { videoDevice_Name = value; }
        }

        int audioDevice_Index;
        /// <summary>
        /// 扬声器(音频设备,索引)
        /// </summary>
        public int AudioDevice_Index
        {
            get { return audioDevice_Index; }
            set { audioDevice_Index = value; }
        }

        string audioDevice_Name = string.Empty;
        /// <summary>
        /// 扬声器名称
        /// </summary>
        public string AudioDevice_Name
        {
            get { return audioDevice_Name; }
            set { audioDevice_Name = value; }
        }

        #endregion

        #region 普通参数

     
        ClientMode clientMode = ClientMode.PC;
        /// <summary>
        /// 客户端交互模式
        /// </summary>
        public ClientMode ClientMode
        {
            get { return clientMode; }
            set { clientMode = value; }
        }

        #endregion

    }
}
