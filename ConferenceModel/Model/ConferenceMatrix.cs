using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConferenceModel.ConferenceMatrixWebservice;
using System.ServiceModel;
using System.Security.Principal;
using ConferenceModel.LogHelper;

namespace ConferenceModel.Model
{
    public class ConferenceMatrix
    {
        #region 静态字段

        static ConferenceMatrixWebserviceSoapClient client;
        /// <summary>
        /// 启用lync扩展服务（语音）
        /// </summary>
        internal static ConferenceMatrixWebserviceSoapClient Client
        {
            get { return client; }
        }

        #endregion

        #region 事件回调

        /// 矩阵设置回调
        /// </summary>
        Action<bool> MatrixSync_CallBack = null;

        /// 获取投影信息回调
        /// </summary>
        Action<bool, ConferenceMatrixEntity> GetMaxtrixInfo_CallBack = null;

        /// 进入座位回调
        /// </summary>
        Action<bool, List<SeatEntity>> IntoOneSeatSync_CallBack = null;

        /// <summary>
        /// 离开座位
        /// </summary>
        Action<bool> LeaveOneSeatSync_CallBack = null;

        /// <summary>
        /// 申请投影
        /// </summary>
        Action<bool> ApplyMaxtrixProjection_CallBack = null;

        /// <summary>
        /// 发送投影操作控制回调
        /// </summary>
        Action<bool> SendMaxtrixControlCode_CallBack = null;

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
                    client = new ConferenceMatrixWebserviceSoapClient(binding, endpoint);
                    //凭据的设置
                    client.ClientCredentials.Windows.AllowedImpersonationLevel =
                        TokenImpersonationLevel.Impersonation;
                    //投影设置完成事件
                    Client.SetMatrixEntityCompleted += Client_SetMatrixEntityCompleted;
                    //进入一个座位完成事件
                    Client.InToOneSeatCompleted += Client_InToOneSeatCompleted;
                    //离开座位完成事件
                    Client.LeaveOneSeatCompleted += Client_LeaveOneSeatCompleted;
                    //申请投影
                    Client.ApplyMatrixEntityCompleted += Client_ApplyMatrixEntityCompleted;
                    //获取投影信息
                    Client.GetMatrixEntityCompleted += Client_GetMatrixEntityCompleted;
                    //发送投影操作控制
                    Client.SendMaxtrixControlCodeCompleted += Client_SendMaxtrixControlCodeCompleted;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }
       
        #endregion

        #region 投影占用

        /// <summary>
        /// 投影占用
        /// </summary>
        /// <param name="confrenceName"></param>
        /// <param name="lyncConversationEntity"></param>
        /// <param name="callBack"></param>
        public void MatrixSetting(int conferenceID, ConferenceMatrixEntity conferenceMatrixEntity, Action<bool> callBack)
        {
            try
            {
                //设置事件回调
                this.MatrixSync_CallBack = callBack;
                //投影占用
                Client.SetMatrixEntityAsync(conferenceID, conferenceMatrixEntity);
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
        /// 投影占用完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_SetMatrixEntityCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    this.MatrixSync_CallBack(true);
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

        #region 获取投影信息

        /// <summary>
        /// 获取投影信息
        /// </summary>
        public void GetMaxtrixInfo(int conferenceID, Action<bool, ConferenceMatrixEntity> callBack)
        {
              try
            {
                GetMaxtrixInfo_CallBack = callBack;
                Client.GetMatrixEntityAsync(conferenceID);
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
        /// 获取投影信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_GetMatrixEntityCompleted(object sender, GetMatrixEntityCompletedEventArgs e)
        {
            try
            {
                if(e.Error == null)
                {
                    GetMaxtrixInfo_CallBack(true,e.Result);                    
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

        #region 投影申请

        /// <summary>
        /// 投影申请
        /// </summary>
        public void ApplyMaxtrixProjection(int conferenceID,ConferenceMaxtrixApplyEntity conferenceMaxtrixApplyEntity,Action<bool> callBack)
        {
            try
            {
                this.ApplyMaxtrixProjection_CallBack = callBack;
                Client.ApplyMatrixEntityAsync(conferenceID, conferenceMaxtrixApplyEntity);
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
        /// 投影申请
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_ApplyMatrixEntityCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    this.ApplyMaxtrixProjection_CallBack(true);
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

        #region 发送投影操作控制

        /// <summary>
        /// 发送投影操作控制
        /// </summary>
        public void SendMaxtrixControlCode(int conferenceID,ConferenceMatrixThrowCode conferenceMatrixThrowCode,Action<bool> callBack)
        {
            try
            {
                this.SendMaxtrixControlCode_CallBack = callBack;
                Client.SendMaxtrixControlCodeAsync(conferenceID, conferenceMatrixThrowCode);
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
        /// 发送投影操作控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_SendMaxtrixControlCodeCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if(e.Error == null)
                {
                    this.SendMaxtrixControlCode_CallBack(true);
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

        #region 进入到一个座位

        /// <summary>
        /// 进入到一个座位
        /// </summary>
        public void IntoOneSeat(int conferenceID, string seatList,string selfUri, string selfName, string selfIP, Action<bool, List<SeatEntity>> callBack)
        {
            try
            {
                //回调绑定
                this.IntoOneSeatSync_CallBack = callBack;
                //进入一个座位
                Client.InToOneSeatAsync(conferenceID, seatList,selfUri, selfName, selfIP);
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
        /// 进入一个座位完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_InToOneSeatCompleted(object sender, InToOneSeatCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    //回调
                    this.IntoOneSeatSync_CallBack(true, e.Result.ToList<SeatEntity>());
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

        #region 离开座位

        /// <summary>
        /// 离开座位
        /// </summary>
        public void LeaveOneSeat(int conferenceID, string selfName, string selfIP, Action<bool> callBack)
        {
            try
            {
                //回调绑定
                this.LeaveOneSeatSync_CallBack = callBack;
                //离开座位事件
                Client.LeaveOneSeatAsync(conferenceID, selfName, selfIP);
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
        /// //离开座位完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_LeaveOneSeatCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    this.LeaveOneSeatSync_CallBack(true);
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
