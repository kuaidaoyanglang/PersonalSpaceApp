using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.ConferenceInfo
{
    [Serializable]
    public class ConferenceInfoFlg
    {
        ConferenceInfoEntity conferenceInfoEntity;
        /// <summary>
        /// 页面同步
        /// </summary>
        public ConferenceInfoEntity ConferenceInfoEntity
        {
            get { return conferenceInfoEntity; }
            set { conferenceInfoEntity = value; }
        }

        ConferenceInfoTypeChangeEntity conferenceInfoTypeChangeEntity;
        /// <summary>
        /// 场景模式
        /// </summary>
        public ConferenceInfoTypeChangeEntity ConferenceInfoTypeChangeEntity
        {
            get { return conferenceInfoTypeChangeEntity; }
            set { conferenceInfoTypeChangeEntity = value; }
        }

        ConferenceInfoFlgType conferenceInfoFlgType;
        /// <summary>
        /// 消息模式
        /// </summary>
        public ConferenceInfoFlgType ConferenceInfoFlgType
        {
            get { return conferenceInfoFlgType; }
            set { conferenceInfoFlgType = value; }
        }

        ConferenceClientControlEntity conferenceClientControlEntity;
        /// <summary>
        /// 客户端控制
        /// </summary>
        public ConferenceClientControlEntity ConferenceClientControlEntity
        {
            get { return conferenceClientControlEntity; }
            set { conferenceClientControlEntity = value; }
        }

    }

    [Serializable]
    public enum ConferenceInfoFlgType
    {
        ConferenceInfoEntity,
        ConferenceInfoTypeChangeEntity,
        ConferenceClientControlEntity,
    }
}
