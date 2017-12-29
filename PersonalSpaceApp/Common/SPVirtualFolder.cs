using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersonalSpaceApp.Common
{
    public class SPVirtualFolder
    {
        /// <summary>
        /// id号
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 文件夹名称
        /// </summary>
        public string Name { get; set; }

        private List<SPVirtualFolder> folders = new List<SPVirtualFolder>();
        /// <summary>
        /// 子目录所有文件夹集合
        /// </summary>
        public List<SPVirtualFolder> Folders
        {
            get { return folders; }
            set { folders = value; }
        }


        private List<SPVirtualFile> files = new List<SPVirtualFile>();
        /// <summary>
        /// 子目录所有文件集合
        /// </summary>            
        public List<SPVirtualFile> Files
        {
            get { return files; }
            set { files = value; }
        }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}
