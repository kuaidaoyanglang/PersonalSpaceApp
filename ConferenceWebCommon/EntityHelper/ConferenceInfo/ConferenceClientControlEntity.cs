using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.ConferenceInfo
{

    [Serializable]
    public class ConferenceClientControlEntity
    {
        /// <summary>
        /// 分享者
        /// </summary>
        public string Sharer { get; set; }

        /// <summary>
        /// 控制命令
        /// </summary>
        public ClientControlType ClientControlType { get; set; }
    }

     [Serializable]
    public enum ClientControlType
    {
        Close,
        VersionUpdate
    }
}
