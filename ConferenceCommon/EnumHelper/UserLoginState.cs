using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceCommon.EnumHelper
{
     [Serializable]
    public enum UserLoginState
    {
        Available = 0,
        Busy = 1,
        DoNotDisturb = 2,
        Away = 3,
        OffWork = 4,
        Leave =5,
    }
}
