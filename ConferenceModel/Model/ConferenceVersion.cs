
using ConferenceModel.ConferenceVersionWebservice;
using ConferenceModel.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;

namespace ConferenceModel.Model
{
    public class ConferenceVersion
    {
        #region 静态字段

        static ConferenceVersionWebserviceSoapClient client;
        /// <summary>
        /// 启用版本更新
        /// </summary>
        internal static ConferenceVersionWebserviceSoapClient Client
        {
            get { return client; }
        }

        #endregion

        #region 事件回调
        
        /// <summary>
        /// 是否更新版本事件回调
        /// </summary>
        Action<bool, Exception> NeedVersionUpdate_callBack = null;

        #endregion

        #region 注册事件

        /// <summary>
        /// 客户端对象模型初始化
        /// </summary>
        internal void ConferenceVersionInit(BasicHttpBinding binding, EndpointAddress endpoint)
        {
            try
            {
                if (client == null)
                {
                    client = new ConferenceVersionWebserviceSoapClient(binding, endpoint);
                    client.ClientCredentials.Windows.AllowedImpersonationLevel =
                       TokenImpersonationLevel.Impersonation;

                    Client.NeedVersionUpdateCompleted += ConferenceVersionClient_NeedVersionUpdateCompleted;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }
        
        #endregion

        #region 更新版本
        
        /// <summary>
        /// 更新版本
        /// </summary>
        /// <param name="PCVersion"></param>
        /// <param name="callBack"></param>
        public void NeedVersionUpdate(string PCVersion, Action<bool, Exception> callBack)
        {
            try
            {
                NeedVersionUpdate_callBack = callBack;
                Client.NeedVersionUpdateAsync(PCVersion);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 更新版本完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ConferenceVersionClient_NeedVersionUpdateCompleted(object sender, ConferenceVersionWebservice.NeedVersionUpdateCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (NeedVersionUpdate_callBack != null)
                    {
                        NeedVersionUpdate_callBack(e.Result, e.Error);
                    } 
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion
    }
}
