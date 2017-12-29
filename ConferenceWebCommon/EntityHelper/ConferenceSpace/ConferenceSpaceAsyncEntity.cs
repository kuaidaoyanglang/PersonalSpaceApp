using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.ConferenceOffice
{
    [Serializable]
    public class ConferenceSpaceAsyncEntity
    {
        /// <summary>
        /// 分享者
        /// </summary>
        public string Sharer { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public FileType FileType { get; set; }

        /// <summary>
        /// 路径（word）
        /// </summary>
        public string Uri { get; set; }
    }
}
