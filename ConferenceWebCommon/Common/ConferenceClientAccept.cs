using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityCommon
{
    [Serializable]
    public class ConferenceClientAccept
    {

        string conferenceName = null;
        /// <summary>
        /// 会议名称
        /// </summary>
        public string ConferenceName
        {
            get { return conferenceName; }
            set { conferenceName = value; }
        }

        int conferenceID;
        /// <summary>
        /// 会议ID
        /// </summary>
        public int ConferenceID
        {
            get { return conferenceID; }
            set { conferenceID = value; }
        }


        string selfUri = null;
        /// <summary>
        /// 当前用户
        /// </summary>
        public string SelfUri
        {
            get { return selfUri; }
            set { selfUri = value; }
        }

        ConferenceClientAcceptType conferenceClientAcceptType;
        /// <summary>
        /// 类型（研讨树，研讨语音，甩屏）
        /// </summary>
        public ConferenceClientAcceptType ConferenceClientAcceptType
        {
            get { return conferenceClientAcceptType; }
            set { conferenceClientAcceptType = value; }
        }
    }  
}
