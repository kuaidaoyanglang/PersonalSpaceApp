using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityCommon
{
    [Serializable]
    public enum MaxtrixModeType
    {
        /// <summary>
        /// 自由投影模式
        /// </summary>
        freeMode,
        /// <summary>
        /// 传递模式
        /// </summary>
        SendMode,
        /// <summary>
        /// 主持人控制模式
        /// </summary>
        ControlMode
    }
}
