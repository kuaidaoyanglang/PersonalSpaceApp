using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceModel.Common
{
    public static class CommunicationCodeEnterEntity
    {
        /// <summary>
        /// 会议名称
        /// </summary>
        public static string ConferenceName { get; set; }

        /// <summary>
        /// 会议ID
        /// </summary>
        public static int ConferenceID { get; set; }
       
        /// <summary>
        /// 当前用户的uri地址
        /// </summary>
        public static string SelfUri { get; set; }
                   
        /// <summary>
        /// Tree服务IP
        /// </summary>
        public static string TreeServiceIP { get; set; }

        /// <summary>
        /// 路由器ip
        /// </summary>
        public static string RouteIp { get; set; }

        /// <summary>
        /// Tree服务web服务地址
        /// </summary>
        public static string TreeServiceAddressFront { get; set; }

        /// <summary>
        /// 研讨同步服务目录（树）
        /// </summary>
        public static string ConferenceTreeServiceWebName { get; set; }
    }
}
