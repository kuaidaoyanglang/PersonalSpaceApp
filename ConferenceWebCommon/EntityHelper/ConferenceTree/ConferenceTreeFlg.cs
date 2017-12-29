using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.ConferenceTree
{
    [Serializable]
    public class ConferenceTreeFlg
    {
        ConferenceTreeItemTransferEntity conferenceTreeItemTransferEntity;
        //知识树映射实体
        public ConferenceTreeItemTransferEntity ConferenceTreeItemTransferEntity
        {
            get { return conferenceTreeItemTransferEntity; }
            set { conferenceTreeItemTransferEntity = value; }
        }

        ConferenceTreeInsteadEntity conferenceTreeInsteadEntity;
        /// <summary>
        /// 知识树拖拽实体
        /// </summary>
        public ConferenceTreeInsteadEntity ConferenceTreeInsteadEntity
        {
            get { return conferenceTreeInsteadEntity; }
            set { conferenceTreeInsteadEntity = value; }
        }

        ConferenceTreeFlgType conferenceTreeFlgType;
        /// <summary>
        /// 知识树操作类型
        /// </summary>
        public ConferenceTreeFlgType ConferenceTreeFlgType
        {
            get { return conferenceTreeFlgType; }
            set { conferenceTreeFlgType = value; }
        }
    }

    [Serializable]
    public enum ConferenceTreeFlgType
    {
        normal =0,
        instead =1,
    }
}
