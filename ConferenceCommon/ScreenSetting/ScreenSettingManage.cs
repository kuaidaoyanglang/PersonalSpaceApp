using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace ConferenceCommon.ScreenSetting
{
    public static class ScreenSettingManage
    {
        #region 静态字段

        ///// <summary>
        ///// 屏幕分辨率宽度
        ///// </summary>
        //static int screenWidth = 0;

        ///// <summary>
        ///// 屏幕分辨率高度
        ///// </summary>
        //static int screenHeight = 0;

        #endregion

        #region 改变分辨率

        /// <summary>
        /// 改变分辨率
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void ChangeResolution(int width, int height)
        {
            try
            {
                if (width != 0 && height != 0)
                {
                   
                    // 初始化 DEVMODE结构
                    DEVMODE devmode = new DEVMODE();
                    devmode.dmDeviceName = new String(new char[32]);
                    devmode.dmFormName = new String(new char[32]);
                    devmode.dmSize = (short)Marshal.SizeOf(devmode);

                    if (0 != NativeMethods.EnumDisplaySettings(null, NativeMethods.ENUM_CURRENT_SETTINGS, ref devmode))
                    {
                        devmode.dmPelsWidth = width;
                        devmode.dmPelsHeight = height;

                        // 改变屏幕分辨率
                        int iRet = NativeMethods.ChangeDisplaySettings(ref devmode, NativeMethods.CDS_TEST);

                        if (iRet == NativeMethods.DISP_CHANGE_FAILED)
                        {
                            LogManage.WriteLog(typeof(ScreenSettingManage), "不能执行你的请求");
                        }
                        else
                        {
                            iRet = NativeMethods.ChangeDisplaySettings(ref devmode, NativeMethods.CDS_UPDATEREGISTRY);

                            switch (iRet)
                            {
                                // 成功改变
                                case NativeMethods.DISP_CHANGE_SUCCESSFUL:
                                    {
                                        break;
                                    }
                                case NativeMethods.DISP_CHANGE_RESTART:
                                    {
                                        LogManage.WriteLog(typeof(ScreenSettingManage), "你需要重新启动电脑设置才能生效");
                                        break;
                                    }
                                default:
                                    {
                                        LogManage.WriteLog(typeof(ScreenSettingManage), "改变屏幕分辨率失败");
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ScreenSettingManage), ex);
            }
            finally
            {

            }
        }

        #endregion

        #region 还原之前的屏幕分辨率

        ///// <summary>
        ///// 还原之前的屏幕分辨率
        ///// </summary>
        //public static void ReturnResolution()
        //{
        //    try
        //    {
        //        //还原
        //        ChangeResolution(screenWidth, screenHeight);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManage.WriteLog(typeof(ScreenSettingManage), ex);
        //    }
        //    finally
        //    {

        //    }
        //}

        #endregion
    }
}
