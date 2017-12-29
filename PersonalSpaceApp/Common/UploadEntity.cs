using ConferenceCommon.FileDownAndUp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using vy = System.Windows.Visibility;

namespace PersonalSpaceApp.Common
{
    public class UploadEntity : INotifyPropertyChanged
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

        string filePath;
        /// <summary>
        /// 文件名称
        /// </summary>
        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;               
            }
        }

        //byte[] byteContent;
        ///// <summary>
        ///// 文件字节数组
        ///// </summary>
        //public byte[] ByteContent
        //{
        //    get { return byteContent; }
        //    set
        //    {
        //        byteContent = value;
        //    }
        //}

        vy uploadVisibility = vy.Collapsed;
        /// <summary>
        /// 上传提示显示
        /// </summary>
        public vy UploadVisibility
        {
            get { return uploadVisibility; }
            set
            {
                if (this.uploadVisibility != value)
                {
                    this.uploadVisibility = value;
                    this.OnPropertyChanged("UploadVisibility");
                }
            }
        }
        string uploadState = "未开始";
        /// <summary>
        /// 上传状态
        /// </summary>
         public string UploadState
        {
            get { return uploadState; }
            set
            {
                if (this.uploadState != value)
                {
                    this.uploadState = value;
                    this.OnPropertyChanged("UploadState");
                }
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

        int folderID;
        /// <summary>
        /// 文件夹ID号
        /// </summary>
        public int FolderID
        {
            get { return folderID; }
            set { folderID = value; }
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
