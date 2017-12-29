using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PersonalSpaceApp.Common
{
    [Serializable]
    public class SPItemEntity
    {
        public string FileLeafRef { get; set; }

        public string Title { get; set; }

        public string _dlc_DocId { get; set; }

        public string  _dlc_DocIdUrl { get; set; }

        public string  ID { get; set; }

        public string  ContentType { get; set; }

        public string Created { get; set; }

        public string  Author { get; set; }

        public string  Modified { get; set; }

        public string Editor { get; set; }

        public string _CopySource { get; set; }

        public string CheckoutUser { get; set; }

        public string _CheckinComment { get; set; }

        public string LinkFilenameNoMenu { get; set; }

        public string LinkFilename { get; set; }

        public string DocIcon { get; set; }

        public string FileSizeDisplay { get; set; }

        public string ItemChildCount { get; set; }

        public string FolderChildCount { get; set; }

        public string AppAuthor { get; set; }

        public string AppEditor { get; set; }

        public string Edit { get; set; }

        public string _UIVersionString { get; set; }

        public string ParentVersionString { get; set; }

        public string ParentLeafName { get; set; }

        /// <summary>
        /// 相对路径
        /// </summary>
        public string FileRef { get; set; }
    }
}
