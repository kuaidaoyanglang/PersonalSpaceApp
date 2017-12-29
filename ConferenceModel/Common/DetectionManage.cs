
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace ConferenceModel.DetectionHelper
{
    public static class DetectionModelManage
    {
        #region  验证web服务是否可以使用

        /// <summary>
        /// 验证web服务是否可以使用
        /// </summary>
        /// <param name="url">web服务地址</param>
        /// <returns>返回验证消息</returns>
        public static bool IsWebServiceAvaiable(String url)
        {
            bool result = false;
            try
            {
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                using (HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse())
                {
                    result = true; ;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }

        #endregion

        #region 验证IP是否能通

        /// <summary>
        /// 验证是否可通过网络访问远程计算机
        /// </summary>
        static Ping pingSender = new Ping();

        /// <summary>
        /// 控制ping的传输数据包
        /// </summary>
        static PingOptions options = new PingOptions();

        /// <summary>
        /// 字符编码
        /// </summary>
        static byte[] buffer = Encoding.ASCII.GetBytes("t");

        /// <summary>
        /// 数据包生存时间
        /// </summary>
        static int timeout = 3000;

        /// <summary>
        /// 
        /// </summary>
        //static bool ping_Lock = false;

        /// <summary>  
        /// 验证某个IP是否可ping通  
        /// </summary>  
        /// <param name="strIP">要验证的IP</param>  
        /// <returns>是否可连通  是：true 否：false</returns>  
        public static bool TestNetConnectity(string strIP)
        {
            bool result = false;

            try
            {
                options.DontFragment = true;

                PingReply reply = pingSender.Send(strIP, timeout, buffer, options);

                if (reply.Status == IPStatus.Success)
                {
                    result = true;
                }
            }
            catch (Exception)
            {
                //LogManage.WriteLog(typeof(DetectionManage), ex);
            }

            return result;
        }

        #endregion
    }
}
