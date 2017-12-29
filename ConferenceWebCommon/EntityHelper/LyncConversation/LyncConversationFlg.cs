using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.LyncConversation
{
    [Serializable]
    public class LyncConversationFlg
    {

        /// <summary>
        /// 邀请参会
        /// </summary>
        public LyncConversationEntity LyncConversationEntity { get; set; }

        /// <summary>
        /// 大屏投影
        /// </summary>
        public BigScreenEnterEntity BigScreenEnterEntity { get; set; }

        /// <summary>
        /// 离开会议实体
        /// </summary>
        public LeaveConversationEntity LeaveConversationEntity { get; set; }

        /// <summary>
        /// 离开会议实体
        /// </summary>
        public PPTControlEntity PPTControlEntity { get; set; }

        /// <summary>
        /// 共享通知
        /// </summary>
        public LyncResourceNotify LyncResourceNotify { get; set; }

        /// <summary>
        /// 消息模式
        /// </summary>
        public LyncConversationFlgType LyncConversationFlgType { get; set; }


    }
    [Serializable]
    public enum LyncConversationFlgType
    {
        InviteContact = 0,
        EnterBigScreen = 1,
        LeaveConversation = 2,
        PPTControl = 3,
        LyncResourceNotify = 4
    }
}
