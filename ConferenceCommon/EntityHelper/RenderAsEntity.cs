using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Visifire.Charts;

namespace ConferenceCommon.EntityHelper
{
    public class RenderAsEntity
    {
        string chartTypeName;
        /// <summary>
        /// 图表类型名称
        /// </summary>
        public string ChartTypeName
        {
            get { return chartTypeName; }
            set { chartTypeName = value; }
        }

        RenderAs renderType = RenderAs.Column;
        /// <summary>
        ///  图表类型
        /// </summary>
        public RenderAs RenderType
        {
            get { return renderType; }
            set { renderType = value; }
        } 
    }
}
