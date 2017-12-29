using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace ConferenceCommon.VersionHelper
{
    /// <summary>
    /// 通用方法（通用且与单个类没有太多依赖性的方法）
    /// </summary>
    public class VersionUpdateManage
    {       
        /// <summary>
        /// 版本检测更新(强制更新)
        /// </summary>
        public static void VersionUpdate(string updateExe)
        {
            try
            {
                var IsUpdateVersion = MessageBox.Show("版本更新，是否继续", "操作提示", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                //当前客户进行手动选择是否更新
                if (IsUpdateVersion == MessageBoxResult.OK)
                {
                    //版本更新应用程序名称
                    string openApp = updateExe;
                    if (File.Exists(openApp))
                    {
                        //通过进程去启动
                        Process process = new Process();
                        process.StartInfo.FileName = openApp;
                        process.Start();
                        //关闭当前应用程序
                        Application.Current.Shutdown(0);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(VersionUpdateManage), ex);
            }
            finally
            {

            }
        }

        /// <summary>
        /// 版本检测更新(强制更新)
        /// </summary>
        public static void JustUpdate(string updateExe)
        {
            try
            {             
                    //版本更新应用程序名称
                    string openApp = updateExe;
                    if (File.Exists(openApp))
                    {
                        //通过进程去启动
                        Process process = new Process();
                        process.StartInfo.FileName = openApp;
                        process.Start();
                        //关闭当前应用程序
                        Application.Current.Shutdown(0);
                    }                
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(VersionUpdateManage), ex);
            }
            finally
            {

            }
        }

    }
}
