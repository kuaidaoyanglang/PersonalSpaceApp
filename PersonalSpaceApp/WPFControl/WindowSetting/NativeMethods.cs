//-----------------------------------------------------------------------
// <copyright file="NativeMethods.cs" company="Vadeware">
//     Copyright (c) Vadeware Enterprises. All rights reserved.
//    windows 函数的借口接口函数
// </copyright>
namespace PersonalSpaceApp.WPFControl.WindowSetting
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// 主窗体内部类
    /// </summary>
    public class NativeMethods
    {
        /// <summary>
        /// 带有外边框和标题的windows的样式
        /// </summary>
        public const long WS_CAPTION = 0X00C0000L;

        /// <summary>
        /// 系统菜单
        /// </summary>
        public const long WS_SYSMENU = 0x00080000L;

        /// <summary>
        /// window 扩展样式 分层显示
        /// </summary>
        public const long WS_EX_LAYERED = 0x00080000L;

        /// <summary>
        /// 带有alpha的样式
        /// </summary>
        public const long LWA_ALPHA = 0x00000002L;

        /// <summary>
        /// 颜色设置
        /// </summary>
        public const long LWA_COLORKEY = 0x00000001L;

        /// <summary>
        /// window的基本样式
        /// </summary>
        public const int GWL_STYLE = -16;

        /// <summary>
        /// window的扩展样式
        /// </summary>
        public const int GWL_EXSTYLE = -20;

        /// <summary>
        /// 设置窗体的样式
        /// </summary>
        /// <param name="handle">操作窗体的句柄</param>
        /// <param name="oldStyle">进行设置窗体的样式类型.</param>
        /// <param name="newStyle">新样式</param>
        [DllImport("User32.dll", EntryPoint = "SetWindowLong")]
        public static extern void SetWindowLong(IntPtr handle, int oldStyle, int newStyle);

        /// <summary>
        /// 获取窗体指定的样式.
        /// </summary>
        /// <param name="handle">操作窗体的句柄</param>
        /// <param name="style">要进行返回的样式</param>
        /// <returns>当前window的样式</returns>
        [DllImport("User32.dll", EntryPoint = "GetWindowLong")]
        public static extern long GetWindowLong(IntPtr handle, int style);

        /// <summary>
        /// 设置窗体的工作区域.
        /// </summary>
        /// <param name="handle">操作窗体的句柄.</param>
        /// <param name="handleRegion">操作窗体区域的句柄.</param>
        /// <param name="regraw">if set to <c>true</c> [regraw].</param>
        /// <returns>返回值</returns>
        [DllImport("User32.dll", EntryPoint = "SetWindowRgn")]
        public static extern int SetWindowRgn(IntPtr handle, IntPtr handleRegion, bool regraw);

        /// <summary>
        /// 创建带有圆角的区域.
        /// </summary>
        /// <param name="x1">左上角坐标的X值.</param>
        /// <param name="y1">左上角坐标的Y值.</param>
        /// <param name="x2">右下角坐标的X值.</param>
        /// <param name="y2">右下角坐标的Y值.</param>
        /// <param name="width">圆角椭圆的 width.</param>
        /// <param name="height">圆角椭圆的 height.</param>
        /// <returns>hRgn的句柄</returns>
        [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn(int x1, int y1, int x2, int y2, int width, int height);

        /// <summary>
        /// Sets the layered window attributes.
        /// </summary>
        /// <param name="handle">要进行操作的窗口句柄</param>
        /// <param name="colorKey">RGB的值</param>
        /// <param name="alpha">Alpha的值，透明度</param>
        /// <param name="flags">附带参数</param>
        /// <returns>true or false</returns>
        [DllImport("User32.dll", EntryPoint = "SetLayeredWindowAttributes")]
        public static extern bool SetLayeredWindowAttributes(IntPtr handle, int colorKey, int alpha, int flags);
    }
}
