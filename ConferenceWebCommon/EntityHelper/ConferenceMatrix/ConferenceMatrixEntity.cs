using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConferenceWebCommon.EntityHelper.ConferenceMatrix;

namespace ConferenceWebCommon.EntityHelper.ConferenceMatrix
{
    [Serializable]
    public class ConferenceMatrixEntity : ConferenceMatrixBase
    {       
        /// <summary>
        /// 分享者
        /// </summary>
        public string Sharer { get; set; }

        /// <summary>
        /// 分享者地址
        /// </summary>
        public string SharerUri { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        /// 输出口
        /// </summary>
        public ConferenceMatrixOutPut ConferenceMatrixOutPut { get; set; }

        /// <summary>
        /// 实际投影人
        /// </summary>
        public SeatEntity SeatEntity { get; set; }
    }
}
