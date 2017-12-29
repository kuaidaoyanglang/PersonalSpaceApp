using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ConferenceCommon.IconHelper
{
    /// <summary> 
    /// 操作Windows窗体,系统托盘所用的API函数 
    /// </summary> 
    public class Win32API
    {
        public const int WM_USER = 0x400;
        public const int WM_CLOSE = 0x10;
        public const int WM_GETTEXT = 0x000D;
        public const int WM_SETTEXT = 0x000C;

        public const int STANDARD_RIGHTS_REQUIRED = 0xF0000;
        public const int SYNCHRONIZE = 0x100000;
        public const int PROCESS_ALL_ACCESS = STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF;
        public const int PROCESS_TERMINATE = 0x1;

        public const int PROCESS_VM_OPERATION = 0x8;
        public const int PROCESS_VM_READ = 0x10;
        public const int PROCESS_VM_WRITE = 0x20;

        public const int MEM_RESERVE = 0x2000;
        public const int MEM_COMMIT = 0x1000;
        public const int MEM_RELEASE = 0x8000;

        public const int PAGE_READWRITE = 0x4;

        public const int TB_BUTTONCOUNT = (WM_USER + 24);
        public const int TB_HIDEBUTTON = (WM_USER + 4);
        public const int TB_GETBUTTON = (WM_USER + 23);
        public const int TB_GETBUTTONTEXT = WM_USER + 75;
        public const int TB_GETBITMAP = (WM_USER + 44);
        public const int TB_DELETEBUTTON = (WM_USER + 22);
        public const int TB_ADDBUTTONS = (WM_USER + 20);
        public const int TB_INSERTBUTTON = (WM_USER + 21);
        public const int TB_ISBUTTONHIDDEN = (WM_USER + 12);
        public const int ILD_NORMAL = 0x0;
        public const int TPM_NONOTIFY = 0x80;

        public const int WS_VISIBLE = 268435456;//窗体可见 
        public const int WS_MINIMIZEBOX = 131072;//有最小化按钮 
        public const int WS_MAXIMIZEBOX = 65536;//有最大化按钮 
        public const int WS_BORDER = 8388608;//窗体有边框 
        public const int GWL_STYLE = (-16);//窗体样式 
        public const int GW_HWNDFIRST = 0;
        public const int GW_HWNDNEXT = 2;
        public const int SW_HIDE = 0;
        public const int SW_SHOW = 5;


        //public const int WM_WININICHANGE = 0x001A;
        //public const int WM_SETTINGCHANGE = WM_WININICHANGE;



        [DllImport("User32.Dll")]
        public static extern void GetClassName(IntPtr hwnd, StringBuilder s, int nMaxCount);

        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow", SetLastError = true)]
        public static extern void SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "GetDlgItem", SetLastError = true)]
        public static extern IntPtr GetDlgItem(int nID, IntPtr phWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int RegisterWindowMessage(string msg);

        [DllImport("kernel32", EntryPoint = "OpenProcess")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, IntPtr bInheritHandle, IntPtr dwProcessId);

        [DllImport("kernel32", EntryPoint = "CloseHandle")]
        public static extern int CloseHandle(IntPtr hObject);

        [DllImport("user32", EntryPoint = "GetWindowThreadProcessId")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hwnd, ref IntPtr lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);

        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, ref IntPtr lpBuffer, int nSize, int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        public static extern bool ReadProcessMemoryEx(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);

        [DllImport("kernel32", EntryPoint = "ReadProcessMemory")]
        public static extern int ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize, int lpNumberOfBytesWritten);

        [DllImport("kernel32", EntryPoint = "WriteProcessMemory")]
        public static extern int WriteProcessMemory(IntPtr hProcess, ref int lpBaseAddress, ref int lpBuffer, int nSize, ref int lpNumberOfBytesWritten);

        [DllImport("kernel32", EntryPoint = "VirtualAllocEx")]
        public static extern IntPtr VirtualAllocEx(IntPtr hProcess, int lpAddress, int dwSize, int flAllocationType, int flProtect);

        [DllImport("kernel32", EntryPoint = "VirtualFreeEx")]
        public static extern int VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, int dwFreeType);

        [DllImport("User32.dll")]
        public extern static int GetWindow(int hWnd, int wCmd);

        [DllImport("User32.dll")]
        public extern static int GetWindowLongA(int hWnd, int wIndx);

        [DllImport("user32.dll")]
        public static extern bool GetWindowText(int hWnd, StringBuilder title, int maxBufSize);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public extern static int GetWindowTextLength(IntPtr hWnd);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindowEx", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


        #region 窗体通讯

        //给窗体发送消息（win32窗体基本都是通过windows消息去通知并触发相应的事件）
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        #endregion

        #region 窗体状态


        [DllImport("user32.dll")]
        public static extern bool GetWindowPlacement(IntPtr handle, ref ManagedWindowPlacement placement);
        [DllImport("user32.dll")]
        public static extern bool SetWindowPlacement(IntPtr handle, ref ManagedWindowPlacement placement);
        [Serializable]   //必须加上，否则后面会提示为非可序列化标记
        public struct ManagedWindowPlacement
        {
            public int length;
            public int flags;
            public int showCmd;//若为2则是正常状态，若为1则为最小化状态
            public Point ptMinPosition;
            public Point ptMaxPosition;
            public Rectangle rcNormalPosition;
        }

        #endregion      ;

        #region 窗体特效

        //定义公共变量

        public const Int32 AW_HOR_POSITIVE = 0x00000001;    //自左向右显示窗体
        public const Int32 AW_HOR_NEGATIVE = 0x00000002;    //自右向左显示窗体
        public const Int32 AW_VER_POSITIVE = 0x00000004;    //自上而下显示窗体
        public const Int32 AW_VER_NEGATIVE = 0x00000008;    //自下而上显示窗体
        public const Int32 AW_CENTER = 0x00000010;          //窗体向外扩展
        public const Int32 AW_HIDE = 0x00010000;            //隐藏窗体
        public const Int32 AW_ACTIVATE = 0x00020000;        //激活窗体
        public const Int32 AW_SLIDE = 0x00040000;           //使用滚动动画类型
        public const Int32 AW_BLEND = 0x00080000;           //使用淡入效果



        //声明AnimateWindow函数
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);


        //AnimateWindow(this.Handle, 2000, AW_HOR_POSITIVE);      //自左向右滚动窗体动画效果

        //AnimateWindow(this.Handle, 2000, AW_SLIDE + AW_HOR_POSITIVE);  //自左向右滑动窗体动画效果

        //AnimateWindow(this.Handle, 2000, AW_HOR_NEGATIVE);   //自右向左滚动窗体动画效果

        //AnimateWindow(this.Handle, 2000, AW_SLIDE + AW_HOR_NEGATIVE);  //自右向左滑动窗体动画效果

        // AnimateWindow(this.Handle, 2000, AW_VER_POSITIVE);   //自上向下滚动窗体动画效果

        //AnimateWindow(this.Handle, 2000, AW_SLIDE + AW_VER_POSITIVE);  // 自上向下滑动窗体动画效果

        //AnimateWindow(this.Handle, 2000, AW_VER_NEGATIVE);//自下向上滚动窗体动画效果

        //AnimateWindow(this.Handle, 2000, AW_SLIDE + AW_VER_NEGATIVE);  //自下向上滑动窗体动画效果

        //AnimateWindow(this.Handle, 2000, AW_SLIDE + AW_CENTER);  // 向外扩展窗体动画效果

        // AnimateWindow(this.Handle, 2000, AW_BLEND);  //淡入窗体动画效果

        #endregion

        #region 鼠标自动点击

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(MouseEventFlag flags,
               int dx, int dy, uint data, UIntPtr extraInfo);

        [Flags]
        public enum MouseEventFlag : uint
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,
            VirtualDesk = 0x4000,
            Absolute = 0x8000
        }

        #endregion

        #region 窗体移动

        //窗口设置，可移动
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        #endregion

        #region 窗体大小设置（置顶）

        //窗口设置，可置顶
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);

        #endregion

        #region 设置背景颜色

        #endregion


        #region 寻找进程

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        [DllImport("kernel32.dll")]
        public static extern int OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);  

        #endregion

    }
}
