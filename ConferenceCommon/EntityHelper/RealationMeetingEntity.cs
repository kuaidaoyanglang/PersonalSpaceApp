using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceCommon.EntityHelper
{
    /// <summary>
    /// 关联的会议
    /// </summary>
   public class RealationMeetingEntity
    {
       int meetid;
        /// <summary>
        /// 会议ID
        /// </summary>
        public int Meetid
        {
            get { return meetid; }
            set { meetid = value; }
        }

        string meetname;
        /// <summary>
        /// 会议名称
        /// </summary>
        public string Meetname
        {
            get { return meetname; }
            set { meetname = value; }
        }
    }
}
