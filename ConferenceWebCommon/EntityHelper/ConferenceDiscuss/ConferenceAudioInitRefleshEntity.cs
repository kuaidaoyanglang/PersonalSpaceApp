using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConferenceWebCommon.EntityHelper.ConferenceDiscuss
{
     [Serializable]
    public class ConferenceAudioInitRefleshEntity
    {

        List<ConferenceAudioItemTransferEntity> academicReviewItemTransferEntity_ObserList = null;
        /// <summary>
        /// 语音节点实体集合
        /// </summary>
        public List<ConferenceAudioItemTransferEntity> AcademicReviewItemTransferEntity_ObserList
        {
            get
            {
                if (this.academicReviewItemTransferEntity_ObserList == null)
                {
                    this.academicReviewItemTransferEntity_ObserList = new List<ConferenceAudioItemTransferEntity>();
                }
                return academicReviewItemTransferEntity_ObserList;
            }
            set { academicReviewItemTransferEntity_ObserList = value; }
        }

        int rootCount;
        /// <summary>
        /// 最终执行到的序列号
        /// </summary>
        public  int RootCount
        {
            get { return rootCount; }
            set { rootCount = value; }
        }
    }
}
