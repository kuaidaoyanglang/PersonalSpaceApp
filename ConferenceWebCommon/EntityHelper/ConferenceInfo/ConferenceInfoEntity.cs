using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.ConferenceInfo
{
    [Serializable]
    public class ConferenceInfoEntity
    {
        /// <summary>
        /// 分享者
        /// </summary>
        public string Sharer { get; set; }

        /// <summary>
        /// 导航页面类型
        /// </summary>
        public ConferencePageType ConferencePageType { get; set; }       
    }
}
