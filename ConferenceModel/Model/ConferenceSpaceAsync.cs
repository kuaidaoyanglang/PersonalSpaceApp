using ConferenceModel.LogHelper;
using ConferenceModel.ConferenceSpaceAsyncWebservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;

namespace ConferenceModel.Model
{
    public class ConferenceSpaceAsync
    {
        #region 静态字段

        static ConferenceSpaceAsyncWebserviceSoapClient client;
        /// <summary>
        /// Word同步
        /// </summary>
        internal static ConferenceSpaceAsyncWebserviceSoapClient Client
        {
            get { return client; }
        }

        #endregion

        #region 事件回调

        /// 填充服务同步数据回调
        /// </summary>
        Action<bool> FillConferenceOfficeServiceData_CallBack = null;

        /// 获取智存空间推送文件回调
        /// </summary>
        Action<bool, ConferenceSpaceAsyncEntity> GetSyncSpaceSendFile_CallBack = null;

        #endregion

        #region 注册事件

        /// <summary>
        /// 客户端对象模型初始化
        /// </summary>
        internal void ConferenceOfficeAsyncInit(BasicHttpBinding binding, EndpointAddress endpoint)
        {
            try
            {
                if (client == null)
                {
                    //生成客户端对象模型
                    client = new ConferenceSpaceAsyncWebservice.ConferenceSpaceAsyncWebserviceSoapClient(binding, endpoint);

                    //设置凭据类型
                    client.ClientCredentials.Windows.AllowedImpersonationLevel =
                       TokenImpersonationLevel.Impersonation;

                    //填充服务器数据（智存空间文件共享）
                    Client.FillSyncServiceDataCompleted += Client_FillSyncServiceDataCompleted;
                    //获取服务器数据（获取智存空间推送文件）
                    Client.GetSyncServiceDataCompleted += Client_GetSyncServiceDataCompleted;

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
        public void FillConferenceOfficeServiceData(int conferenceID, string sharer, string uri, FileType fileType, Action<bool> callBack)
        {
            try
            {
                this.FillConferenceOfficeServiceData_CallBack = callBack;

                Client.FillSyncServiceDataAsync(conferenceID, sharer, uri, fileType);
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
        void Client_FillSyncServiceDataCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this.FillConferenceOfficeServiceData_CallBack != null)
                    {
                        this.FillConferenceOfficeServiceData_CallBack(true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 获取智存空间推送文件

        /// <summary>
        /// 获取智存空间推送文件
        /// </summary>
        public void GetSyncSpaceSendFile(int conferenceID, Action<bool, ConferenceSpaceAsyncEntity> callBack)
        {
            try
            {
                this.GetSyncSpaceSendFile_CallBack = callBack;
                Client.GetSyncServiceDataAsync(conferenceID);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        void Client_GetSyncServiceDataCompleted(object sender, GetSyncServiceDataCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    this.GetSyncSpaceSendFile_CallBack(true, e.Result);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        #endregion
    }
}
