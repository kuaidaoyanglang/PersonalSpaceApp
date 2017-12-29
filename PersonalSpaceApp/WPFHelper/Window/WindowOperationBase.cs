using ConferenceCommon.IconHelper;
using ConferenceCommon.LogHelper;
using ConferenceCommon.TimerHelper;
using PersonalSpaceApp.WPFControl.WindowSetting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using win32 = ConferenceCommon.IconHelper.Win32API;

namespace PersonalSpaceApp.WPFHelper
{
    public class WindowOperationBase : WindowBase
    {
        #region 内部字段

        /// <summary>
        /// 会话窗体状态管理模型
        /// </summary>
        win32.ManagedWindowPlacement Placement = new win32.ManagedWindowPlacement() { showCmd = 2 };

        #endregion

        #region 自定义事件

        //public event EventHandler CloseDownHandle = null;

        //public void CloseDown()
        //{
        //    var handle = CloseDownHandle;
        //    this.Close();
        //    if (handle != null)
        //        handle(this, EventArgs.Empty);
        //}

        public void CloseDown()
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        #endregion

        #region 构造函数

        public WindowOperationBase()
        {
            try
            {
                this.Loaded += MainWindow_Loaded;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }

        }

        #endregion

        #region 透明自定义窗体设置

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // 获取窗体句柄
                IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;

                // 获得窗体的 样式
                long oldstyle = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_STYLE);

                // 更改窗体的样式为无边框窗体
                NativeMethods.SetWindowLong(hwnd, NativeMethods.GWL_STYLE, (int)oldstyle & ~(int)NativeMethods.WS_CAPTION);

                // SetWindowLong(hwnd, GWL_EXSTYLE, oldstyle & ~WS_EX_LAYERED);
                // 1 | 2 << 8 | 3 << 16  r=1,g=2,b=3 详见winuse.h文件
                // 设置窗体为透明窗体
                NativeMethods.SetLayeredWindowAttributes(hwnd, 1 | 2 << 8 | 3 << 16, 0, (int)NativeMethods.LWA_ALPHA);

                // 创建圆角窗体
                //this.RefreshRoundWindowRect();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            try
            {
                base.OnSourceInitialized(e);
                //WindowSetting.ChangeWindowSize changeWindowSize = new WindowSetting.ChangeWindowSize(this);
                //changeWindowSize.RegisterHook();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// Refreshes the round window rect.
        /// </summary>
        private void RefreshRoundWindowRect()
        {
            try
            {
                // 获取窗体句柄
                IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;

                // 创建圆角窗体
                NativeMethods.SetWindowRgn(hwnd, NativeMethods.CreateRoundRectRgn(0, 0, Convert.ToInt32(this.ActualWidth) + 1, Convert.ToInt32(this.ActualHeight) + 1, 12, 12), true);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        #endregion

        #region 设置窗体大小(按比例设置)

        protected void Window_Seize(double widthPercent, double heightPercent)
        {
            try
            {
                this.WindowState = System.Windows.WindowState.Normal;
                //获取屏幕分辨率
                System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.PrimaryScreen;
                //获取宽度
                this.Width = screen.Bounds.Width * widthPercent;
                //获取高度
                this.Height = screen.Bounds.Height * heightPercent;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        #endregion

        #region 窗体显示

        public void Window_Show()
        {
            try
            {
                if (!this.IsActive)
                {
                    this.Show();
                }
                TimerJob.StartRun(new Action(() =>
                    {
                        this.Activate();
                    }), 20);

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
             
            }
        }

        #endregion


    }
}
