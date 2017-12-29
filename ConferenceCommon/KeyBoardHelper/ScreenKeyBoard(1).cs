using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using ConferenceCommon.LogHelper;

namespace ConferenceCommon.KeyBoard
{
    public class ScreenKeyBoard
    {
        private const Int32 WM_MOVE = 0x0003;
        private const Int32 WM_SYSCOMMAND = 274;
        private const UInt32 SC_CLOSE = 61536;
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "PostMessage")]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "PostMessage")]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "PostMessage")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "RegisterWindowMessage")]
        private static extern int RegisterWindowMessage(string lpString);


        private static readonly IntPtr HWND_TOP = new IntPtr(0); //将窗口置于Z序的顶部
        private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1); //将窗口置于所有非顶层窗口之上。即使窗口未被激活窗口也将保持顶级位置
        private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);  //不是顶部


        //标识位uFlags
        private const UInt32 SWP_NOSIZE = 0x0001; //保持当前的大小（忽略cx和cy参数）
        private const UInt32 SWP_NOMOVE = 0x0002; //保持当前的位置（忽略x和y参数）
        private const UInt32 SWP_NOZORDER = 0x0004;//保持当前的次序（忽略pWndInsertAfter）
        private const UInt32 SWP_NOREDRAW = 0x0008;//不重画变化。如果设置了这个标志，则不发生任何种类的变化
        private const UInt32 SWP_NOACTIVATE = 0x0010;//不激活窗口。如果没有设置这个标志，则窗口将被激活并移动到顶层或非顶层窗口组（依赖于pWndInsertAfter参数的设置）的顶部
        private const UInt32 SWP_FRAMECHANGED = 0x0020;//向窗口发送一条WM_NCCALCSIZE消息，即使窗口的大小不会改变。如果没有指定这个标志，则仅当窗口的大小发生变化时才发送WM_NCCALCSIZE消息
        private const UInt32 SWP_SHOWWINDOW = 0x0040;//显示窗口
        private const UInt32 SWP_HIDEWINDOW = 0x0080;//隐藏窗口
        private const UInt32 SWP_NOCOPYBITS = 0x0100;// 废弃这个客户区的内容。如果没有指定这个参数，则客户区的有效内容将被保存，
        private const UInt32 SWP_NOOWNERZORDER = 0x0200;//不改变拥有者窗口在Z轴次序上的位置。
        private const UInt32 SWP_NOSENDCHANGING = 0x0400;//防止窗口接收WM_WINDOWPOSCHANGING消息
        private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        /// <summary>
        /// 设置窗口位置
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="hWndInsertAfter">在z序中的位于被置位的窗口前的窗口句柄</param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="cx"></param>
        /// <param name="cy"></param>
        /// <param name="uFlags">标识位uFlags</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowPos")]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "MoveWindow")]
        private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetForegroundWindow")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowRect")]
        public static extern int GetWindowRect(IntPtr hwnd, ref System.Drawing.Rectangle lpRect);

        /// <summary>
        /// 显示屏幕键盘
        /// </summary>
        public static void ShowInputPanel()
        {
            try
            {
                using (Process _process = Process.Start("osk.exe")) { };

                Thread thread = new Thread(new ThreadStart(() =>
                    {
                        try
                        {
                            Thread.Sleep(200);
                            IntPtr TouchhWnd = IntPtr.Zero;
                            TouchhWnd = FindWindow(null, "屏幕键盘");
                            if (TouchhWnd != IntPtr.Zero)
                            {
                                //设置osk.exe屏幕键盘窗体位置、大小时程序需具备管理员权限，否则不起作用
                                SetWindowPos(TouchhWnd, HWND_TOPMOST, 0, 0, 400, 300, SWP_SHOWWINDOW);
                            }
                        }
                        catch { }
                    }));
                thread.Start();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ScreenKeyBoard), ex);
            }
        }

        /// <summary>
        /// 显示屏幕键盘
        /// </summary>
        public static void ShowInputPanel(int x, int y)
        {
            try
            {
                using (Process _process = Process.Start("osk.exe")) { };

                Thread thread = new Thread(new ThreadStart(() =>
                {
                    try
                    {
                        IntPtr TouchhWnd = IntPtr.Zero;
                        TouchhWnd = FindWindow(null, "屏幕键盘");
                        if (TouchhWnd != IntPtr.Zero)
                        {
                            //设置osk.exe屏幕键盘窗体位置、大小时程序需具备管理员权限，否则不起作用
                            SetWindowPos(TouchhWnd, HWND_TOPMOST, x, y, 1000, 300, SWP_SHOWWINDOW);
                        }
                    }
                    catch { }
                }));
                thread.Start();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ScreenKeyBoard), ex);
            }
        }

        /// <summary>
        /// 关闭屏幕键盘
        /// </summary>
        public static void HideInputPanel()
        {
            try
            {
                foreach (Process _process in Process.GetProcesses())
                {
                    if (_process.ProcessName == "osk")
                    {
                        _process.Kill();
                        _process.Close();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ScreenKeyBoard), ex);
            }
        }
    }
}
