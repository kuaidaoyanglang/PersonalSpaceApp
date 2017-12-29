using ConferenceModel.LogHelper;
using ConferenceModel.FileSyncAppPoolWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;

namespace ConferenceModel.Model
{
    public class FileSyncAppPool
    {
        #region 静态字段

        static FileSyncAppPoolWebserviceSoapClient client;
        /// <summary>
        /// 甩屏同步
        /// </summary>
        internal static FileSyncAppPoolWebserviceSoapClient Client
        {
            get { return client; }
        }

        #endregion

        #region 事件回调
     
        /// 填充服务同步数据回调
        /// </summary>
        Action<bool> FillSyncServiceData_CallBack = null;

        /// <summary>
        /// 获取服务同步数据回调
        /// </summary>
        Action<bool, FileSyncAppEntity> GetSyncServiceData_CallBack = null;

        #endregion

        #region 注册事件

        /// <summary>
        /// 客户端对象模型初始化
        /// </summary>
        internal void FileSyncAppPoolInit(BasicHttpBinding binding, EndpointAddress endpoint)
        {
            try
            {
                if (client == null)
                {
                    client = new FileSyncAppPoolWebserviceSoapClient(binding, endpoint);

                    client.ClientCredentials.Windows.AllowedImpersonationLevel =
                       TokenImpersonationLevel.Impersonation;
                    Client.FillSyncServiceDataCompleted += FileSyncAppPoolClient_FillSyncServiceDataCompleted;
                    Client.GetSyncServiceDataCompleted += FileSyncAppPoolClient_GetSyncServiceDataCompleted;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion
      
        #region 填充服务同步数据

        /// <summary>
        /// 填充服务同步数据
        /// </summary>
        /// <param name="conferenceName"></param>
        /// <param name="bytes"></param>
        /// <param name="callBack"></param>
        public void FillSyncServiceData(int conferenceID, byte[] bytes, Action<bool> callBack)
        {
            try
            {
                this.FillSyncServiceData_CallBack = callBack;

                Client.FillSyncServiceDataAsync(conferenceID, bytes);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 填充服务同步数据完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FileSyncAppPoolClient_FillSyncServiceDataCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this.FillSyncServiceData_CallBack != null)
                    {
                        this.FillSyncServiceData_CallBack(true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 获取服务同步数据

        /// <summary>
        /// 获取服务同步数据
        /// </summary>
        /// <param name="conferenceName"></param>
        /// <param name="callBack"></param>
        public void GetSyncServiceData(int conferenceID, Action<bool, FileSyncAppEntity> callBack)
        {
            try
            {
                this.GetSyncServiceData_CallBack = callBack;
                Client.GetSyncServiceDataAsync(conferenceID);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 获取服务同步数据完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void FileSyncAppPoolClient_GetSyncServiceDataCompleted(object sender, FileSyncAppPoolWebService.GetSyncServiceDataCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this.GetSyncServiceData_CallBack != null)
                    {
                        this.GetSyncServiceData_CallBack(true, e.Result);
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
