using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.ConferenceDiscuss
{
    /// <summary>
    /// 语音映射实体
    /// </summary>
    [Serializable]
    public class ConferenceAudioItemTransferEntity
    {
        int guid;
        /// <summary>
        /// 唯一标示符
        /// </summary>
        public int Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        string messageSendTime;
        /// <summary>
        /// 信息发送时间
        /// </summary>
        public string MessageSendTime
        {
            get { return messageSendTime; }
            set { messageSendTime = value; }
        }

        string messageSendName;
        /// <summary>
        /// 信息发送人
        /// </summary>
        public string MessageSendName
        {
            get { return messageSendName; }
            set { messageSendName = value; }
        }

        //bool isSelfSend;
        ///// <summary>
        ///// 是否为自己发送的信息
        ///// </summary>
        //public bool IsSelfSend
        //{
        //    get { return isSelfSend; }
        //    set { isSelfSend = value; }
        //} 

        string addAuthor;
        /// <summary>
        /// 添加人
        /// </summary>
        public string AddAuthor
        {
            get { return addAuthor; }
            set { addAuthor = value; }
        }

        string audioMessage;
        /// <summary>
        /// 语音信息
        /// </summary>
        public string AudioMessage
        {
            get { return audioMessage; }
            set { audioMessage = value; }
        }

        ConferenceAudioOperationType operation;
        /// <summary>
        /// 语音讨论同步类型
        /// </summary>
        public ConferenceAudioOperationType Operation
        {
            get { return operation; }
            set { operation = value; }
        }
        
        string audioUrl;
        /// <summary>
        /// 语音存储地址
        /// </summary>
        public string AudioUrl
        {
            get { return audioUrl; }
            set { audioUrl = value; }
        }       

        string audioFileName;
        /// <summary>
        /// 音频文件名称
        /// </summary>
        public string AudioFileName
        {
            get { return audioFileName; }
            set { audioFileName = value; }
        }

        string iMMType = "Text";
        /// <summary>
        /// 信息类型
        /// </summary>
        public string IMMType
        {
            get { return iMMType; }
            set { iMMType = value; }
        }

        string personalImg;
        /// <summary>
        /// 个人头像
        /// </summary>
        public string PersonalImg
        {
            get { return personalImg; }
            set { personalImg = value; }
        }

        #region 重新对象识别

        public override bool Equals(object obj)
        {
            bool result = false;
            ConferenceAudioItemTransferEntity entity = obj as ConferenceAudioItemTransferEntity;
            if (entity.Guid.Equals(this.Guid))
            {
                result = true;
            }
            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
