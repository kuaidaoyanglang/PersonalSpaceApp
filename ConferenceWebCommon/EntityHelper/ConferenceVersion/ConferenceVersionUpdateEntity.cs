using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.ConferenceVersion
{
    /// <summary>
    /// 更新文件实体
    /// </summary>
    public class ConferenceVersionUpdateEntity
    {

        string updateFile;
        /// <summary>
        /// 更新文件
        /// </summary>
        public string UpdateFile
        {
            get { return updateFile; }
            set { updateFile = value; }
        }

        string updateRootFile;
        /// <summary>
        /// 更新文件（带有文件目录）
        /// </summary>
        public string UpdateRootFile
        {
            get { return updateRootFile; }
            set { updateRootFile = value; }
        }
    }
}
