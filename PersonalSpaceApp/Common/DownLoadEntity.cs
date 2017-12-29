using ConferenceCommon.FileDownAndUp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PersonalSpaceApp.Common
{
    public class DownLoadEntity : INotifyPropertyChanged
    {

        #region 实时更新

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        string fileName;
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set
            {
                fileName = value;
                OnPropertyChanged("FileName");
            }
        }

        string fileUri;
        /// <summary>
        /// 文件地址
        /// </summary>
        public string FileUri
        {
            get { return fileUri; }
            set
            {
                fileUri = value;
            }
        }

        long allSize = 1000;
        /// <summary>
        /// 总大小
        /// </summary>
        public long AllSize
        {
            get { return allSize; }
            set
            {
                if (this.allSize != value)
                {
                    this.allSize = value;
                    this.OnPropertyChanged("AllSize");
                }
            }
        }

        string allSize_MB;
        /// <summary>
        /// 文件大小
        /// </summary>
        public string AllSize_MB
        {
            get { return allSize_MB; }
            set
            {
                if (this.allSize_MB != value)
                {
                    this.allSize_MB = value;
                    this.OnPropertyChanged("AllSize_MB");
                }
            }
        }

        string completePercent;
        /// <summary>
        /// 完成百分比
        /// </summary>
        public string CompletePercent
        {
            get { return completePercent; }
            set
            {
                if (this.completePercent != value)
                {
                    this.completePercent = value;
                    this.OnPropertyChanged("CompletePercent");
                }
            }
        }

        double progress;
        /// <summary>
        /// 进度
        /// </summary>
        public double Progress
        {
            get { return progress; }
            set
            {
                if (this.progress != value)
                {
                    this.progress = value;
                    this.OnPropertyChanged("Progress");
                }
            }
        }

        WebClientManage webClientHelper;
        /// <summary>
        /// 文件下载
        /// </summary>
        public WebClientManage WebClientHelper
        {
            get { return webClientHelper; }
            set { webClientHelper = value; }
        }

        public override bool Equals(object obj)
        {
            bool result = false;
            if (obj is DownLoadEntity)
            {
                DownLoadEntity ov = obj as DownLoadEntity;
                if (ov.FileName.Equals(this.FileName))
                {
                    result = true;
                }
            }
            return result;
        }
    }
}
