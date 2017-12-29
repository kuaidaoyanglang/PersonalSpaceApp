using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersonalSpaceApp.Common
{
    public class AdEntity
    {
        /// <summary>
        /// 组织、人员名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 登录名称
        /// </summary>
        public string loginname { get; set; }

        /// <summary>
        /// ad
        /// </summary>
        public string ad { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }
    }
}
