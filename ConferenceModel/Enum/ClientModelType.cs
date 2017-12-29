using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceModel.Enum
{
    public enum ClientModelType
    {
        /// <summary>
        /// 版本服务初始化
        /// </summary>
        ConferenceVersion,
        /// <summary>
        /// 会议讨论初始化(树)
        /// </summary>
        ConferenceTree,
        /// <summary>
        /// 会议讨论初始化（语音）
        /// </summary>
        ConferenceAudio,
        /// <summary>
        /// 文件同步（甩屏）
        /// </summary>
        FileSync,
        /// <summary>
        /// word同步
        /// </summary>
        Spacesync,
        /// <summary>
        /// 信息同步
        /// </summary>
        ConferenceInfo,
        /// <summary>
        /// lync会话同步
        /// </summary>
        LyncConversationSync,
        /// <summary>
        /// 矩阵切换
        /// </summary>
        MaxtriSync,
        /// <summary>
        /// sp搜索（资料搜索）
        /// </summary>
        ConferenceSpSearch,
        /// <summary>
        /// 物联网服务
        /// </summary>
        StuiomService,

        /// <summary>
        /// 智存空间独立服务
        /// </summary>
        Space_Service,
    }
}
