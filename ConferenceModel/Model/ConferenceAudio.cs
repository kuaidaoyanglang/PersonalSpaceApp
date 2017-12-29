using ConferenceModel.LogHelper;
using ConferenceModel.ConferenceAudioWebservice;
//using ConferenceModel.ConferenceAudioWebservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;

namespace ConferenceModel.Model
{
    public class ConferenceAudio
    {
        #region 静态字段

        static ConferenceAudioWebserviceSoapClient client;
        /// <summary>
        /// 启用lync扩展服务（语音）
        /// </summary>
        internal static ConferenceAudioWebserviceSoapClient Client
        {
            get { return client; }
        }

        #endregion

        #region 函数回调

        /// <summary>
        /// 实时同步回调
        /// </summary>
        //Action<ConferenceAudioItemTransferEntity> SynchronizationCallBack = null;

        /// <summary>
        /// 初始化同步回调
        /// </summary>
        Action<ConferenceAudioInitRefleshEntity> GetAllCallBack = null;

        /// <summary>
        /// 添加子节点回调
        /// </summary>
        Action<bool,int> AddItemCallBack = null;

        /// <summary>
        /// 删除子节点回调
        /// </summary>
        Action<bool> DeleteItemCallBack = null;

        /// <summary>
        /// 语音转文字事件回调
        /// </summary>
        Action<bool> SettingAudioTransferCallBack = null;

        /// <summary>
        /// 更新子节点事件回调
        /// </summary>
        Action<bool> UpdateItemCallBack = null;

        /// <summary>
        /// 上传音频文件返回标示事件回调
        /// </summary>
        Action<bool> NotifyAudioFileUploadCompleateCallBack = null;
       
        #endregion

        #region 客户端对象模型初始化

        /// <summary>
        /// 客户端对象模型初始化
        /// </summary>
        internal void ConferenceAudioInit(BasicHttpBinding binding, EndpointAddress endpoint)
        {
            try
            {
                if (client == null)
                {
                    //生成语音转文字
                   client = new ConferenceAudioWebserviceSoapClient(binding, endpoint);
                    //生产凭据
                   Client.ClientCredentials.Windows.AllowedImpersonationLevel =
                        TokenImpersonationLevel.Impersonation;
                    //添加节点完成事件
                   Client.AddOneCompleted += Client_AddOneCompleted;
                    //删除语音子节点完成事件
                   Client.DeleteOneCompleted += ConferenceAudioClient_DeleteOneCompleted;
                    //获取语音子节点完成事件
                   Client.GetAllCompleted += ConferenceAudioClient_GetAllCompleted;
                    //获取语音子节点语音转文字信息完成事件
                   Client.SettingAudioTransferTxtCompleted += client_SettingAudioTransferTxtCompleted;
                    //更新语音子节点完成事件
                   Client.UpdateOneCompleted += client_UpdateOneCompleted;
                    //上传音频文件返回标示
                   Client.NotifyAudioFileUploadCompleateCompleted += Client_NotifyAudioFileUploadCompleateCompleted;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 添加一个语音节点

        /// <summary>
        /// 添加一个语音节点
        /// </summary>
        /// <param name="conferenceName"></param>
        /// <param name="callBack"></param>
        /// <param name="academicReviewItemTransferEntity"></param>
        public void Add(int conferenceID, ConferenceAudioItemTransferEntity academicReviewItemTransferEntity, Action<bool,int> callBack)
        {
            try
            {
                //设置回调
                AddItemCallBack = callBack;
                //异步向服务器添加一个语音节点
               Client.AddOneAsync(conferenceID, academicReviewItemTransferEntity);                
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }     

        void Client_AddOneCompleted(object sender, AddOneCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (AddItemCallBack != null)
                    {
                        //启动回调
                        AddItemCallBack(true, e.Result);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 删除语音节点

        /// <summary>
        /// 删除语音节点
        /// </summary>
        /// <param name="conferenceName"></param>
        /// <param name="academicReviewItemTransferEntity"></param>
        /// <param name="callback"></param>
        public void Delete(int conferenceID, ConferenceAudioItemTransferEntity academicReviewItemTransferEntity, Action<bool> callback)
        {
            try
            {
                //设置回调
                this.DeleteItemCallBack = callback;
                //向服务器提交一个删除节点
               Client.DeleteOneAsync(conferenceID, academicReviewItemTransferEntity);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 删除语音节点完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ConferenceAudioClient_DeleteOneCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this.DeleteItemCallBack != null)
                    {
                        //启动回调
                        this.DeleteItemCallBack(true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 获取所有节点

        /// <summary>
        /// 获取所有节点
        /// </summary>
        /// <param name="conferenceName"></param>
        /// <param name="callback"></param>
        public void GetAll(int conferenceID, Action<ConferenceAudioInitRefleshEntity> callback)
        {
            try
            {
                //设置回调
                GetAllCallBack = callback;
                //向服务器申请获取语音子节点
               Client.GetAllAsync(conferenceID);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 获取所有节点完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ConferenceAudioClient_GetAllCompleted(object sender, GetAllCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (GetAllCallBack != null)
                    {
                        //启动回调
                        GetAllCallBack(e.Result);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 更改语音节点

        /// <summary>
        /// 更改语音节点
        /// </summary>
        public void UpdateItem(int conferenceID, ConferenceAudioItemTransferEntity academicReviewItemTransferEntity, Action<bool> callback)
        {
            try
            {               
                //设置回调
                this.UpdateItemCallBack = callback;
                //更新语音子节点
               client.UpdateOneAsync(conferenceID, academicReviewItemTransferEntity);
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
        /// 更改语音子节点完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_UpdateOneCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    //调用回调
                    this.UpdateItemCallBack(true);
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
      
        #region 获取语音子节点语音转文字信息

        /// <summary>
        /// 获取语音子节点语音转文字信息
        /// </summary>
        public void SettingAudioTransfer(int conferenceID, ConferenceAudioItemTransferEntity academicReviewItemTransferEntity, Action<bool> callback)
        {
            try
            {
                //设置事件回调
                this.SettingAudioTransferCallBack = callback;
                //获取语音子节点语音转文字信息
               client.SettingAudioTransferTxtAsync(conferenceID, academicReviewItemTransferEntity);
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
        /// 获取语音子节点语音转文字信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_SettingAudioTransferTxtCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    //语音转文字事件回调
                    if (this.SettingAudioTransferCallBack != null)
                    {
                        this.SettingAudioTransferCallBack(true);
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

        #region 通知音频文件上传完成

        public void UploadAudioCompleate(int conferenceID, ConferenceAudioItemTransferEntity academicReviewItemTransferEntity, Action<bool> callBack)
        {
              try
            {                 
                this.NotifyAudioFileUploadCompleateCallBack = callBack;
               Client.NotifyAudioFileUploadCompleateAsync(conferenceID, academicReviewItemTransferEntity);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        void Client_NotifyAudioFileUploadCompleateCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
              try
            {
               if(e.Error == null)
               {
                    if(this.NotifyAudioFileUploadCompleateCallBack!= null)
                    {
                        this.NotifyAudioFileUploadCompleateCallBack(true);
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

