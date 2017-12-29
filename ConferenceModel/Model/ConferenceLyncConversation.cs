
using ConferenceModel.ConferenceLyncConversationWebservice;
using ConferenceModel.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;

namespace ConferenceModel.Model
{
    public class ConferenceLyncConversation
    {
        #region 静态字段


        static ConferenceLyncConversationWebserviceSoapClient client;
        /// <summary>
        /// 启用lync扩展服务（语音）
        /// </summary>
        internal static ConferenceLyncConversationWebserviceSoapClient Client
        {
            get { return client; }
        }

        #endregion

        #region 事件回调

        /// 填充服务同步数据回调
        /// </summary>
        Action<bool> LyncConversationSync_CallBack = null;

        /// <summary>
        /// 投影大屏幕
        /// </summary>
        Action<bool> EnterBigScreen_CallBack = null;

        /// <summary>
        /// ppt演示控制
        /// </summary>
        Action<bool> PPTControl_CallBack = null;

        /// <summary>
        /// 填充会话
        /// </summary>
        Action<bool> FillConversation_CallBack = null;

        /// <summary>
        /// 移除会话
        /// </summary>
        Action<bool> RemoveConversation_CallBack = null;

        /// <summary>
        /// 查看是否包含相关会话
        /// </summary>
        Action<bool, string> ContainConversation_CallBack = null;

        /// <summary>
        /// 离开会话
        /// </summary>
        Action<bool> LeaveConversation_CallBack = null;

        /// <summary>
        /// 允许会话初始化
        /// </summary>
        Action<bool> AllowConversationInit_CallBack = null;

        /// <summary>
        /// 允许会话初始化
        /// </summary>
        Action<bool> ForbiddenConversationInit_CallBack = null;

        /// <summary>
        /// 允许会话初始化
        /// </summary>
        Action<bool> CheckConversationInit_CallBack = null;

        /// <summary>
        /// 强制移除会话回调
        /// </summary>
        Action<bool> ForceRemoveConversation_CallBack = null;

        /// <summary>
        /// 共享应用程序回调
        /// </summary>
        Action<bool> ShareApplicationResourceNotify_CallBack = null;

        #endregion

        #region 客户端对象模型初始化

        /// <summary>
        /// 客户端对象模型初始化
        /// </summary>
        internal void ConferenceInfoInit(BasicHttpBinding binding, EndpointAddress endpoint)
        {
            try
            {
                if (client == null)
                {
                    //webservice客户端对象模型生成
                    client = new ConferenceLyncConversationWebserviceSoapClient(binding, endpoint);
                    //凭据的设置
                    client.ClientCredentials.Windows.AllowedImpersonationLevel =
                        TokenImpersonationLevel.Impersonation;
                    //同步lync会话完成事件
                    Client.JoinConferenceMainLyncConversationCompleted += client_JoinConferenceMainLyncConversationCompleted;
                    //投影大屏幕
                    Client.InterBigScreenCompleted += client_InterBigScreenCompleted;
                    //填充会话
                    Client.FillConversationCompleted += client_FillConversationCompleted;
                    //移除会话
                    Client.RemoveConversationCompleted += client_RemoveConversationCompleted;
                    //强制移除会话
                    Client.ForceRemoveConversationCompleted += Client_ForceRemoveConversationCompleted;
                    //查看是否包含相关会话
                    Client.ContainConversationCompleted += client_ContainConversationCompleted;
                    //离开会话
                    Client.LeaveConversationCompleted += client_LeaveConversationCompleted;
                    //PPT演示控制
                    Client.PPTControlCompleted += Client_PPTControlCompleted;
                    //允许会话初始化
                    Client.AllowConversationInitCompleted += Client_AllowConversationInitCompleted;
                    //禁止会话初始化
                    Client.ForbiddenConversationInitCompleted += Client_ForbiddenConversationInitCompleted;
                    //查询是否可以进行会话初始化
                    Client.CheckConversationInitCompleted += Client_CheckConversationInitCompleted;

                    Client.ShareApplicationResourceNotifyCompleted += Client_ShareApplicationResourceNotifyCompleted;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }
      
        #endregion

        #region 查看是否包含相关会话

        public void ContainConversation(int conferenceID, Action<bool, string> callBack)
        {
            try
            {
                this.ContainConversation_CallBack = callBack;
                Client.ContainConversationAsync(conferenceID);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        void client_ContainConversationCompleted(object sender, ContainConversationCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this.ContainConversation_CallBack != null)
                    {
                        this.ContainConversation_CallBack(true, e.Result);
                    }
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

        #region 移除会话

        public void RemoveConversation(int conferenceID, Action<bool> callBack)
        {
            try
            {
                this.RemoveConversation_CallBack = callBack;
                Client.RemoveConversationAsync(conferenceID);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        void client_RemoveConversationCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this.RemoveConversation_CallBack != null)
                    {
                        this.RemoveConversation_CallBack(true);
                    }
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

        public void ForceRemoveConversation(int conferenceID, Action<bool> callBack)
        {
            try
            {
                this.ForceRemoveConversation_CallBack = callBack;
                Client.ForceRemoveConversationAsync(conferenceID);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        void Client_ForceRemoveConversationCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    this.ForceRemoveConversation_CallBack(true);
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

        #region 填充会话

        public void FillConversation(int conferenceID, string meetingAddress, Action<bool> callback)
        {
            try
            {
                this.FillConversation_CallBack = callback;
                Client.FillConversationAsync(conferenceID, meetingAddress);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        void client_FillConversationCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this != null)
                    {
                        this.FillConversation_CallBack(true);
                    }
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

        #region 投影大屏幕

        public void EnterBigScreen(int conferenceID, string sharer, Action<bool> callBack)
        {
            //设置事件回调
            this.EnterBigScreen_CallBack = callBack;
            //同步lync会话
            Client.InterBigScreenAsync(conferenceID, sharer);
        }

        void client_InterBigScreenCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    //调用异步回调
                    this.EnterBigScreen_CallBack(true);
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

        #region ppt演示控制

        public void PPTControl(int conferenceID, string controler, Action<bool> callBack)
        {
            try
            {
                //设置事件回调
                this.PPTControl_CallBack = callBack;
                Client.PPTControlAsync(conferenceID, controler);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        void Client_PPTControlCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    //调用异步回调
                    this.PPTControl_CallBack(true);
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

        #region 离开会话

        public void LeaveConversation(int conferenceID, string contactUri, Action<bool> callBack)
        {
            try
            {
                this.LeaveConversation_CallBack = callBack;
                Client.LeaveConversationAsync(conferenceID, contactUri);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }
        void client_LeaveConversationCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    //调用异步回调
                    this.LeaveConversation_CallBack(true);
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

        #region 同步lync会话

        /// <summary>
        /// 同步lync会话
        /// </summary>
        /// <param name="confrenceName"></param>
        /// <param name="lyncConversationEntity"></param>
        /// <param name="callBack"></param>
        public void JoinConferenceMainLyncConversation(int conferenceID, LyncConversationEntity lyncConversationEntity, Action<bool> callBack)
        {
            try
            {
                //设置事件回调
                this.LyncConversationSync_CallBack = callBack;
                //同步lync会话
                Client.JoinConferenceMainLyncConversationAsync(conferenceID, lyncConversationEntity);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
        }

        /// <summary>
        /// 同步lync会话完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_JoinConferenceMainLyncConversationCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    //调用异步回调
                    this.LyncConversationSync_CallBack(true);
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

        #region 允许会话初始化

        public void AllowConversationInit(int conferenceID, Action<bool> callBack)
        {
            try
            {
                this.AllowConversationInit_CallBack = callBack;
                Client.AllowConversationInitAsync(conferenceID);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
        }

        void Client_AllowConversationInitCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    this.AllowConversationInit_CallBack(true);
                }
                else
                {
                    this.AllowConversationInit_CallBack(false);
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

        #region 禁止会话初始化

        public void ForbiddenConversationInit(int conferenceID, Action<bool> callBack)
        {
            try
            {
                this.ForbiddenConversationInit_CallBack = callBack;
                Client.ForbiddenConversationInitAsync(conferenceID);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
        }

        void Client_ForbiddenConversationInitCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    this.ForbiddenConversationInit_CallBack(true);
                }
                else
                {
                    this.ForbiddenConversationInit_CallBack(false);
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

        #region 共享应用程序通知

        /// <summary>
        /// 共享应用程序通知
        /// </summary>
        /// <param name="conference"></param>
        /// <param name="lyncResourceNotify"></param>
        /// <param name="callBack"></param>
        public void ShareApplicationResourceNotify(int conferenceID, LyncResourceNotify lyncResourceNotify, Action<bool> callBack)
        {
            try
            {
                this.ShareApplicationResourceNotify_CallBack = callBack;
                Client.ShareApplicationResourceNotifyAsync(conferenceID, lyncResourceNotify);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 共享应用程序通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_ShareApplicationResourceNotifyCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    this.ShareApplicationResourceNotify_CallBack(true);
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


        #region 查询是否可以会话初始化

        public void CheckConversationInit(int conferenceID, Action<bool> callBack)
        {
            try
            {
                this.CheckConversationInit_CallBack = callBack;
                Client.CheckConversationInitAsync(conferenceID);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {

            }
        }

        void Client_CheckConversationInitCompleted(object sender, CheckConversationInitCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    this.CheckConversationInit_CallBack(e.Result);
                }
                else
                {
                    this.CheckConversationInit_CallBack(false);
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
