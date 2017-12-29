using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityCommon
{
    [Serializable]
    public enum ConferenceClientAcceptType
    {
        /// <summary>
        /// 研讨树
        /// </summary>
        ConferenceTree,
        /// <summary>
        /// 语音树
        /// </summary>
        ConferenceAudio,
        /// <summary>
        /// 文件同步
        /// </summary>
        ConferenceFileSync,
       
        /// <summary>
        /// 智存空间
        /// </summary>
        ConferenceSpaceSync,

        /// <summary>
        /// 信息同步
        /// </summary>
        ConferenceInfoSync,

        /// <summary>
        /// lync会话同步
        /// </summary>
        LyncConversationSync,

        /// <summary>
        /// 矩阵应用
        /// </summary>
        ConferenceMatrixSync,
    }
}
