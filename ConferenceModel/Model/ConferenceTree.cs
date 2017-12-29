using ConferenceModel.LogHelper;
using ConferenceModel.ConferenceTreeWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;

namespace ConferenceModel.Model
{
    public class ConferenceTree
    {
        #region 静态字段

        static ConferenceTreeWebServiceSoapClient client;
        /// <summary>
        /// 启用lync扩展服务（树）
        /// </summary>
        internal static ConferenceTreeWebServiceSoapClient Client
        {
            get { return client; }
        }

        #endregion

        #region 字段

        /// <summary>
        /// 实时同步回调
        /// </summary>
        //Action<ConferenceTreeItemTransferEntity> SynchronizationCallBack = null;

        /// <summary>
        /// 初始化同步回调
        /// </summary>
        Action<ConferenceTreeInitRefleshEntity> GetAllCallBack = null;

        /// <summary>
        /// 添加子节点回调
        /// </summary>
        Action<bool> AddItemCallBack = null;

        /// <summary>
        /// 更改子节点回调
        /// </summary>
        Action<string> UpdateItemCallBack = null;

        /// <summary>
        /// 替换子节点回调
        /// </summary>
        Action<bool> InsteadElementCallBack = null;

        /// <summary>
        /// 更新整个树（服务器里的）
        /// </summary>
        Action<bool> SetAll_CallBack = null;

        /// <summary>
        /// 检查投票人回调
        /// </summary>
        Action<bool, bool> CheckVoteList_CallBack = null;

        /// <summary>
        /// 投票回调
        /// </summary>
        Action<bool> Vote_CallBack = null;

        /// <summary>
        /// 清除节点所有投票回调
        /// </summary>
        Action<bool> ClearItemAllVote_CallBack = null;

        /// <summary>
        /// 强制占用焦点事件回调
        /// </summary>
        Action<bool> FocusCallBack = null;

        /// <summary>
        /// 更改子节点标题回调
        /// </summary>
        Action<string> UpdateItemTitleCallBack = null;

        /// <summary>
        /// 更改子节点评论回调
        /// </summary>
        Action<string> UpdateItemCommentCallBack = null;

        /// <summary>
        /// 更改子节点链接回调
        /// </summary>
        Action<string> UpdateItemLinkCallBack = null;

        #endregion

        #region 对象初始化

        /// <summary>
        /// 客户端对象模型初始化
        /// </summary>
        internal void ConferenceTreeInit(BasicHttpBinding binding, EndpointAddress endpoint)
        {
            try
            {
                if (client == null)
                {
                    client = new ConferenceTreeWebServiceSoapClient(binding, endpoint);

                    client.ClientCredentials.Windows.AllowedImpersonationLevel =
                     TokenImpersonationLevel.Impersonation;

                    //提交一个节点（更新）完成事件
                    Client.UpdateOneCompleted += client_UpdateOneCompleted;
                    //删除一个子节点完成事件
                    Client.DeleteOneCompleted += client_DeleteOneCompleted;
                    //添加一个子节点完成事件
                    Client.AddOneCompleted += client_AddOneCompleted;
                    //获取知识树完成事件
                    Client.GetAllCompleted += client_GetAllCompleted;

                    //设置整个知识树完成事件
                    Client.SetAllCompleted += ConferenceTreeClient_SetAllCompleted;
                    //查看投票完成事件
                    Client.CheckVoteListContainsSelfCompleted += ConferenceTreeClient_CheckVoteListContainsSelfCompleted;
                    //知识树子节点投票更改完成事件
                    Client.VoteChangedCompleted += ConferenceTreeClient_VoteChangedCompleted;
                    //清除节点投票完成事件
                    Client.ClearItemAllVoteCompleted += ConferenceTreeClient_ClearItemAllVoteCompleted;

                    //强行占用焦点完成事件
                    Client.ForceOccuptFocusCompleted += Client_ForceOccuptFocusCompleted;
                    //更改标题事件
                    Client.UpdateTittleCompleted += Client_UpdateTittleCompleted;
                    //更改评论事件
                    Client.UpdateCommentCompleted += Client_UpdateCommentCompleted;
                    //添加链接事件
                    Client.LinkAddCompleted += Client_LinkAddCompleted;
                    //替换节点完成事件
                    Client.InsteadCompleted += Client_InsteadCompleted;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 初始化同步

        /// <summary>
        /// 初始化同步
        /// </summary>
        /// <param name="conferenceName"></param>
        /// <param name="callback"></param>
        public void GetAll(int conferenceID, Action<ConferenceTreeInitRefleshEntity> callback)
        {
            try
            {
                this.GetAllCallBack = callback;
                Client.GetAllAsync(conferenceID);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 初始化同步完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_GetAllCompleted(object sender, GetAllCompletedEventArgs e)
        {

            try
            {
                if (e.Error == null)
                {
                    if (this.GetAllCallBack != null)
                    {
                        this.GetAllCallBack(e.Result);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 更改一个节点（更新）

        /// <summary>
        /// 提交一个节点（更新）完成事件
        /// </summary>
        /// <param name="conferenceName"></param>
        /// <param name="academicReviewItemTransferEntity"></param>
        /// <param name="callback"></param>
        public void Update(int conferenceID, ConferenceTreeItemTransferEntity academicReviewItemTransferEntity, Action<string> callback)
        {
            try
            {
                UpdateItemCallBack = callback;
                Client.UpdateOneAsync(conferenceID, academicReviewItemTransferEntity);

            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 提交一个节点（更新）完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_UpdateOneCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (UpdateItemCallBack != null)
                    {
                        UpdateItemCallBack(null);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 替换节点

        /// <summary>
        /// 替换节点
        /// </summary>
        public void InsteadElement(int conferenceID, int beforeItemGuid, int nowItemGuid, Action<bool> callback)
        {
            try
            {
                InsteadElementCallBack = callback;
                Client.InsteadAsync(conferenceID, beforeItemGuid, nowItemGuid);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 替换节点完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_InsteadCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    InsteadElementCallBack(true);
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 强行占用焦点

        /// <summary>
        /// 强行占用焦点
        /// </summary>
        public void ForceOccupyFocus(int conferenceID, ConferenceTreeItemTransferEntity academicReviewItemTransferEntity, Action<bool> callback)
        {
            try
            {
                //强制占用焦点事件回调
                this.FocusCallBack = callback;
                //强行占用焦点
                Client.ForceOccuptFocusAsync(conferenceID, academicReviewItemTransferEntity);
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
        /// 强行占用焦点完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Client_ForceOccuptFocusCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    this.FocusCallBack(true);
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

        #region 投票更改

        /// <summary>
        /// 投票更改
        /// </summary>
        /// <param name="conferenceName"></param>
        /// <param name="academicReviewItemTransferEntity"></param>
        /// <param name="participant"></param>
        /// <param name="callback"></param>
        public void Vote(int conferenceID, ConferenceTreeItemTransferEntity academicReviewItemTransferEntity, string participant, Action<bool> callback)
        {
            try
            {
                this.Vote_CallBack = callback;
                Client.VoteChangedAsync(conferenceID, academicReviewItemTransferEntity, participant);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 投票更改完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ConferenceTreeClient_VoteChangedCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this.Vote_CallBack != null)
                    {
                        this.Vote_CallBack(true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 清除节点所有投票

        /// <summary>
        /// 清除节点所有投票
        /// </summary>
        /// <param name="conferenceName"></param>
        /// <param name="conferenceTreeItemTransferEntity"></param>
        /// <param name="callBack"></param>
        public void ClearItemAllVote(int conferenceID, ConferenceTreeItemTransferEntity conferenceTreeItemTransferEntity, Action<bool> callBack)
        {
            try
            {
                this.ClearItemAllVote_CallBack = callBack;
                Client.ClearItemAllVoteAsync(conferenceID, conferenceTreeItemTransferEntity);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 清除节点所有投票完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ConferenceTreeClient_ClearItemAllVoteCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this.ClearItemAllVote_CallBack != null)
                    {
                        this.ClearItemAllVote_CallBack(true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 删除一个节点

        /// <summary>
        /// 删除一个节点
        /// </summary>
        /// <param name="conferenceName"></param>
        /// <param name="academicReviewItemTransferEntity"></param>
        public void Delete(int conferenceID, ConferenceTreeItemTransferEntity academicReviewItemTransferEntity)
        {
            try
            {
                Client.DeleteOneAsync(conferenceID, academicReviewItemTransferEntity);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 删除一个节点完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void client_DeleteOneCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {

        }

        #endregion

        #region 添加一个节点

        /// <summary>
        /// 添加一个节点
        /// </summary>
        /// <param name="conferenceName"></param>
        /// <param name="academicReviewItemTransferEntity"></param>
        /// <param name="callBack"></param>
        public void Add(int conferenceID, ConferenceTreeItemTransferEntity academicReviewItemTransferEntity, Action<bool> callBack)
        {
            try
            {
                AddItemCallBack = callBack;
                Client.AddOneAsync(conferenceID, academicReviewItemTransferEntity);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 添加一个节点完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_AddOneCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (AddItemCallBack != null)
                    {
                        AddItemCallBack(true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 重新构造服务器中的研讨树

        /// <summary>
        /// 重新构造服务器中的研讨树
        /// </summary>
        /// <param name="conferenceName"></param>
        /// <param name="conferenceTreeInitRefleshEntity"></param>
        /// <param name="callback"></param>
        public void SetAll(int conferenceID, ConferenceTreeInitRefleshEntity conferenceTreeInitRefleshEntity, Action<bool> callback)
        {
            try
            {
                this.SetAll_CallBack = callback;
                Client.SetAllAsync(conferenceID, conferenceTreeInitRefleshEntity);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 重新构造服务器中的研讨树完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ConferenceTreeClient_SetAllCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this.SetAll_CallBack != null)
                    {
                        this.SetAll_CallBack(true);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 查看当前节点自己是否投过票

        /// <summary>
        /// 查看当前节点自己是否投过票
        /// </summary>
        /// <param name="conferenceName"></param>
        /// <param name="academicReviewItemTransferEntity"></param>
        /// <param name="Participant"></param>
        public void CheckVoteList(int conferenceID, ConferenceTreeItemTransferEntity academicReviewItemTransferEntity, string Participant, Action<bool, bool> callback)
        {
            try
            {
                this.CheckVoteList_CallBack = callback;
                //Client.CheckVoteListContainsSelfAsync(conferenceID, academicReviewItemTransferEntity, Participant);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 查看当前节点自己是否投过票完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ConferenceTreeClient_CheckVoteListContainsSelfCompleted(object sender, CheckVoteListContainsSelfCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (this.CheckVoteList_CallBack != null)
                    {
                        this.CheckVoteList_CallBack(true, e.Result);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 添加链接事件

        public void LinkAdd(int conferenceID, ConferenceTreeItemTransferEntity academicReviewItemTransferEntity, Action<string> callback)
        {
            try
            {
                UpdateItemLinkCallBack = callback;
                Client.LinkAddAsync(conferenceID, academicReviewItemTransferEntity);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        void Client_LinkAddCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (UpdateItemLinkCallBack != null)
                    {
                        UpdateItemLinkCallBack(null);
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

        #region 更改评论事件

        public void UpdateComment(int conferenceID, ConferenceTreeItemTransferEntity academicReviewItemTransferEntity, Action<string> callback)
        {
            try
            {
                UpdateItemCommentCallBack = callback;
                Client.UpdateCommentAsync(conferenceID, academicReviewItemTransferEntity);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        void Client_UpdateCommentCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (UpdateItemCommentCallBack != null)
                    {
                        UpdateItemCommentCallBack(null);
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

        #region 更改标题事件

        public void UpdateTitle(int conferenceID, ConferenceTreeItemTransferEntity academicReviewItemTransferEntity, Action<string> callback)
        {
            try
            {
                UpdateItemTitleCallBack = callback;
                Client.UpdateTittleAsync(conferenceID, academicReviewItemTransferEntity);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        void Client_UpdateTittleCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null)
                {
                    if (UpdateItemTitleCallBack != null)
                    {
                        UpdateItemTitleCallBack(null);
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
