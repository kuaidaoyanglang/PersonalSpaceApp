using ConferenceCommon.FileDownAndUp;
using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// DownLoadItem.xaml 的交互逻辑
    /// </summary>
    public partial class DownLoadItem : ListViewItem
    {

        #region 自定义回调

        /// <summary>
        /// 自定义回调
        /// </summary>
        public static Action<DownLoadItem> DownLoad_Delete_Click_CallBack = null;

        #endregion

        #region 注册依赖项属性

        public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(DownLoadItem), new FrameworkPropertyMetadata(string.Empty));
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

        public static readonly DependencyProperty AllSize_MBProperty = DependencyProperty.Register("AllSize_MB", typeof(string), typeof(DownLoadItem), new FrameworkPropertyMetadata(string.Empty));
        public string AllSize_MB
        {
            get
            {
                return (string)this.GetValue(AllSize_MBProperty);
            }
            set
            {
                this.SetValue(AllSize_MBProperty, value);
            }
        }

        public static readonly DependencyProperty AllSizeProperty = DependencyProperty.Register("AllSize", typeof(double), typeof(DownLoadItem), new FrameworkPropertyMetadata((double)100.0));
        public double AllSize
        {
            get
            {
                return (double)this.GetValue(AllSizeProperty);
            }
            set
            {
                this.SetValue(AllSizeProperty, value);
            }
        }
       
        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress", typeof(double), typeof(DownLoadItem), new FrameworkPropertyMetadata((double)0.0));
        public double Progress
        {
            get
            {
                return (double)this.GetValue(ProgressProperty);
            }
            set
            {
                this.SetValue(ProgressProperty, value);
            }
        }

        public static readonly DependencyProperty CompletePercentProperty = DependencyProperty.Register("CompletePercent", typeof(string), typeof(DownLoadItem), new FrameworkPropertyMetadata(string.Empty));
         public string CompletePercent
        {
            get
            {
                return (string)this.GetValue(CompletePercentProperty);
            }
            set
            {
                this.SetValue(CompletePercentProperty, value);
            }
        }
        
        

        #endregion

         #region 自定义事件

         public void Item_Delete_Click()
         {
             try
             {
                 if (DownLoad_Delete_Click_CallBack != null)
                 {
                     DownLoad_Delete_Click_CallBack(this);
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

        #region 属性

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

        WebClientManage webClientHelper;
        /// <summary>
        /// 文件下载
        /// </summary>
        public WebClientManage WebClientHelper
        {
            get { return webClientHelper; }
            set { webClientHelper = value; }
        }

        bool isCompleate_DownLoad = false;
        /// <summary>
        /// 是否下载完成
        /// </summary>
        public bool IsCompleate_DownLoad
        {
            get { return isCompleate_DownLoad; }
            set { isCompleate_DownLoad = value; }
        }

        bool isDownLoading_Now = false;
        /// <summary>
        /// 是否正在下载
        /// </summary>
        public bool IsDownLoading_Now
        {
            get { return isDownLoading_Now; }
            set { isDownLoading_Now = value; }
        }

        long length;
        /// <summary>
        /// 文件长度
        /// </summary>
        public long Length
        {
            get { return length; }
            set { length = value; }
        }

        long recieve_Bytes;
        /// <summary>
        /// 文件接收长度
        /// </summary>
        public long Recieve_Bytes
        {
            get { return recieve_Bytes; }
            set { recieve_Bytes = value; }
        }

        #endregion
       
        #region 构建

        static DownLoadItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DownLoadItem), new FrameworkPropertyMetadata(typeof(DownLoadItem)));
        }

        #endregion

        #region 构造函数

        public DownLoadItem()
        {
            this.DataContext = this;
        }

        #endregion
    }
}
