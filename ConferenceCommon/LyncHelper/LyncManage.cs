using ConferenceCommon.EntityHelper;
using ConferenceCommon.IconHelper;
using ConferenceCommon.LogHelper;
using ConferenceCommon.TimerHelper;
using Microsoft.Lync.Model;
using Microsoft.Lync.Model.Conversation;
using Microsoft.Lync.Model.Conversation.AudioVideo;
using Microsoft.Lync.Model.Conversation.Sharing;
using Microsoft.Lync.Model.Extensibility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace ConferenceCommon.LyncHelper
{
    public class LyncManage
    {
        #region 清除lync客户端缓存文件

        /// <summary>
        /// 清除lync客户端缓存文件 15.0【2013 lync 版本】
        /// </summary>
        public static void ClearLyncAppData()
        {
            try
            {
                //获取lync的缓存目录
                string lyncAppDataRoot = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Microsoft\Office\15.0\Lync";
                //
                DirectoryInfo directoryInfo = new DirectoryInfo(lyncAppDataRoot);
                //删除目录里的文件
                directoryInfo.Delete(true);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(LyncManage), ex);
            }
            finally
            {

            }
        }

        #endregion      
    }
}
