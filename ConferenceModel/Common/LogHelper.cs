using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
//using log4net;

namespace ConferenceModel.LogHelper
{
    [assembly: log4net.Config.XmlConfigurator(Watch = true)]
    public static class LogManage
    {
        public static ILog OSSLog;

        public static void LogInit()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        #region 输出日志到Log4Net

        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="t"></param>
        /// <param name="ex"></param>
        public static void WriteLog(Type t, Exception ex)
        {
           var trace = ex.StackTrace;
            OSSLog = LogManager.GetLogger(t);

            OSSLog.Info("------------------------------------------------");
            OSSLog.Info(string.Empty, ex);
            OSSLog.Info("------------------------------------------------");
        }

        #endregion

        #region 输出日志到Log4Net

        /// <summary>
        /// 输出日志到Log4Net
        /// </summary>
        /// <param name="t"></param>
        /// <param name="msg"></param>
        public static void WriteLog(Type t, string msg)
        {
            log4net.Config.XmlConfigurator.Configure();
            OSSLog = LogManager.GetLogger(t);
            OSSLog.Info("------------------------------------------------");
            OSSLog.Info(msg);
            OSSLog.Info("------------------------------------------------");
        }

        #endregion
      
    }
}