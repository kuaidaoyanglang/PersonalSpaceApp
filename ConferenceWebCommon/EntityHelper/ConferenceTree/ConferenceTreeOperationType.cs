using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.ConferenceTree
{
    [Serializable]
    public enum ConferenceTreeOperationType
    {
        AddType = 0,
        DeleteType = 1,
        UpdateType = 2,
        RefleshAllType = 3,

        FocusType1 = 4,
        FocusType2 = 5,

        UpdateTittle = 6,
        UpdateComment = 7,
        LinkAdd = 8,
    }
}
