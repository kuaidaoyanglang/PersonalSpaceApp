using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SP = Microsoft.SharePoint.Client;

namespace ConferenceCommon.SharePointHelper
{
    public class SPEntity
    {
        List<SP.Folder> folderCollection = new List<SP.Folder>() ;
        /// <summary>
        /// sp文件夹集合
        /// </summary>
        public List<SP.Folder> FolderCollection
        {
            get { return folderCollection; }
            set { folderCollection = value; }
        }

        List<SP.File> fileCollection = new List<SP.File>();
        /// <summary>
        /// 文件集合
        /// </summary>
        public List<SP.File> FileCollection
        {
            get { return fileCollection; }
            set { fileCollection = value; }
        }
    }
}
