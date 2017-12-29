using ConferenceCommon.LogHelper;
using ConferenceCommon.TimerHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ConferenceCommon.WPFHelper
{
   public static  class VisibilityManage
    {

       /// <summary>
       /// 显示并在短时间内关闭
       /// </summary>
       /// <param name="visibility">隐藏属性</param>
       public static void SetShowThanHidenVisibility(FrameworkElement element)
       {
             try
            {
                if (element.Visibility == System.Windows.Visibility.Collapsed)
                {
                    //提示显示
                    element.Visibility = System.Windows.Visibility.Visible;
                    TimerJob.StartRun(new Action(() =>
                    {
                        element.Visibility = System.Windows.Visibility.Collapsed;
                    }), 1000);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(VisibilityManage), ex);
            }
            finally
            {
            }
       }

       /// <summary>
       /// 显示并在短时间内关闭
       /// </summary>
       /// <param name="visibility">隐藏属性</param>
       public static void SetShowThanHidenVisibility(FrameworkElement element,long microSecond)
       {
           try
           {
               if (element.Visibility == System.Windows.Visibility.Collapsed)
               {
                   //提示显示
                   element.Visibility = System.Windows.Visibility.Visible;
                   TimerJob.StartRun(new Action(() =>
                   {
                       element.Visibility = System.Windows.Visibility.Collapsed;
                   }), microSecond);
               }
           }
           catch (Exception ex)
           {
               LogManage.WriteLog(typeof(VisibilityManage), ex);
           }
           finally
           {
           }
       }
    }
}
