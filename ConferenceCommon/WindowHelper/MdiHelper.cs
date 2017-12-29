using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ConferenceCommon.WindowHelper
{
    public class MdiManage
    {
        [DllImport("user32.dll", EntryPoint = "SetParent")]
        public extern static IntPtr SetParent(IntPtr childPtr, IntPtr parentPtr);
      
    }
}
