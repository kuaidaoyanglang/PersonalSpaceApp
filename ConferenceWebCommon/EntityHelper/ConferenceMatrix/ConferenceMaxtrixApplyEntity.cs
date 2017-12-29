using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.ConferenceMatrix
{
    [Serializable]
    public class ConferenceMaxtrixApplyEntity : ConferenceMatrixBase
    {      
        /// <summary>
        /// 申请人
        /// </summary>
        public string ApplyPeople { get; set; }
       
        /// <summary>
        /// 输出口
        /// </summary>
        public ConferenceMatrixOutPut ConferenceMatrixOutPut { get; set; }

        /// <summary>
        /// 实际投影人
        /// </summary>
        public SeatEntity BeforeSeatEntity { get; set; }

        /// <summary>
        /// 实际投影人
        /// </summary>
        public SeatEntity SeatEntity { get; set; }
    }
}
