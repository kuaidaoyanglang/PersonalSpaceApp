using ConferenceWebCommon.EntityHelper.ConferenceDiscuss;
using ConferenceWebCommon.EntityHelper.ConferenceInfo;
using ConferenceWebCommon.EntityHelper.ConferenceMatrix;
using ConferenceWebCommon.EntityHelper.ConferenceOffice;
using ConferenceWebCommon.EntityHelper.ConferenceTree;
using ConferenceWebCommon.EntityHelper.FileSyncAppPool;
using ConferenceWebCommon.EntityHelper.LyncConversation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityCommon
{
    [Serializable]
    public class PackageBase
    {
        #region old solution

        //ConferenceTreeItemTransferEntity conferenceTreeItemTransferEntity = null;
        ///// <summary>
        ///// 研讨树同步节点
        ///// </summary>
        //public ConferenceTreeItemTransferEntity ConferenceTreeItemTransferEntity
        //{
        //    get { return conferenceTreeItemTransferEntity; }
        //    set { conferenceTreeItemTransferEntity = value; }
        //}

        //ConferenceAudioItemTransferEntity conferenceAudioItemTransferEntity = null;
        ///// <summary>
        ///// 研讨语音节点
        ///// </summary>
        //public ConferenceAudioItemTransferEntity ConferenceAudioItemTransferEntity
        //{
        //    get { return conferenceAudioItemTransferEntity; }
        //    set { conferenceAudioItemTransferEntity = value; }
        //}

        //FileSyncAppEntity fileSyncAppEntity = null;
        ///// <summary>
        ///// 刷屏节点
        ///// </summary>
        //public FileSyncAppEntity FileSyncAppEntity
        //{
        //    get { return fileSyncAppEntity; }
        //    set { fileSyncAppEntity = value; }
        //}

        #endregion

        ConferenceClientAcceptType conferenceClientAcceptType;
        /// <summary>
        /// 类型（研讨树，研讨语音，甩屏）
        /// </summary>
        public ConferenceClientAcceptType ConferenceClientAcceptType
        {
            get { return conferenceClientAcceptType; }
            set { conferenceClientAcceptType = value; }
        }


        ConferenceAudioItemTransferEntity conferenceAudioItemTransferEntity;
        //语音映射实体
        public ConferenceAudioItemTransferEntity ConferenceAudioItemTransferEntity
        {
            get { return conferenceAudioItemTransferEntity; }
            set { conferenceAudioItemTransferEntity = value; }
        }

        ConferenceTreeFlg conferenceTreeFlg;
        //知识树映射实体
        public ConferenceTreeFlg ConferenceTreeFlg
        {
            get { return conferenceTreeFlg; }
            set { conferenceTreeFlg = value; }
        }

        ConferenceInfoFlg conferenceInfoFlg = new ConferenceInfoFlg();
        //刷屏映射实体
        public ConferenceInfoFlg ConferenceInfoFlg
        {
            get { return conferenceInfoFlg; }
            set { conferenceInfoFlg = value; }
        }

        ConferenceSpaceAsyncEntity conferenceSpaceAsyncEntity;
        //智存空间文件共享映射实体
        public ConferenceSpaceAsyncEntity ConferenceSpaceAsyncEntity
        {
            get { return conferenceSpaceAsyncEntity; }
            set { conferenceSpaceAsyncEntity = value; }
        }

        LyncConversationFlg lyncConversationFlg = new LyncConversationFlg();
        //lync会话同步
        public LyncConversationFlg LyncConversationFlg
        {
            get { return lyncConversationFlg; }
            set { lyncConversationFlg = value; }
        }


        ConferenceMatrixBase conferenceMatrixBase;
        //矩阵应用
        public ConferenceMatrixBase ConferenceMatrixBase
        {
            get { return conferenceMatrixBase; }
            set { conferenceMatrixBase = value; }
        }

        bool isEnd = true;
        /// <summary>
        /// 是否结束
        /// </summary>
        public bool IsEnd
        {
            get { return isEnd; }
            set { isEnd = value; }
        }
    }
}
