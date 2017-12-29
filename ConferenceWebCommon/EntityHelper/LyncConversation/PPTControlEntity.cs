using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.LyncConversation
{
     [Serializable]
    public class PPTControlEntity : LyncEntityBase
    {
        /// <summary>
        /// ppt控制者
        /// </summary>
        public string Controler { get; set; }
    }
}
