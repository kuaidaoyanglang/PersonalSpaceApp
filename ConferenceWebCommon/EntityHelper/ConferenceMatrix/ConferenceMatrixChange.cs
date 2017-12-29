using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConferenceWebCommon.EntityCommon;

namespace ConferenceWebCommon.EntityHelper.ConferenceMatrix
{
    [Serializable]
    public class ConferenceMatrixChange : ConferenceMatrixBase
    {
        /// <summary>
        /// 矩阵投影模式
        /// </summary>
        public MaxtrixModeType MaxtrixModeType { get; set; }
    }
}
