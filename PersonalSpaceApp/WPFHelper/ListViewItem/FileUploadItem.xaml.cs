using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PersonalSpaceApp.WPFHelper
{
    /// <summary>
    /// FileUploadItem.xaml 的交互逻辑
    /// </summary>
    public partial class FileUploadItem : ListViewItem
    {
        #region 自定义回调

        /// <summary>
        /// 自定义回调
        /// </summary>
        public static Action<FileUploadItem> UpLoad_Delete_Click_CallBack = null;

        #endregion

        #region 注册依赖项属性

        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(FileUploadItem), new FrameworkPropertyMetadata(string.Empty));
        public string FileName
        {
            get
            {
                return (string)this.GetValue(FileNameProperty);
            }
            set
            {
                this.SetValue(FileNameProperty, value);
            }
        }

        public static readonly DependencyProperty Upload_StateProperty = DependencyProperty.Register("Upload_State", typeof(string), typeof(FileUploadItem), new FrameworkPropertyMetadata("等待上传"));
        public string Upload_State
        {
            get
            {
                return (string)this.GetValue(Upload_StateProperty);
            }
            set
            {
                this.SetValue(Upload_StateProperty, value);
            }
        }

        public static readonly DependencyProperty File_SizeProperty = DependencyProperty.Register("File_Size", typeof(string), typeof(FileUploadItem), new FrameworkPropertyMetadata(string.Empty));
        public string File_Size
        {
            get
            {
                return (string)this.GetValue(File_SizeProperty);
            }
            set
            {
                this.SetValue(File_SizeProperty, value);
            }
        }


        //public static readonly DependencyProperty Real_SourceProperty = DependencyProperty.Register("Real_Source", typeof(ImageSource), typeof(FileUploadItem), new FrameworkPropertyMetadata(null));
        //public ImageSource Real_Source
        //{
        //    get
        //    {
        //        return (ImageSource)this.GetValue(Real_SourceProperty);
        //    }
        //    set
        //    {
        //        this.SetValue(Real_SourceProperty, value);
        //    }
        //}


        public static readonly DependencyProperty ImageSource_WaitProperty = DependencyProperty.Register("ImageSource_Wait", typeof(ImageSource), typeof(FileUploadItem), new FrameworkPropertyMetadata(null));
        public ImageSource ImageSource_Wait
        {
            get
            {
                return (ImageSource)this.GetValue(ImageSource_WaitProperty);
            }
            set
            {
                this.SetValue(ImageSource_WaitProperty, value);
            }
        }

        public static readonly DependencyProperty ImageSource_UploadingProperty = DependencyProperty.Register("ImageSource_Uploading", typeof(ImageSource), typeof(FileUploadItem), new FrameworkPropertyMetadata(null));
        public ImageSource ImageSource_Uploading
        {
            get
            {
                return (ImageSource)this.GetValue(ImageSource_UploadingProperty);
            }
            set
            {
                this.SetValue(ImageSource_UploadingProperty, value);
            }
        }

        public static readonly DependencyProperty ImageSource_Upload_SuccessedProperty = DependencyProperty.Register("ImageSource_Upload_Successed", typeof(ImageSource), typeof(FileUploadItem), new FrameworkPropertyMetadata(null));
        public ImageSource ImageSource_Upload_Successed
        {
            get
            {
                return (ImageSource)this.GetValue(ImageSource_Upload_SuccessedProperty);
            }
            set
            {
                this.SetValue(ImageSource_Upload_SuccessedProperty, value);
            }
        }

        public static readonly DependencyProperty ImageSource_Upload_FailedProperty = DependencyProperty.Register("ImageSource_Upload_Failed", typeof(ImageSource), typeof(FileUploadItem), new FrameworkPropertyMetadata(null));
        public ImageSource ImageSource_Upload_Failed
        {
            get
            {
                return (ImageSource)this.GetValue(ImageSource_Upload_FailedProperty);
            }
            set
            {
                this.SetValue(ImageSource_Upload_FailedProperty, value);
            }
        }
        //public static readonly DependencyProperty Gif_VisibilityProperty = DependencyProperty.Register("Gif_Visibility", typeof(Visibility), typeof(FileUploadItem), new FrameworkPropertyMetadata(Visibility.Visible));
        //public Visibility Gif_Visibility
        //{
        //    get
        //    {
        //        return (Visibility)this.GetValue(Gif_VisibilityProperty);
        //    }
        //    set
        //    {
        //        this.SetValue(Gif_VisibilityProperty, value);
        //    }
        //}

        //public static readonly DependencyProperty IMG_VisibilityProperty = DependencyProperty.Register("IMG_Visibility", typeof(Visibility), typeof(FileUploadItem), new FrameworkPropertyMetadata(Visibility.Collapsed));
        //  public Visibility IMG_Visibility
        //{
        //    get
        //    {
        //        return (Visibility)this.GetValue(IMG_VisibilityProperty);
        //    }
        //    set
        //    {
        //        this.SetValue(IMG_VisibilityProperty, value);
        //    }
        //}

          public static readonly DependencyProperty ImgStateTypeProperty = DependencyProperty.Register("ImgStateType", typeof(ImgStateType), typeof(FileUploadItem), new FrameworkPropertyMetadata(ImgStateType.wait));
          public ImgStateType ImgStateType
        {
            get
            {
                return (ImgStateType)this.GetValue(ImgStateTypeProperty);
            }
            set
            {
                this.SetValue(ImgStateTypeProperty, value);
            }
        }

          public static readonly DependencyProperty NummberProperty = DependencyProperty.Register("Nummber", typeof(int), typeof(FileUploadItem), new FrameworkPropertyMetadata(-1));
          public int Nummber
          {
              get
              {
                  return (int)this.GetValue(NummberProperty);
              }
              set
              {
                  this.SetValue(NummberProperty, value);
              }
          }


          public static readonly DependencyProperty CancelVisibilityProperty = DependencyProperty.Register("CancelVisibility", typeof(Visibility), typeof(FileUploadItem), new FrameworkPropertyMetadata(Visibility.Visible));
          public Visibility CancelVisibility
        {
            get
            {
                return (Visibility)this.GetValue(CancelVisibilityProperty);
            }
            set
            {
                this.SetValue(CancelVisibilityProperty, value);
            }
        }

        

        #endregion

        #region 自定义事件

        public void Item_Delete_Click()
        {
            try
            {
                if (UpLoad_Delete_Click_CallBack != null)
                {
                    UpLoad_Delete_Click_CallBack(this);
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

        #region 一般属性

        int folderID;
        /// <summary>
        /// 文件夹ID号
        /// </summary>
        public int FolderID
        {
            get { return folderID; }
            set { folderID = value; }
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

        long allSize = 1000;
        /// <summary>
        /// 总大小
        /// </summary>
        public long AllSize
        {
            get { return allSize; }
            set
            {
                allSize = value;
            }
        }

        bool isCompleate_Upload = false;
        /// <summary>
        /// 是否上传完成
        /// </summary>
        public bool IsCompleate_Upload
        {
            get { return isCompleate_Upload; }
            set { isCompleate_Upload = value; }
        }

        bool isUploading_Now = false;
        /// <summary>
        /// 是否正在上传
        /// </summary>
        public bool IsUploading_Now
        {
            get { return isUploading_Now; }
            set { isUploading_Now = value; }
        }

        Thread item_Thread;
        /// <summary>
        /// 子项线程
        /// </summary>
        public Thread Item_Thread
        {
            get { return item_Thread; }
            set { item_Thread = value; }
        }

        bool is_Removed = false;
        /// <summary>
        /// 是否已移除
        /// </summary>
        public bool Is_Removed
        {
            get { return is_Removed; }
            set { is_Removed = value; }
        }

        #endregion


        #region 构建

        static FileUploadItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileUploadItem), new FrameworkPropertyMetadata(typeof(FileUploadItem)));
        }

        #endregion

        public FileUploadItem()
        {
            this.DataContext = this;
        }


    }

    public enum ImgStateType
    {
        wait,
        uploading,
        successed,
        failed
    }
}
