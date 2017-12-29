
using ConferenceModel.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Threading;

namespace ConferenceCommon.TimerHelper
{
    public static class TimerModelJob
    {
        #region TimerJob@2

        ///// <summary>
        ///// 自定义计时器（指定时间内触发一次，执行完指定的任务后结束计时器）
        ///// </summary>
        ///// <param name="action">执行的内容</param>
        ///// <param name="Milliseconds">指定时间</param>
        //public static void StartRun(Action action, double Milliseconds)
        //{
        //    try
        //    {
        //        Timer dispatcher_Timer = new Timer();
        //        dispatcher_Timer.Interval = Milliseconds;
        //        dispatcher_Timer.Elapsed += (object sender, ElapsedEventArgs e) =>
        //        {

        //            dispatcher_Timer.Dispose();
        //            if (Application.Current != null)
        //            {
        //                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
        //                {
        //                    try
        //                    {
        //                        action();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        dispatcher_Timer.Dispose();
        //                        LogManage.WriteLog(typeof(TimerJob), ex);
        //                    }
        //                }));
        //            }
        //        };
        //        dispatcher_Timer.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManage.WriteLog(typeof(TimerJob), ex);
        //    }

        //}

        ///// <summary>
        ///// 自定义计时器（指定时间内触发一次，执行完指定的任务后结束计时器）
        ///// </summary>
        ///// <param name="action">执行的内容</param>
        ///// <param name="Milliseconds">指定时间</param>
        //public static void StartRun_Sync(Action action, double Milliseconds)
        //{
        //    try
        //    {
        //        Timer dispatcher_Timer = new Timer();
        //        dispatcher_Timer.Interval = Milliseconds;
        //        dispatcher_Timer.Elapsed += (object sender, ElapsedEventArgs e) =>
        //        {
        //            dispatcher_Timer.Dispose();

        //            try
        //            {
        //                action();
        //            }
        //            catch (Exception ex)
        //            {
        //                dispatcher_Timer.Dispose();
        //                LogManage.WriteLog(typeof(TimerJob), ex);
        //            }
        //        };
        //        dispatcher_Timer.Start();
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManage.WriteLog(typeof(TimerJob), ex);
        //    }

        //}

        #endregion
    }
}
