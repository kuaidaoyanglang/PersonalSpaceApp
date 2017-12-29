using ConferenceCommon.LogHelper;
using ConferenceCommon.ProcessHelper;
using ConferenceCommon.RegeditHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ConferenceCommon.ApplicationHelper
{
    public class ApplicationManage
    {
        #region 打开本地应用程序

        public static void OpenApp(AppType appType)
        {
            try
            {
                switch (appType)
                {
                    case AppType.IE:
                        RegeditManage.OpenAplicationByRegedit2("IEXPLORE.EXE");
                        break;
                    case AppType.Notepad:
                        ProcessManage.OpenAppByAppName("notepad.exe");
                        break;
                    case AppType.calc:
                        ProcessManage.OpenAppByAppName("calc.exe");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ApplicationManage), ex);
            }
        }

        #endregion

        #region 使用本地ie浏览器打开一个网站

        /// <summary>
        /// 打开一个网站
        /// </summary>
        /// <param name="url">网址</param>
        public static void OpenIE(string url)
        {
            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo("IExplore.exe");
                startInfo.Arguments = url;
                Process process1 = new Process();
                process1.StartInfo = startInfo;
                process1.Start();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ApplicationManage), ex);
            }
        }

        #endregion
    }

    public enum AppType
    {
        IE,
        Notepad,
        calc
    }
}
