using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.ConferenceTree
{
    /// <summary>
    /// 知识树映射实体
    /// </summary>
    [Serializable]
    public class ConferenceTreeItemTransferEntity
    {
        #region 属性

        string title;
        /// <summary>
        /// 显示名称
        /// </summary>
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
            }
        }

        int guid;
        /// <summary>
        /// 每个节点的唯一标示
        /// </summary>
        public int Guid
        {
            get { return guid; }
            set
            {
                guid = value;
            }
        }

        string comment ;
        /// <summary>
        /// 评论
        /// </summary>
        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
            }
        }

        List<string> linkList = new List<string>();
        /// <summary>
        /// 超链接列表
        /// </summary>
        public List<string> LinkList
        {
            get { return linkList; }
            set { linkList = value; }
        }
       
        int parentGuid;
        /// <summary>
        /// 父类GUID标示
        /// </summary>
        public int ParentGuid
        {
            get { return this.parentGuid; }
            set { this.parentGuid = value; }
        }

        ConferenceTreeOperationType operation;
        /// <summary>
        /// 操作类型
        /// </summary>
        public ConferenceTreeOperationType Operation
        {
            get { return operation; }
            set { operation = value; }
        }

        string operationer;
        /// <summary>
        /// 当前操作人员（一般更改时适用）
        /// </summary>
        public string Operationer
        {
            get { return operationer; }
            set { operationer = value; }
        }

        List<string> participantList = new List<string>();
        /// <summary>
        /// 参会人员
        /// </summary>
        public List<string> ParticipantList
        {
            get { return participantList; }
            set { participantList = value; }
        }

      
        string focusAuthor;
        /// <summary>
        /// 焦点占用人
        /// </summary>
        public string FocusAuthor
        {
            get { return focusAuthor; }
            set { focusAuthor = value; }
        }


        #endregion

        #region 重新对象识别

        public override bool Equals(object obj)
        {
            bool result = false;
            ConferenceTreeItemTransferEntity entity = obj as ConferenceTreeItemTransferEntity;
            if(entity.Guid.Equals(this.Guid))
            {
                result = true;
            }
            return result;
        }

        #endregion

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
