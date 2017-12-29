using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ConferenceCommon.TimerHelper
{
    public class TimerManage
    {
        //设置系统时间的API函数
        [DllImport("kernel32.dll")]
        private static extern bool SetLocalTime(ref SYSTEMTIME time);

        /// <summary>
        /// 系统时间类
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEMTIME
        {
            public short year;
            public short month;
            public short dayOfWeek;
            public short day;
            public short hour;
            public short minute;
            public short second;
            public short milliseconds;
        }

        /// <summary>
        /// 设置系统时间
        /// </summary>
        /// <param name="dt">需要设置的时间</param>
        /// <returns>返回系统时间设置状态，true为成功，false为失败</returns>
        public static bool SetDate(DateTime dt)
        {
            bool result = false;
            try
            {
                //设置系统时间
                SYSTEMTIME st = default(SYSTEMTIME);
                //设置年
                st.year = (short)dt.Year;
                //月
                st.month = (short)dt.Month;
                //星期
                st.dayOfWeek = (short)dt.DayOfWeek;
                //日期
                st.day = (short)dt.Day;
                //小时
                st.hour = (short)dt.Hour;
                //分钟
                st.minute = (short)dt.Minute;
                //秒
                st.second = (short)dt.Second;
                //毫秒
                st.milliseconds = (short)dt.Millisecond;
                //设置系统时间
                result = SetLocalTime(ref st);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(TimerManage), ex);
            }
            finally
            {

            }
            return result;
        }
    }
}
