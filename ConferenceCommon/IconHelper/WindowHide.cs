using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ConferenceCommon.IconHelper
{
    public static class WindowHide
    {
        ///// <summary>
        ///// 判断进程是否已经初始化完毕
        ///// </summary>
        //public static bool MainWindowIsShow = false;

        //获取托盘指针 
        private static IntPtr TrayToolbarWindow32()
        {
            IntPtr h = IntPtr.Zero;
            IntPtr hTemp = IntPtr.Zero;

            h = Win32API.FindWindow("Shell_TrayWnd", null); //托盘容器 
            h = Win32API.FindWindowEx(h, IntPtr.Zero, "TrayNotifyWnd", null);//找到托盘 
            h = Win32API.FindWindowEx(h, IntPtr.Zero, "SysPager", null);

            hTemp = Win32API.FindWindowEx(h, IntPtr.Zero, "ToolbarWindow32", null);

            return hTemp;
        }

        //获取托盘图标列表 
        public static List<WindowInfo> GetIconList()
        {
            List<WindowInfo> iconList = new List<WindowInfo>();
            try
            {
                IntPtr pid = IntPtr.Zero;
                IntPtr ipHandle = IntPtr.Zero; //图标句柄 
                IntPtr lTextAdr = IntPtr.Zero; //文本内存地址 

                IntPtr ipTray = TrayToolbarWindow32();

                Win32API.GetWindowThreadProcessId(ipTray, ref pid);
                if (pid.Equals(0)) return iconList;

                IntPtr hProcess = Win32API.OpenProcess(Win32API.PROCESS_ALL_ACCESS | Win32API.PROCESS_VM_OPERATION | Win32API.PROCESS_VM_READ | Win32API.PROCESS_VM_WRITE, IntPtr.Zero, pid);
                IntPtr lAddress = Win32API.VirtualAllocEx(hProcess, 0, 4096, Win32API.MEM_COMMIT, Win32API.PAGE_READWRITE);

                //得到图标个数 
                int lButton = Win32API.SendMessage(ipTray, Win32API.TB_BUTTONCOUNT, 0, 0);

                for (int i = 0; i < lButton; i++)
                {
                    Win32API.SendMessage(ipTray, Win32API.TB_GETBUTTON, i, lAddress);

                    //读文本地址 
                    Win32API.ReadProcessMemory(hProcess, (IntPtr)(lAddress.ToInt32() + 16), ref lTextAdr, 4, 0);

                    if (!lTextAdr.Equals(-1))
                    {
                        byte[] buff = new byte[1024];

                        Win32API.ReadProcessMemory(hProcess, lTextAdr, buff, 1024, 0);//读文本 
                        string title = System.Text.ASCIIEncoding.Unicode.GetString(buff);

                        // 从字符0处截断 
                        int nullindex = title.IndexOf("\0");
                        if (nullindex > 0)
                        {
                            title = title.Substring(0, nullindex);
                        }

                        IntPtr ipHandleAdr = IntPtr.Zero;

                        //读句柄地址 
                        Win32API.ReadProcessMemory(hProcess, (IntPtr)(lAddress.ToInt32() + 12), ref ipHandleAdr, 4, 0);
                        Win32API.ReadProcessMemory(hProcess, ipHandleAdr, ref ipHandle, 4, 0);

                        if (title.Replace("\0", "") == "") continue;//不加载空项 
                        iconList.Add(new WindowInfo(title, ipHandle));
                    }
                }
                Win32API.VirtualFreeEx(hProcess, lAddress, 4096, Win32API.MEM_RELEASE);
                Win32API.CloseHandle(hProcess);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(WindowHide), ex);
            }
            return iconList;
        }

        //获取进程窗体列表 
        public static List<WindowInfo> GetWindowList(int Handle)
        {
            List<WindowInfo> list = new List<WindowInfo>();
            try
            {
                const string CLASS_PARENT = "TXGuiFoundation";
                const string CLASS_CHILD = "ATL:006CC4D0";

                StringBuilder title = new StringBuilder(256);
                StringBuilder className = new StringBuilder(256);

                int h = Handle;// (int)Win32API.FindWindow(CLASS_PARENT, ""); 
                while (h != 0)
                {
                    //窗体的子窗体 
                    IntPtr p = Win32API.FindWindowEx(new IntPtr(h), IntPtr.Zero, CLASS_CHILD, string.Empty);

                    if (p != IntPtr.Zero)
                    {
                        Win32API.GetWindowText(h, title, title.Capacity);//得到窗口的标题 

                        if (title.ToString().Trim() != string.Empty)
                            Win32API.GetClassName(new IntPtr(h), className, className.Capacity);

                        if (className.ToString().Trim() != string.Empty && className.ToString() == CLASS_PARENT)
                        {
                            WindowInfo wi = new WindowInfo(title.ToString(), new IntPtr(h));
                            list.Add(wi);
                        }
                    }

                    h = Win32API.GetWindow(h, Win32API.GW_HWNDNEXT);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(WindowHide), ex);
            }
            return list;
        }

        //获取MSN窗体列表 
        public static List<WindowInfo> GetMSNWindowList(int Handle)
        {
            List<WindowInfo> list = new List<WindowInfo>();
            try
            {
                const string CLASS_PARENT = "IMWindowClass";
                const string CLASS_CHILD = "IM Window Native WindowBar Class";



                StringBuilder title = new StringBuilder(256);
                StringBuilder className = new StringBuilder(256);

                int h = Handle;// (int)Win32API.FindWindow(CLASS_PARENT, ""); 
                while (h != 0)
                {
                    //窗体的子窗体 
                    IntPtr p = Win32API.FindWindowEx(new IntPtr(h), IntPtr.Zero, CLASS_CHILD, "");

                    if (p != IntPtr.Zero)
                    {
                        Win32API.GetWindowText(h, title, title.Capacity);//得到窗口的标题 

                        if (title.ToString().Trim() != "")
                            Win32API.GetClassName(new IntPtr(h), className, className.Capacity);

                        if (className.ToString().Trim() != "" && className.ToString() == CLASS_PARENT)
                        {
                            WindowInfo wi = new WindowInfo(title.ToString(), new IntPtr(h));
                            list.Add(wi);
                        }
                    }
                    h = Win32API.GetWindow(h, Win32API.GW_HWNDNEXT);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(WindowHide), ex);
            }
            return list;
        }

        //获取打开的窗体列表 
        public static List<WindowInfo> GetHandleList(int Handle)
        {
            List<WindowInfo> fromInfo = new List<WindowInfo>();
            try
            {
                int handle = Win32API.GetWindow(Handle, Win32API.GW_HWNDFIRST);
                while (handle > 0)
                {
                    int IsTask = Win32API.WS_VISIBLE | Win32API.WS_BORDER | Win32API.WS_MAXIMIZEBOX;//窗体可见、有边框、有最大化按钮     

                    //int IsTask = Win32API.WS_VISIBLE ;//窗体可见    
                    int lngStyle = Win32API.GetWindowLongA(handle, Win32API.GWL_STYLE);
                    bool TaskWindow = ((lngStyle & IsTask) == IsTask);
                    if (TaskWindow)
                    {
                        int length = Win32API.GetWindowTextLength(new IntPtr(handle));
                        StringBuilder stringBuilder = new StringBuilder(2 * length + 1);
                        Win32API.GetWindowText(handle, stringBuilder, stringBuilder.Capacity);
                        string strTitle = stringBuilder.ToString();
                        if (!string.IsNullOrEmpty(strTitle))
                        {
                            fromInfo.Add(new WindowInfo(strTitle, (IntPtr)handle));
                        }
                        else
                        {
                            fromInfo.Add(new WindowInfo("无标题", (IntPtr)handle));
                        }
                    }
                    handle = Win32API.GetWindow(handle, Win32API.GW_HWNDNEXT);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(WindowHide), ex);
            }
            return fromInfo;
        }

        //显示/隐藏所有系统托盘图标 
        public static void SetTrayIconAllVisible(bool visible)
        {
            IntPtr vHandle = TrayToolbarWindow32();
            int vCount = Win32API.SendMessage(vHandle, Win32API.TB_BUTTONCOUNT, 0, 0);
            IntPtr vProcessId = IntPtr.Zero;
            Win32API.GetWindowThreadProcessId(vHandle, ref vProcessId);
            IntPtr vProcess = Win32API.OpenProcess(Win32API.PROCESS_VM_OPERATION | Win32API.PROCESS_VM_READ |
            Win32API.PROCESS_VM_WRITE, IntPtr.Zero, vProcessId);
            IntPtr vPointer = Win32API.VirtualAllocEx(vProcess, (int)IntPtr.Zero, 0x1000,
            Win32API.MEM_RESERVE | Win32API.MEM_COMMIT, Win32API.PAGE_READWRITE);
            char[] vBuffer = new char[256];
            IntPtr pp = Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0);
            uint vNumberOfBytesRead = 0;
            try
            {
                for (int i = 0; i < vCount; i++)
                {
                    Win32API.SendMessage(vHandle, Win32API.TB_GETBUTTONTEXT, i, vPointer.ToInt32());

                    Win32API.ReadProcessMemoryEx(vProcess, vPointer,
                    Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
                    vBuffer.Length * sizeof(char), ref vNumberOfBytesRead);

                    if (visible)
                        Win32API.SendMessage(vHandle, Win32API.TB_HIDEBUTTON, i, 0);
                    else
                        Win32API.SendMessage(vHandle, Win32API.TB_HIDEBUTTON, i, 1);

                }
            }                
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(WindowHide), ex);
            }
            finally
            {
                Win32API.VirtualFreeEx(vProcess, vPointer, 0, Win32API.MEM_RELEASE);
                Win32API.CloseHandle(vProcess);
            }
        }

     static    List<string> list = new List<string>();

        //显示/隐藏单个系统托盘图标,由参数caption指定图标 
        public static void SetTrayIconVisible(string caption, bool isShow)
        {
            IntPtr vHandle = TrayToolbarWindow32();
            int vCount = Win32API.SendMessage(vHandle, Win32API.TB_BUTTONCOUNT, 0, 0);
            IntPtr vProcessId = IntPtr.Zero;
            Win32API.GetWindowThreadProcessId(vHandle, ref vProcessId);
            IntPtr vProcess = Win32API.OpenProcess(Win32API.PROCESS_VM_OPERATION | Win32API.PROCESS_VM_READ |
            Win32API.PROCESS_VM_WRITE, IntPtr.Zero, vProcessId);
            IntPtr vPointer = Win32API.VirtualAllocEx(vProcess, (int)IntPtr.Zero, 0x1000,
            Win32API.MEM_RESERVE | Win32API.MEM_COMMIT, Win32API.PAGE_READWRITE);
            char[] vBuffer = new char[256];
            IntPtr pp = Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0);
            uint vNumberOfBytesRead = 0;
            try
            {
                for (int i = 0; i < vCount; i++)
                {
                    Win32API.SendMessage(vHandle, Win32API.TB_GETBUTTONTEXT, i, vPointer.ToInt32());

                    Win32API.ReadProcessMemoryEx(vProcess, vPointer,
                    Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
                    vBuffer.Length * sizeof(char), ref vNumberOfBytesRead);

                    int l = 0;
                    for (int j = 0; j < vBuffer.Length; j++)
                    {
                        if (vBuffer[j] == (char)0)
                        {
                            l = j;
                            break;
                        }
                    }
                    string s = new string(vBuffer, 0, l);

                    var realC = s.IndexOf(caption);
                    if (realC>= 0)
                    {
                      
                        if (isShow)
                        {                            
                            Win32API.SendMessage(vHandle, Win32API.TB_HIDEBUTTON, i, 0);
                        }
                        else
                        {                         
                            Win32API.SendMessage(vHandle, Win32API.TB_HIDEBUTTON, i, 1);
                            //MainWindowIsShow = true;
                        }
                    }
                    list.Add(s);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(WindowHide), ex);
            }
            finally
            {
                Win32API.VirtualFreeEx(vProcess, vPointer, 0, Win32API.MEM_RELEASE);
                Win32API.CloseHandle(vProcess);
            }
        }

        //显示/隐藏单个系统托盘图标,由参数caption指定图标 
        public static void SetTrayIconAllDsiplay(string caption)
        {
            IntPtr vHandle = TrayToolbarWindow32();
            int vCount = Win32API.SendMessage(vHandle, Win32API.TB_BUTTONCOUNT, 0, 0);
            IntPtr vProcessId = IntPtr.Zero;
            Win32API.GetWindowThreadProcessId(vHandle, ref vProcessId);
            IntPtr vProcess = Win32API.OpenProcess(Win32API.PROCESS_VM_OPERATION | Win32API.PROCESS_VM_READ |
            Win32API.PROCESS_VM_WRITE, IntPtr.Zero, vProcessId);
            IntPtr vPointer = Win32API.VirtualAllocEx(vProcess, (int)IntPtr.Zero, 0x1000,
            Win32API.MEM_RESERVE | Win32API.MEM_COMMIT, Win32API.PAGE_READWRITE);
            char[] vBuffer = new char[256];
            IntPtr pp = Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0);
            uint vNumberOfBytesRead = 0;
            try
            {
                for (int i = 0; i < vCount; i++)
                {
                    Win32API.SendMessage(vHandle, Win32API.TB_GETBUTTONTEXT, i, vPointer.ToInt32());

                    Win32API.ReadProcessMemoryEx(vProcess, vPointer,
                    Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
                    vBuffer.Length * sizeof(char), ref vNumberOfBytesRead);

                    int l = 0;
                    for (int j = 0; j < vBuffer.Length; j++)
                    {
                        if (vBuffer[j] == (char)0)
                        {
                            l = j;
                            break;
                        }
                    }
                    string s = new string(vBuffer, 0, l);

                    if (s.IndexOf(caption) >= 0)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            Win32API.SendMessage(vHandle, Win32API.TB_HIDEBUTTON, j, 0);
                        }
                    }
                    Console.WriteLine(s);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(WindowHide), ex);
            }
            finally
            {
                Win32API.VirtualFreeEx(vProcess, vPointer, 0, Win32API.MEM_RELEASE);
                Win32API.CloseHandle(vProcess);
            }
        }


        //显示/隐藏所有进程相关的窗体及托盘图标（如QQ）
        public static void SetWindowVisible(IntPtr owner, string caption, bool visible)
        {
            try
            {
                //显示/隐藏窗体 
                IList<WindowInfo> list = WindowHide.GetWindowList((int)owner);
                foreach (WindowInfo wi in list)
                    Win32API.ShowWindow(wi.Handle, visible ? Win32API.SW_SHOW : Win32API.SW_HIDE);

                //显示/隐藏托盘图标 
                WindowHide.SetTrayIconVisible(caption, visible);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(WindowHide), ex);
            }
        }

        //显示/隐藏所有进程相关的窗体（如QQ）
        public static void SetWindowVisible(IntPtr owner, bool visible)
        {
            try
            {
                //const string CLASS_PARENT = "TXGuiFoundation";
                //const string CLASS_CHILD = "ATL:006CC4D0";

                //窗体的子窗体 
                //IntPtr p = Win32API.FindWindowEx(owner, IntPtr.Zero, CLASS_CHILD, string.Empty);

                StringBuilder title = new StringBuilder(256);
                StringBuilder className = new StringBuilder(256);

                Win32API.GetWindowText(owner.ToInt32(), title, title.Capacity);//得到窗口的标题 

                if (title.ToString().Trim() != "")
                    Win32API.GetClassName(owner, className, className.Capacity);

                Win32API.ShowWindow(owner, visible ? Win32API.SW_SHOW : Win32API.SW_HIDE);

                //显示/隐藏托盘图标 
                WindowHide.SetTrayIconVisible(title.ToString(), visible);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(WindowHide), ex);
            }
        }

        //显示/隐藏所有MSN窗体及MSN托盘图标 
        public static void SetMSNWindowVisible(IntPtr owner, bool visible)
        {
            try
            {
                //显示/隐藏MSN窗体 
                IList<WindowInfo> list = WindowHide.GetMSNWindowList((int)owner);
                foreach (WindowInfo wi in list)
                    Win32API.ShowWindow(wi.Handle, visible ? Win32API.SW_SHOW : Win32API.SW_HIDE);

                //显示/隐藏MSN托盘图标 
                //WindowHide.SetTrayIconVisible("Windows Live Messenger", visible);
              
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(WindowHide), ex);
            }
        }

        //显示/隐藏所有窗体及窗体托盘图标 
        public static void SetMSNWindowVisible(IntPtr owner,string caption, bool visible)
        {
            try
            {
                //显示/隐藏MSN窗体 
                IList<WindowInfo> list = WindowHide.GetMSNWindowList((int)owner);
                foreach (WindowInfo wi in list)
                {
                    Win32API.ShowWindow(wi.Handle, visible ? Win32API.SW_SHOW : Win32API.SW_HIDE);
                    //Win32API.CloseHandle(wi.Handle);
                    }
                //显示/隐藏MSN托盘图标 
                WindowHide.SetTrayIconVisible("caption", visible);

              

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(WindowHide), ex);
            }
        }
    }
}
