using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersonalSpaceApp.Common
{
    public class SPVirtualFile
    {
        /// <summary>
        /// id号
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 文件长度
        /// </summary>
        public long Length { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        public string ServerRelativeUrl { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
