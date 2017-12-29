using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityCommon
{
    [Serializable]
    public enum SceneModeType
    {
        /// <summary>
        /// 标准模式
        /// </summary>
        StantardMode,
        /// <summary>
        /// 精简模式
        /// </summary>
        SimpleMode,
        /// <summary>
        /// 教育模式
        /// </summary>
        EducationMode,
    }
}
