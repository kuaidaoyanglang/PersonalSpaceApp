using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceCommon.EntityHelper
{
    public class SpSearchEntity
    {
        /// <summary>
        /// 文档ID
        /// </summary>
        public int DocId { get; set; }

       /// <summary>
       /// 文档标题
       /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文档作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 文档路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 写入时间
        /// </summary>
        public string Write { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public string FileType { get; set; }    
    }
}
