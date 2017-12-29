using ConferenceCommon.EnumHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceCommon.WPFHelper
{
    public class NavicateButton:System.Windows.Controls.Button
    {
        private ViewSelectedItemEnum viewSelectedItemEnum;
        /// <summary>
        /// 导航选项按钮
        /// </summary>
        public ViewSelectedItemEnum ViewSelectedItemEnum
        {
            get { return viewSelectedItemEnum; }
            set { viewSelectedItemEnum = value; }
        }

        int intType;
        /// <summary>
        /// 
        /// </summary>
        public int IntType
        {
            get { return intType; }
            set { intType = value; }
        }

    }
}
