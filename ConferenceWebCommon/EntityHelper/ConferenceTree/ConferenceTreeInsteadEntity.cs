using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.ConferenceTree
{
    [Serializable]
    public class ConferenceTreeInsteadEntity
    {
        int beforeItemGuid;
        /// <summary>
        /// 目标树节点GUID
        /// </summary>
        public int BeforeItemGuid
        {
            get { return beforeItemGuid; }
            set { beforeItemGuid = value; }
        }

        int nowItemGuid;
        /// <summary>
        /// 拖拽到目标GUID
        /// </summary>
        public int NowItemGuid
        {
            get { return nowItemGuid; }
            set { nowItemGuid = value; }
        }
    }
}
