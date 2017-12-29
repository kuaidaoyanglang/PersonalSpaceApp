using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace ConferenceCommon.TimerHelper
{
    public static class TimerJob
    {
        #region TimerJob@1

        /// <summary>
        /// 自定义计时器（200毫秒触发一次，执行完指定的任务后结束计时器）
        /// </summary>
        /// <param name="action">执行的内容</param>
        public static void StartRun(Action action)
        {
            try
            {
                Timer dispatcher_Timer = new Timer();
                dispatcher_Timer.Interval = 200;
                dispatcher_Timer.Elapsed += (object sender, ElapsedEventArgs e) =>
                {

                    dispatcher_Timer.Dispose();
                    if (Application.Current != null)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                      {
                          try
                          {
                              action();
                          }
                          catch (Exception ex)
                          {
                              dispatcher_Timer.Dispose();
                              LogManage.WriteLog(typeof(TimerJob), ex);
                          }
                      }));
                    }

                };
                dispatcher_Timer.Start();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(TimerJob), ex);
            }

        }

        public static void StartRun_Sync(Action action)
        {
            try
            {
                Timer dispatcher_Timer = new Timer();
                dispatcher_Timer.Interval = 200;
                dispatcher_Timer.Elapsed += (object sender, ElapsedEventArgs e) =>
                {

                    dispatcher_Timer.Dispose();
                    try
                    {
                        if (System.Threading.Thread.CurrentThread != null)
                        {
                            System.Threading.Thread.CurrentThread.IsBackground = true;
                        }
                        action();
                    }
                    catch (Exception ex)
                    {
                        dispatcher_Timer.Dispose();
                        LogManage.WriteLog(typeof(TimerJob), ex);
                    }
                };
                dispatcher_Timer.Start();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(TimerJob), ex);
            }

        }

        #endregion

        #region TimerJob@2

        /// <summary>
        /// 自定义计时器（指定时间内触发一次，执行完指定的任务后结束计时器）
        /// </summary>
        /// <param name="action">执行的内容</param>
        /// <param name="Milliseconds">指定时间</param>
        public static void StartRun(Action action, double Milliseconds)
        {
            try
            {
                Timer dispatcher_Timer = new Timer();
                dispatcher_Timer.Interval = Milliseconds;
                dispatcher_Timer.Elapsed += (object sender, ElapsedEventArgs e) =>
                {

                    dispatcher_Timer.Dispose();
                    if (Application.Current != null)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                       {
                           try
                           {
                               action();
                           }
                           catch (Exception ex)
                           {
                               dispatcher_Timer.Dispose();
                               LogManage.WriteLog(typeof(TimerJob), ex);
                           }
                       }));
                    }
                };
                dispatcher_Timer.Start();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(TimerJob), ex);
            }

        }

        /// <summary>
        /// 自定义计时器（指定时间内触发一次，执行完指定的任务后结束计时器）
        /// </summary>
        /// <param name="action">执行的内容</param>
        /// <param name="Milliseconds">指定时间</param>
        public static void StartRun_Sync(Action action, double Milliseconds)
        {
            try
            {
                Timer dispatcher_Timer = new Timer();
                dispatcher_Timer.Interval = Milliseconds;
                dispatcher_Timer.Elapsed += (object sender, ElapsedEventArgs e) =>
                {
                    dispatcher_Timer.Dispose();

                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        dispatcher_Timer.Dispose();
                        LogManage.WriteLog(typeof(TimerJob), ex);
                    }
                };
                dispatcher_Timer.Start();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(TimerJob), ex);
            }

        }

        #endregion

        #region TimerJob@3

        /// <summary>
        /// 自定义计时器（指定时间内触发一次，在达到预期的目的之后，通过外部引用计时器来进行相应的操作（如结束计时器的生命周期））
        /// </summary>
        /// <param name="action">执行的内容</param>
        /// <param name="Milliseconds">指定时间</param>
        /// <param name="timer">向外引用计时器</param>
        public static void StartRun(Action action, double Milliseconds, out Timer timer)
        {
            Timer dispatcher_Timer = new Timer();
            timer = dispatcher_Timer;
            try
            {
                dispatcher_Timer.Interval = Milliseconds;                
                dispatcher_Timer.Elapsed += (object sender, ElapsedEventArgs e) =>
                {
                    if (Application.Current != null)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            try
                            {
                                action();
                            }
                            catch (Exception ex)
                            {
                                dispatcher_Timer.Dispose();
                                LogManage.WriteLog(typeof(TimerJob), ex);
                            }
                        }));
                    }
                };
                dispatcher_Timer.Start();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(TimerJob), ex);
            }
        }

        /// <summary>
        /// 自定义计时器（指定时间内触发一次，在达到预期的目的之后，通过外部引用计时器来进行相应的操作（如结束计时器的生命周期））
        /// </summary>
        /// <param name="action">执行的内容</param>
        /// <param name="Milliseconds">指定时间</param>
        /// <param name="timer">向外引用计时器</param>
        public static void StartRun_Sync(Action action, double Milliseconds, out Timer timer)
        {
            Timer dispatcher_Timer = new Timer();
            timer = dispatcher_Timer;
            try
            {
                dispatcher_Timer.Interval = Milliseconds;
                dispatcher_Timer.Elapsed += (object sender, ElapsedEventArgs e) =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        dispatcher_Timer.Dispose();
                        LogManage.WriteLog(typeof(TimerJob), ex);
                    }
                };
                dispatcher_Timer.Start();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(TimerJob), ex);
            }
        }

        #endregion

        #region TimerJob@NOUsing

        /// <summary>
        /// 自定义计时器（指定时间内触发一次，执行完指定的任务后）
        /// </summary>
        /// <param name="action">执行的内容</param>
        /// <param name="Milliseconds">指定时间</param>
        public static void StartRunNoStop(Action action, double Milliseconds)
        {
            try
            {
                Timer dispatcher_Timer = new Timer();
                dispatcher_Timer.Interval = Milliseconds;
                dispatcher_Timer.Elapsed += (object sender, ElapsedEventArgs e) =>
                {
                    if (Application.Current != null)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                       {
                           try
                           {
                               action();
                           }
                           catch (Exception ex)
                           {
                               dispatcher_Timer.Dispose();
                               LogManage.WriteLog(typeof(TimerJob), ex);
                           }
                       }));
                    }
                };
                dispatcher_Timer.Start();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(TimerJob), ex);
            }
        }

        /// <summary>
        /// 自定义计时器（指定时间内触发一次，在达到预期的目的之后，通过外部引用计时器来进行相应的操作（如结束计时器的生命周期））
        /// </summary>
        /// <param name="action">执行的内容</param>
        /// <param name="Milliseconds">指定时间</param>
        /// <param name="timer">向外引用计时器</param>
        public static void StartRun(Action action, int actionCount, double Milliseconds)
        {
            Timer dispatcher_Timer = new Timer();

            int count = 0;

            dispatcher_Timer.Interval = Milliseconds;
            dispatcher_Timer.Elapsed += (object sender, ElapsedEventArgs e) =>
            {
                //进行计时
                count++;
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                         {
                             //执行操作
                             action();
                         }));
                //达到指定操作数量，停止
                if (count.Equals(actionCount))
                {
                    dispatcher_Timer.Dispose();
                }
            };
            dispatcher_Timer.Start();
        }


        /// <summary>
        /// 自定义计时器（指定时间内触发一次，在达到预期的目的之后，通过外部引用计时器来进行相应的操作（如结束计时器的生命周期））
        /// </summary>
        /// <param name="action">执行的内容</param>
        /// <param name="Milliseconds">指定时间</param>
        /// <param name="timer">向外引用计时器</param>
        public static void StartRun(Action action, int actionCount, double Milliseconds, out DispatcherTimer timer)
        {
            DispatcherTimer dispatcher_Timer = new DispatcherTimer();

            int count = 0;

            timer = dispatcher_Timer;

            dispatcher_Timer.Interval = TimeSpan.FromMilliseconds(Milliseconds);
            dispatcher_Timer.Tick += (object o, EventArgs e) =>
            {
                //进行计时
                count++;
                //执行操作
                action();
                //达到指定操作数量，停止
                if (count.Equals(actionCount))
                {
                    dispatcher_Timer.Stop();
                }
            };
            dispatcher_Timer.Start();
        }

        #endregion
    }
}
