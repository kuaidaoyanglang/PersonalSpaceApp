using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceCommon.FileHelper
{
    public class CutFile
    {
        /// <summary>
        /// 创建快捷方式
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="description"></param>
        /// <param name="root"></param>
        /// <param name="ico"></param>
        public void CreateCutFile(string fileName, string description, string root, string ico)
        {
            string DesktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);//得到桌面文件夹 
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShellClass();


            IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(DesktopPath + "\\知识研讨客户端.lnk");
            shortcut.TargetPath = fileName;
            shortcut.Arguments = "";// 参数 
            shortcut.Description = description;
            shortcut.WorkingDirectory = root;//程序所在文件夹，在快捷方式图标点击右键可以看到此属性 
            //shortcut.IconLocation = @"D:\software\cmpc\zy.exe,0";//图标 
            shortcut.IconLocation = ico + ",0";//图标 
            //shortcut.Hotkey = "CTRL+SHIFT+Z";//热键 
            shortcut.WindowStyle = 1;
            shortcut.Save();
        }
    }
}

/*
         * 网站的快捷方式
        IWshRuntimeLibrary.IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(DesktopPath + "\\自动创建+.lnk");
        shortcut.TargetPath = @"%HOMEDRIVE%/Program Files\Internet Explorer\IEXPLORE.EXE";
        shortcut.Arguments = "http://www.baidu.com";// 参数 
        shortcut.Description = "快捷链接到网站";
        shortcut.WorkingDirectory = Application.StartupPath;//程序所在文件夹，在快捷方式图标点击右键可以看到此属性 
        shortcut.IconLocation = @"%HOMEDRIVE%/Program Files\Internet Explorer\IEXPLORE.EXE, 0";//图标 
        shortcut.Hotkey = "CTRL+SHIFT+Z";//热键 
        shortcut.WindowStyle = 1;
        shortcut.Save();
        */
