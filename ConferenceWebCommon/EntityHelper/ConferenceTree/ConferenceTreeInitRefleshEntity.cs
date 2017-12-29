using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.ConferenceTree
{
    [Serializable]
    public class ConferenceTreeInitRefleshEntity
    {

        List<ConferenceTreeItemTransferEntity> academicReviewItemTransferEntity_ObserList = null;
        /// <summary>
        /// 知识树实体集合
        /// </summary>
        public List<ConferenceTreeItemTransferEntity> AcademicReviewItemTransferEntity_ObserList
        {
            get
            {
                if (this.academicReviewItemTransferEntity_ObserList == null)
                {
                    this.academicReviewItemTransferEntity_ObserList = new List<ConferenceTreeItemTransferEntity>();
                }
                return academicReviewItemTransferEntity_ObserList;
            }
            set { academicReviewItemTransferEntity_ObserList = value; }
        }


        int rootCount =1;
        /// <summary>
        /// 最终执行到的序列号
        /// </summary>
        public int RootCount
        {
            get { return rootCount; }
            set { rootCount = value; }
        }


        ConferenceTreeItemTransferEntity rootParent_AcademicReviewItemTransferEntity;
        /// <summary>
        /// 知识树根目录
        /// </summary>
        public ConferenceTreeItemTransferEntity RootParent_AcademicReviewItemTransferEntity
        {
            get
            {
                if (rootParent_AcademicReviewItemTransferEntity == null)
                {
                    rootParent_AcademicReviewItemTransferEntity = new ConferenceTreeItemTransferEntity();
                }
                return rootParent_AcademicReviewItemTransferEntity;

            }
            set { rootParent_AcademicReviewItemTransferEntity = value; }
        }

        string summarize;
        /// <summary>
        /// 综述
        /// </summary>
        public string Summarize
        {
            get { return summarize; }
            set { summarize = value; }
        }
    }
}
