
using ConferenceModel.LogHelper;
using ConferenceModel.ConferenceInfoWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;

namespace ConferenceModel.Model
{
    public class ConferenceInfo
    {
        #region 静态字段


        static ConferenceInfoWebServiceSoapClient client;
        /// <summary>
        /// 启用lync扩展服务（语音）
        /// </summary>
        internal static ConferenceInfoWebServiceSoapClient Client
        {
            get { return client; }
        }

        #endregion

        #region 事件回调

        /// 填充服务同步数据回调
        /// </summary>
        Action<bool> FillConferenceInfoServiceData_CallBack = null;

        /// <summary>
        /// 获取服务同步数据回调
        /// </summary>
        //Action<bool, ConferenceInfoEntity> GetConferenceInfoServiceData_CallBack = null;

        /// <summary>
        /// 获取会议通讯端口回调
        /// </summary>
        Action<bool, PortTypeEntity> GetMeetPort_CallBack = null;

        /// <summary>
        /// 获取会议所有通讯端口回调
        /// </summary>
        Action<bool, PortTypeEntity[]> GetMeetAllPort_CallBack = null;

        /// <summary>
        /// 删除服务器关于本用户的通讯节点（套接字）
        /// </summary>
        Action<bool> RemoveSelfClientSocket_CallBack = null;

        /// <summary>
        /// 获取会议信息（当前参会人的所有会议）
        /// </summary>
        Action<bool, List<ConferenceInformationEntityPC>> GetConferenceInformation_CallBack = null;

        /// <summary>
        /// 更改会议信息（场景模式）
        /// </summary>
        Action<bool> UpdateSceneMode_CallBack = null;

        /// <summary>
        /// 更改会议信息（投影模式）
        /// </summary>
        Action<bool> UpdateMxtrixMode_CallBack = null;

        /// <summary>
        /// 获取客户端配置信息函数回调
        /// </summary>
        Action<bool, ClientConfigEntity> GetClientAppConfig_CallBack = null;

        /// <summary>
        /// 获取客户端配置信息函数回调
        /// </summary>
        Action<bool> ClientControl_CallBack = null;

        /// <summary>
        /// 移除当前用户所有通讯节点函数回调
        /// </summary>
        Action<bool> RemoveCurrentUser_AllClientSocket_CallBack = null;

        /// <summary>
        /// 查看当前用户是否在线
        /// </summary>
        Action<bool> CheckUserIsOnline_CallBack = null;

        #endregion

        #region 客户端对象模型初始化

        /// <summary>
        /// 客户端对象模型初始化
        /// </summary>
        internal void ConferenceInfoInit(BasicHttpBinding binding, EndpointAddress endpoint)
        {
            try
            {
                if (ConferenceInfo.client == null)
                {
                    //webservice客户端对象模型生成
                    ConferenceInfo.client = new ConferenceInfoWebServiceSoapClient(binding, endpoint);
                    //凭据的设置
                    ConferenceInfo.client.ClientCredentials.Windows.AllowedImpersonationLevel =
                        TokenImpersonationLevel.Impersonation;
                    //获取会议信息完成事件
                    ConferenceInfo.Client.GetTempInformationCompleted += Client_GetTempInformationCompleted;
                    //填充同步数据完成事件
                    ConferenceInfo.Client.FillSyncServiceDataCompleted += ConferenceInfoAsyncClient_FillSyncServiceDataCompleted;
                    //启动服务通讯机制并返回侦听的端口号（完成事件）
                    ConferenceInfo.Client.RunServerSocketCompleted += Client_RunServerSocketCompleted;

                    ConferenceInfo.Client.RunServerAllSocketCompleted += Client_RunServerAllSocketCompleted;
                    //自我删除通讯节点
                    ConferenceInfo.Client.RemoveClientSocketCompleted += Client_RemoveClientSocketCompleted;
                    //保持服务器应用持续
                    ConferenceInfo.Client.SetKeepAliveCompleted += Client_SetKeepAliveCompleted;
                    //获取客户端配置信息
                    ConferenceInfo.Client.GetClientConfigInformationCompleted += Client_GetClientConfigInformationCompleted;
                    //更改客户端模式
                    ConferenceInfo.Client.UpdateTempInformationAboutSceneModeCompleted += Client_UpdateTempInformationCompleted;
                    //更改投影模式
                    ConferenceInfo.Client.UpdateTempInformationAboutMaxtrixModeCompleted += Client_UpdateTempInformationAboutMaxtrixModeCompleted;
                    //客户端统一命令
                    ConferenceInfo.Client.ClientControlCompleted += Client_ClientControlCompleted;
                    //移除当前所有通讯节点
                    ConferenceInfo.Client.RemoveAllClientSocketBySomeOneCompleted += Client_RemoveAllClientSocketBySomeOneCompleted;
                    //判断当前用户是否在线
                    ConferenceInfo.Client.CheckUserIsOnlineCompleted += Client_CheckUserIsOnlineCompleted;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 获取客户端配置信息

        /// <summary>
        /// 获取客户端配置信息
        /// </summary>
        public void GetClientAppConfig(Action<bool, ClientConfigEntity> callback)
        {
            try
            {

                if (ConferenceInfo.client != null)
                {
                    //绑定函数回调
                    this.GetClientAppConfig_CallBack = callback;
                    //异步获取会议信息
                    ConferenceInfo.Client.GetClientConfigInformationAsync();
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

        /// <summary>
        /// 获取客户端配置信息完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_GetClientConfigInformationCompleted(object sender, GetClientConfigInformationCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this.GetClientAppConfig_CallBack != null)
                    {
                        //获取会议信息回调
                        this.GetClientAppConfig_CallBack(true, e.Result);
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

        #region 获取指定会议信息

        /// <summary>
        /// 获取指定会议信息
        /// </summary>
        /// <param name="selfName"></param>
        /// <param name="callBack"></param>
        public void GetConferenceInformation(string selfName, Action<bool, List<ConferenceInformationEntityPC>> callBack)
        {
            try
            {
                if (ConferenceInfo.client != null)
                {
                    //获取会议信息回调
                    this.GetConferenceInformation_CallBack = callBack;
                    //异步获取会议信息
                    ConferenceInfo.Client.GetTempInformationAsync();
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ConferenceInfo), ex);
            }
        }

        /// <summary>
        /// 获取指定会议信息完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_GetTempInformationCompleted(object sender, GetTempInformationCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this.GetConferenceInformation_CallBack != null)
                    {
                        //获取会议信息回调
                        this.GetConferenceInformation_CallBack(true, e.Result.ToList<ConferenceInformationEntityPC>());
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(ConferenceInfo), ex);
            }
        }

        #endregion

        #region 模式切换（场景模式切换）

        /// <summary>
        /// 模式切换（场景模式切换）
        /// </summary>
        /// <param name="conferenceName"></param>
        /// <param name="sceneModeType"></param>
        /// <param name="callBack"></param>
        public void ChangeSceneModel(int conferenceID, SceneModeType sceneModeType, Action<bool> callBack)
        {
            try
            {
                if (ConferenceInfo.client != null)
                {
                    //模式切换回调
                    this.UpdateSceneMode_CallBack = callBack;
                    //异步获取会议信息
                    ConferenceInfo.Client.UpdateTempInformationAboutSceneModeAsync(conferenceID, sceneModeType);
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
        void Client_UpdateTempInformationCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this.UpdateSceneMode_CallBack != null)
                    {
                        this.UpdateSceneMode_CallBack(true);
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

        #region 投影模式切换

        /// <summary>
        /// 投影模式切换
        /// </summary>
        /// <param name="conferenceName"></param>
        /// <param name="maxtrixModeType"></param>
        /// <param name="callBack"></param>
        public void UpdateMaxtrixUsingMode(int conferenceID, MaxtrixModeType maxtrixModeType, Action<bool> callBack)
        {
            try
            {
                ConferenceInfo.Client.UpdateTempInformationAboutMaxtrixModeAsync(conferenceID, maxtrixModeType);
                this.UpdateMxtrixMode_CallBack = callBack;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        void Client_UpdateTempInformationAboutMaxtrixModeCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    this.UpdateMxtrixMode_CallBack(true);
                }
                else
                {
                    this.UpdateMxtrixMode_CallBack(false);
                }
            }
            catch (Exception ex)
            {
                this.UpdateMxtrixMode_CallBack(false);
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        #endregion

        #region 客户端统一命令

        /// <summary>
        /// 客户端统一命令
        /// </summary>
        public void ClientControl(int conferenceID, string commander, ClientControlType ClientControlType, Action<bool> callBack)
        {
            try
            {
                if (ConferenceInfo.client != null)
                {
                    //模式切换回调
                    this.ClientControl_CallBack = callBack;
                    //异步获取会议信息
                    ConferenceInfo.Client.ClientControlAsync(conferenceID, commander, ClientControlType);
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

        void Client_ClientControlCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this.ClientControl_CallBack != null)
                    {
                        this.ClientControl_CallBack(true);
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

        #region 获取会议通讯端口

        /// <summary>
        /// 获取会议通讯端口
        /// </summary>
        /// <param name="conferenceName">会议名称</param>   
        public void GetMeetPort(int conferenceID, ConferenceClientAcceptType conferenceClientAcceptType, Action<bool, PortTypeEntity> callBack)
        {
            try
            {
                if (ConferenceInfo.client != null)
                {
                    //绑定会议通讯节点回调
                    this.GetMeetPort_CallBack = callBack;
                    //异步获取会议通讯节点
                    ConferenceInfo.Client.RunServerSocketAsync(conferenceID, conferenceClientAcceptType);
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

        /// <summary>
        /// 启动服务通讯机制并返回侦听的端口号（完成事件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_RunServerSocketCompleted(object sender, RunServerSocketCompletedEventArgs e)
        {
            try
            {
                //没有调用异常
                if (e.Error == null)
                {
                    if (this.GetMeetPort_CallBack != null)
                    {
                        //会议通讯节点回调
                        this.GetMeetPort_CallBack(true, e.Result);
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

        /// <summary>
        /// 获取会议通讯端口
        /// </summary>
        /// <param name="conferenceName">会议名称</param>   
        public void GetMeetAllPort(int conferenceID, Action<bool, PortTypeEntity[]> callBack)
        {
            try
            {
                if (ConferenceInfo.client != null)
                {
                    //绑定会议通讯节点回调
                    this.GetMeetAllPort_CallBack = callBack;
                    //异步获取会议通讯节点
                    ConferenceInfo.Client.RunServerAllSocketAsync(conferenceID);
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

        void Client_RunServerAllSocketCompleted(object sender, RunServerAllSocketCompletedEventArgs e)
        {
            try
            {
                if(e.Error == null)
                {
                    this.GetMeetAllPort_CallBack(true, e.Result);
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

        #region 填充服务同步数据

        /// <summary>
        /// 填充服务同步数据
        /// </summary>
        /// <param name="conferenceName"></param>
        /// <param name="bytes"></param>
        /// <param name="callBack"></param>
        public void FillConferenceOfficeServiceData(int conferenceID, string sharer, ConferencePageType conferencePageType, Action<bool> callBack)
        {
            try
            {
                this.FillConferenceInfoServiceData_CallBack = callBack;

                ConferenceInfo.Client.FillSyncServiceDataAsync(conferenceID, sharer, conferencePageType);
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
        void ConferenceInfoAsyncClient_FillSyncServiceDataCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this.FillConferenceInfoServiceData_CallBack != null)
                    {
                        this.FillConferenceInfoServiceData_CallBack(true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 保持服务器应用持续

        /// <summary>
        /// 保持服务器应用持续
        /// </summary>
        public void SetKeepAlive()
        {
            try
            {
                //ConferenceInfo.Client.SetKeepAliveAsync();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 保持服务器应用持续完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_SetKeepAliveCompleted(object sender, SetKeepAliveCompletedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 自我删除通讯节点

        /// <summary>
        /// 自我删除通讯节点
        /// </summary>
        public void RemoveSelfClientSocket(int conferenceID, ConferenceClientAcceptType conferenceClientAcceptType, string contactUri, Action<bool> callback)
        {
            try
            {
                this.RemoveSelfClientSocket_CallBack = callback;
                ConferenceInfo.Client.RemoveClientSocketAsync(conferenceID, conferenceClientAcceptType, contactUri);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 自我删除通讯节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_RemoveClientSocketCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this.RemoveSelfClientSocket_CallBack != null)
                    {
                        this.RemoveSelfClientSocket_CallBack(true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 移除当前用户所有通讯节点

        public void RemoveCurrentUser_AllClientSocket(int conferenceID, string contactUri, Action<bool> callback)
        {
            try
            {
                this.RemoveCurrentUser_AllClientSocket_CallBack = callback;
                ConferenceInfo.Client.RemoveAllClientSocketBySomeOneAsync(conferenceID, contactUri);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }
        void Client_RemoveAllClientSocketBySomeOneCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this.RemoveCurrentUser_AllClientSocket_CallBack != null)
                    {
                        this.RemoveCurrentUser_AllClientSocket_CallBack(true);
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

        #region 检测当前用户是否在线

        public void CheckUserIsOnline(string uri, Action<bool> callBack)
        {
            try
            {
                CheckUserIsOnline_CallBack = callBack;
                ConferenceInfo.Client.CheckUserIsOnlineAsync(uri);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        void Client_CheckUserIsOnlineCompleted(object sender, CheckUserIsOnlineCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (CheckUserIsOnline_CallBack != null)
                    {
                        CheckUserIsOnline_CallBack(e.Result);
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
    }
}
