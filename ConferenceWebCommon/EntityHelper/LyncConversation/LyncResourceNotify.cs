using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.LyncConversation
{
    [Serializable]
    public class LyncResourceNotify : LyncEntityBase
    {
        /// <summary>
        /// 共享资源文件名称
        /// </summary>
        public string ResourceShareName { get; set; }

        /// <summary>
        /// 共享人名称
        /// </summary>
        public string ResourceSharePersonName { get; set; }
       
        /// <summary>
        /// 共享人邮箱地址
        /// </summary>
        public string ResourceSharePersonUri { get; set; }

        /// <summary>
        /// lync共享操作类型
        /// </summary>
        public LyncResourceOperationCodeType LyncResourceOperationCodeType { get; set; }

        /// <summary>
        /// 共享类型
        /// </summary>
        public ResourceType ResourceType { get; set; }
    }
}
