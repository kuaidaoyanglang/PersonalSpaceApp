using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace ConferenceCommon.ProcessHelper
{
    public static class ProcessManage
    {
        #region 关闭后台进程

        /// <summary>
        /// 释放后台进程的资源
        /// </summary>
        public static void KillProcess(string processName)
        {
            try
            {
                //获取指定名称的后台进程
                var process = Process.GetProcessesByName(processName);
                //判断是否存在
                foreach (var item in process)
                {
                    item.Kill();
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ProcessManage), ex);
            }
        }

        #endregion

        #region 根据进程获取主窗体句柄

        public static IntPtr GetMainWindowHandle(Process process)
        {
            IntPtr MainWindowHandle = IntPtr.Zero;
            try
            {
                EnumWindows(new EnumWindowsProc((hWnd, lParam) =>
                {
                    IntPtr PID;
                    GetWindowThreadProcessId(hWnd, out PID);

                    if (PID == lParam &&
                        IsWindowVisible(hWnd) &&
                        GetWindow(hWnd, GW_OWNER) == IntPtr.Zero)
                    {
                        MainWindowHandle = hWnd;
                        return false;
                    }

                    return true;

                }), new IntPtr(process.Id));
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ProcessManage), ex);
            }
            return MainWindowHandle;
        }

        internal const uint GW_OWNER = 4;

        internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern int GetWindowThreadProcessId(IntPtr hWnd, out IntPtr lpdwProcessId);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        #endregion

        #region 根据窗体句柄隐藏窗体

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        private const int SW_HIDE = 0;                                                          //常量，隐藏 
        private const int SW_SHOWNORMAL = 1;                                                    //常量，显示，标准状态 
        private const int SW_SHOWMINIMIZED = 2;                                                 //常量，显示，最小化 
        private const int SW_SHOWMAXIMIZED = 3;                                                 //常量，显示，最大化 
        private const int SW_SHOWNOACTIVATE = 4;                                                //常量，显示，不激活 
        private const int SW_RESTORE = 9;                                                       //常量，显示，回复原状 
        private const int SW_SHOWDEFAULT = 10;                                                  //常量，显示，默认 

        #endregion

        #region 根据进程隐藏窗体

        public static Process[] process;

        public static void ProcessMainWindowVisibleSetting(string processName, bool isShow)
        {
            try
            {
                if (process == null)
                {
                    process = Process.GetProcessesByName(processName);
                }
                if (process.Count() > 0)
                {
                    IntPtr intptr = GetMainWindowHandle(process[0]);
                    if (intptr.ToInt32() > 0)
                    {
                        if (isShow)
                        {
                            ShowWindowAsync(intptr, 1);
                        }
                        else
                        {
                            ShowWindowAsync(intptr, 0);
                            LogManage.WriteLog(typeof(ProcessManage), "开始隐藏");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ProcessManage), ex);
            }
        }

        #endregion

        #region 根据名称获取进程句柄

        public static IntPtr GetProcessHandle(string processName)
        {
            IntPtr intptr = default(IntPtr);
            try
            {
                var process = Process.GetProcessesByName(processName);
                if (process.Count() > 0)
                {
                    intptr = process[0].Handle;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ProcessManage), ex);
            }
            return intptr;
        }

        #endregion

        #region 命令名称打开应用程序

        public static void OpenAppByAppName(string AppName)
        {
            System.Diagnostics.Process Proc;
            try
            {
                // 启动记事本 
                Proc = new System.Diagnostics.Process();
                Proc.StartInfo.FileName = AppName;
                Proc.StartInfo.UseShellExecute = false;
                Proc.StartInfo.RedirectStandardInput = true;
                Proc.StartInfo.RedirectStandardOutput = true;
                Proc.Start();
            }
            catch
            {
                Proc = null;
            }
        }

        public static Process OpenAppByAppNameGetProcess(string AppName)
        {
            System.Diagnostics.Process Proc;
            try
            {
                // 启动记事本 
                Proc = new System.Diagnostics.Process();
                Proc.StartInfo.FileName = AppName;
                Proc.StartInfo.UseShellExecute = false;
                Proc.StartInfo.RedirectStandardInput = true;
                Proc.StartInfo.RedirectStandardOutput = true;
                Proc.Start();
            }
            catch
            {
                Proc = null;
            }
            return Proc;
        }

        #endregion

        #region 执行cmd命令（无视图模式）

        public static void RunCmd(string cmd)
        {
            Thread thread = new Thread(new ThreadStart(() =>
            {
                Process process = new Process();
                process.StartInfo.FileName = "cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.StandardInput.WriteLine(cmd);
                process.StandardInput.WriteLine("exit");
                process.Dispose();
            }));
            thread.Start();
        }

        #endregion

        #region 判断当前进程是否为单例

        /// <summary>
        /// 判断当前进程是否为单例
        /// </summary>
        public static bool CheckCurrentProcessIsSingleInstance(Action callBack)
        {
            bool isSingleInstance = true;
            try
            {
                //获取当前的进程
                Process currentProceess = Process.GetCurrentProcess();
                //获取当前进程的名称
                string currentProcessName = currentProceess.ProcessName;
                //根据当前进程的名称去判断是否为单例
                var list = Process.GetProcessesByName(currentProcessName);
                if (list.Count() > 1)
                {
                    callBack();

                    ////关闭当前进程
                    //currentProceess.Kill();
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ProcessManage), ex);
            }
            return isSingleInstance;
        }

        #endregion

        #region 根据路径打开文件


        /// <summary>
        /// 根据路径打开文件
        /// </summary>
        /// <param name="AppName"></param>
        public static void OpenFileByLocalAddress(string FileName)
        {
            System.Diagnostics.Process Proc;
            try
            {
                if (System.IO.File.Exists(FileName))
                {
                    // 启动应用程序
                    Proc = new System.Diagnostics.Process();
                    Proc.StartInfo.FileName = FileName;
                    Proc.Start();
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ProcessManage), ex);
            }
        }

        static IntPtr handle = default(IntPtr);

        /// <summary>
        /// 根据路径打开文件
        /// </summary>
        /// <param name="AppName"></param>
        public static void OpenFileByLocalAddressReturnHandel(string FileName, Action<int, IntPtr> callback)
        {
            System.Diagnostics.Process Proc = null;
            try
            {
                if (System.IO.File.Exists(FileName))
                {
                    // 启动应用程序
                    Proc = new System.Diagnostics.Process();
                    Proc.StartInfo.FileName = FileName;
                    Proc.Start();
                    Thread.Sleep(2000);
                    if (Proc.HasExited)
                    {
                        handle = Proc.MainWindowHandle;

                        int ProcessID = Proc.Id;
                        callback(ProcessID, handle);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ProcessManage), ex);
            }
            finally
            {
                Proc.Dispose();
            }
        }

        static IntPtr handle2 = default(IntPtr);
        /// <summary>
        /// 根据路径打开文件
        /// </summary>
        /// <param name="AppName"></param>
        public static void OpenFileByLocalAddressReturnHandel2(string FileName, Action<int, IntPtr> callback)
        {


            System.Diagnostics.Process Proc = null;
            try
            {
                if (System.IO.File.Exists(FileName))
                {
                    // 启动应用程序
                    Proc = new System.Diagnostics.Process();
                    Proc.StartInfo.FileName = FileName;
                    Proc.Start();
                    Thread.Sleep(1000);
                    handle2 = Proc.MainWindowHandle;

                    int ProcessID = Proc.Id;
                    callback(ProcessID, handle2);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ProcessManage), ex);
            }
            finally
            {
                Proc.Dispose();
            }
        }

        #endregion
    }
}
