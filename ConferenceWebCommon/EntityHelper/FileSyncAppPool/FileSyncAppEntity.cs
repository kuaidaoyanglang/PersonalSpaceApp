using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.FileSyncAppPool
{
    [Serializable]
    public class FileSyncAppEntity
    {
        byte[] imgBytes;
        /// <summary>
        /// 图片转成的字节数组
        /// </summary>
        public byte[] ImgBytes
        {
            get { return imgBytes; }
            set { imgBytes = value; }
        }       
    }
}
