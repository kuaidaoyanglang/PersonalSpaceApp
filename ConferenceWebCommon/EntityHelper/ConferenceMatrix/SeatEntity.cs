using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.ConferenceMatrix
{
    [Serializable]
    public class SeatEntity : ConferenceMatrixBase
    {
        /// <summary>
        /// 座位ip号
        /// </summary>
        public string SettingIP { get; set; }

        /// <summary>
        /// 座位序列号
        /// </summary>
        public int SettingNummber { get; set; }

        /// <summary>
        /// 当前占用该位置的参会人名称
        /// </summary>
        public string  UserName { get; set; }
    }
}
