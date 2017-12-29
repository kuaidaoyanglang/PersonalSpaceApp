using ConferenceCommon.IconHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace ConferenceCommon.WindowHelper
{
   public static class Win32_WPF
    {
       [DllImport("user32.dll")]
       public static extern int SetParent(IntPtr hWndChild, IntPtr hWndParent);
    }      
}
