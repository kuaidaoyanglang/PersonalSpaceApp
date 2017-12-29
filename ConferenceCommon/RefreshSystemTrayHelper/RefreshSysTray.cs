using ConferenceCommon.LogHelper;
using System;
namespace ConferenceCommon.RefreshSystemTrayHelper
{

    public static class SysTray
    {
        private static int GetSysTrayWnd()
        {
            int rtInit = 0;
            try
            {
                OSName version = OS.GetVersion();
                int num = Windows.FindWindow("Shell_TrayWnd", null);
                num = Windows.FindWindowEx(num, 0, "TrayNotifyWnd", null);
                if (version != OSName.Win2000 && version != OSName.WinXP && version != OSName.Win2003 && version != OSName.Win7 )
                {
                    return num;
                }
                if (version == OSName.Win2000)
                {
                    return Windows.FindWindowEx(num, 0, "ToolbarWindow32", null);
                }
                num = Windows.FindWindowEx(num, 0, "SysPager", null);
                rtInit = Windows.FindWindowEx(num, 0, "ToolbarWindow32", null);               
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(SysTray), ex);
            }
            return rtInit;
        }
        public static void Refresh()
        {
            try
            {
                int sysTrayWnd = SysTray.GetSysTrayWnd();
                HRect hRect = default(HRect);
                Windows.GetClientRect((IntPtr)sysTrayWnd, out hRect);
                for (int i = 0; i < hRect.right; i += 2)
                {
                    for (int j = 0; j < hRect.bottom; j += 2)
                    {
                        Message.SendMessage(sysTrayWnd, 512, 0, Message.MakeLParam(i, j));
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(SysTray), ex);
            }
        }

    }
}
