using ConferenceCommon.LogHelper;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace ConferenceCommon.RegeditHelper
{
    public static class RegeditManage
    {
        #region 根据注册表打开某个程序

        /// <summary>
        /// 一般应用程序的注册表路径都在这里（HKEY_LOCAL_MACHINE）
        /// </summary>
        /// <param name="ProgressName"></param>
        public static void OpenAplicationByRegedit(string ProgressName)
        {
            RegistryKey appPath = null;
            try
            {
                appPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + ProgressName);
                string fullName = null;
                if (appPath != null)
                {
                    fullName = Convert.ToString(appPath.GetValue(string.Empty));
                }
                if (!string.IsNullOrEmpty(fullName))
                {
                    Process process = new Process();
                    process.StartInfo.FileName = fullName;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(RegeditManage), ex);
            }
            finally
            {
                if (appPath != null)
                    appPath.Close();
            }
        }

        /// <summary>
        /// 一般应用程序的注册表路径都在这里（HKEY_LOCAL_MACHINE）
        /// </summary>
        /// <param name="ProgressName"></param>
        public static Process OpenAplicationByRegedit2(string ProgressName)
        {
            Process process = null;
            try
            {
                RegistryKey appPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + ProgressName);
                string fullName = null;
                if (appPath != null)
                {
                    fullName = Convert.ToString(appPath.GetValue(string.Empty));
                }
                if (!string.IsNullOrEmpty(fullName))
                {
                    process = new Process();
                    process.StartInfo.FileName = fullName;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(RegeditManage), ex);
            }

            return process;
        }

        /// <summary>
        /// 一般应用程序的注册表路径都在这里（HKEY_LOCAL_MACHINE）,并带参数
        /// </summary>
        /// <param name="ProgressName"></param>
        public static void OpenAplicationByRegedit2(string ProgressName, string parameter, Action<int, IntPtr> callback)
        {
            Process Proc = null;
            try
            {
                RegistryKey appPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + ProgressName);
                string fullName = null;
                if (appPath != null)
                {
                    fullName = Convert.ToString(appPath.GetValue(string.Empty));
                }
                Proc = System.Diagnostics.Process.Start(fullName, parameter);
                IntPtr mainWindowHandle = Proc.MainWindowHandle;
                Thread.Sleep(2000);

                IntPtr handle = Proc.MainWindowHandle;

                int ProcessID = Proc.Id;
                callback(ProcessID, handle);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(RegeditManage), ex);
            }
            finally
            {
                Proc.Dispose();
            }
        }

        #endregion

        #region 修改Lync注册表（整体）


        /// <summary>
        ///修改Lync注册表  HKEY_CURRENT_USER() ===开启lync的时候不启动初始化界面和最小化主窗体
        /// </summary>
        public static void UpdateLyncRegedit()
        {
            RegistryKey appPath = null;
            try
            {
                string root = @"Software\Microsoft\Office\15.0\Lync\";
                appPath = Registry.CurrentUser.CreateSubKey(root);
                if (appPath != null)
                {
                    appPath.SetValue("AutoOpenMainWindowWhenStartup", 1);
                    appPath.SetValue("AutoSignInWhenUserSessionStarts", 0);
                    appPath.SetValue("AlwaysShowMenu", 0);
                    appPath.SetValue("AlwaysOnTop", 0);
                    appPath.SetValue("MinimizeWindowToNotificationArea", 1);
                    //单选项卡
                    appPath.SetValue("IsTabbedConversationWindowEnabled_1", 0);
                    appPath.SetValue("JoinAudioConferenceFrom", 0);
                    appPath.SetValue("ShowIMForLyncMeetings", 0);
                    appPath.SetValue("ShowRosterForLyncMeetings", 0);


                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(RegeditManage), ex);
            }
            finally
            {
                if (appPath != null)
                    appPath.Close();
            }
        }

        #endregion

        #region 修改注册表（修改单个）

        /// <summary>
        /// 修改注册表
        /// </summary>
        public static void UpdateLyncRegeditSingle()
        {
            RegistryKey appPath = null;
            try
            {
                appPath = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Office\15.0\Lync\");
                if (appPath != null)
                {
                    appPath.SetValue("AlwaysOnTop", 1);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(RegeditManage), ex);
            }
            finally
            {
                if (appPath != null)
                    appPath.Close();
            }
        }

        #endregion

        #region 删除lync注册表

        /// <summary>
        ///删除Lync注册表（）  
        /// </summary>
        public static void DeleteLyncRegedit()
        {
            RegistryKey appPath = null;
            try
            {
                string subkey = @"Software\Microsoft\Office\15.0\Lync\";
                appPath = Registry.CurrentUser.OpenSubKey(subkey, true);
                if (appPath != null)
                {
                    appPath.DeleteValue("SavePassword");
                    appPath.DeleteValue("ServerUsername");

                    //ServerSipUri
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(RegeditManage), ex);
            }
            finally
            {
                if (appPath != null)
                    appPath.Close();
            }
        }

        #endregion

        #region 开机自动启动设置

        public static void SetAutoRun(string fileName, bool isAutoRun)
        {
            RegistryKey reg = null;
            try
            {
                if (!System.IO.File.Exists(fileName))
                {
                    LogManage.WriteLog(typeof(RegeditManage), "文件不存在");
                }
                String name = fileName.Substring(fileName.LastIndexOf(@"\") + 1).Replace(".exe", string.Empty);
                reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", RegistryKeyPermissionCheck.ReadWriteSubTree);
                if (reg == null)
                {
                    reg = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", RegistryKeyPermissionCheck.ReadWriteSubTree);
                }
                if (isAutoRun)
                {
                    reg.SetValue(name, fileName);
                }
                else
                {
                    reg.SetValue(name, false);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(RegeditManage), ex);
            }
            finally
            {
                if (reg != null)
                    reg.Close();
            }
        }

        #endregion

        #region 指定webbrowser版本(9.0)

        /// <summary>
        /// 指定webbrowser版本(9.0)
        /// </summary>
        /// <param name="currentApplicationExeName"></param>
        public static void SetWebBrowserVersion(string currentApplicationExeName)
        {
            RegistryKey appPath = null;
            try
            {
                int systemType = GetSystemType();
                if (systemType == 64)
                {
                    appPath = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION\");
                }
                else if (systemType == 32)
                {
                    appPath = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_BROWSER_EMULATION\");
                }
                if (appPath != null)
                {
                    var subKey = appPath.OpenSubKey(currentApplicationExeName);
                    if (subKey == null)
                    {
                        appPath.CreateSubKey(currentApplicationExeName);
                    }
                    appPath.SetValue(currentApplicationExeName, 9000);
                }

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(RegeditManage), ex);
            }
            finally
            {
                if (appPath != null)
                    appPath.Close();
            }
        }

        #endregion

        #region 获取系统类型 32位或者64位

        /// <summary>
        /// 获取系统类型 32位或者64位
        /// </summary>
        /// <returns></returns>
        public static int GetSystemType()
        {
            int type = 0;
            if (IntPtr.Size == 8)
            {
                type = 64;
            }
            if (IntPtr.Size == 4)
            {
                type = 32;
            }
            return type;
        }

        #endregion

        #region 设置证书（检查发行商的证书是否吊销，检查服务器证书吊销）

        /// <summary>
        /// 设置证书（不检查发行商的证书是否吊销，不检查服务器证书吊销）
        /// </summary>
        public static void SetNoCheckCertificationIsRevoke()
        {
            RegistryKey appPath = null;
            RegistryKey appPath2 = null;
            try
            {

                appPath = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings\");
                if (appPath != null)
                {
                    appPath.SetValue("CertificateRevocation", 0);
                }

                appPath2 = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Windows\CurrentVersion\WinTrust\Trust Providers\Software Publishing\");
                if (appPath2 != null)
                {
                    appPath2.SetValue("State", 00023e00);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(RegeditManage), ex);
            }
        }

        #endregion

    }
}
