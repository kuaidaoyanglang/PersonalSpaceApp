using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.ConferenceMatrix
{
    [Serializable]
    public class ConferenceMatrixThrowCode : ConferenceMatrixBase
    {
        public MatrixThrowCodeType matrixThrowCodeType { get; set; }
    }

    [Serializable]
    public enum MatrixThrowCodeType
    {
        /// <summary>
        /// 清楚投影状态
        /// </summary>
        ClearProjectionState,
        /// <summary>
        /// 清楚投影提示
        /// </summary>
        ClearProjectionTip,
    }
}
