using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.LyncConversation
{
    [Serializable]
    public class BigScreenEnterEntity : LyncEntityBase
    {
        /// <summary>
        /// 屏幕分享人
        /// </summary>
        public string Sharer { get; set; }

    }
}
