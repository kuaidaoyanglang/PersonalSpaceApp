using ConferenceCommon.LogHelper;
using ConferenceCommon.TimerHelper;
using PersonalSpaceApp.WPFHelper;
using ConferenceModel;
using Microsoft.Win32;
using PersonalSpaceApp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using vy = System.Windows.Visibility;

namespace PersonalSpaceApp.Window
{
    /// <summary>
    /// UploadWindow.xaml 的交互逻辑
    /// </summary>
    public partial class UploadWindow : WindowOperationBase
    {
        #region 回调函数

        /// <summary>
        /// 上传所有文件完成回调
        /// </summary>
        internal Action<string, int, string> AllUploadCompleateCallBack = null;

        /// <summary>
        /// 开始上传回调
        /// </summary>
        internal Action BeginUploadCallBack = null;

        #endregion

        #region 静态字段

        private static UploadWindow uploadWindow = null;
        /// <summary>
        /// 下载窗体(单例循环模式)
        /// </summary>
        public static UploadWindow _UploadWindow
        {
            get
            {
                if (uploadWindow == null)
                {
                    uploadWindow = new UploadWindow();
                }

                return UploadWindow.uploadWindow;
            }
        }

        #endregion

        #region 内部字段

        /// <summary>
        /// 上传列表
        /// </summary>
        List<UploadEntity> uploadEntityList = new List<UploadEntity>();

        /// <summary>
        /// 文件上传完成数量
        /// </summary>
        int uploadCompleateCount = 0;

        /// <summary>
        /// 上传停止
        /// </summary>
        bool isStopUpload = false;

        #endregion

        #region 静态字段

        /// <summary>
        /// 百分比格式
        /// </summary>
        //static string Percent_Formart = "{0}%";


        /// <summary>
        /// 百分比标示
        /// </summary>
        //static string Percent_flg = "100%";

        #endregion

        #region 绑定属性

        string sourceRoot;
        /// <summary>
        /// 文件名称
        /// </summary>
        public string SourceRoot
        {
            get { return sourceRoot; }
            set
            {
                sourceRoot = value;
                this.OnPropertyChanged("SourceRoot");
            }
        }

        #endregion

        #region 一般属性

        /// <summary>
        /// 空间类型
        /// </summary>
        protected SpaceType SpaceType { get; set; }

        #endregion

        #region 构造函数

        public UploadWindow()
        {
            try
            {
                InitializeComponent();

                this.EventRegedit();

                this.ParaMetersInit();
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

        #region 初始化

        /// <summary>
        /// 初始化
        /// </summary>
        // <param name="fileUri"></param>
        private void ParaMetersInit()
        {
            try
            {
                string dir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                this.SourceRoot = dir;
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

        #region 事件注册区域

        /// <summary>
        /// 事件注册区域
        /// </summary>
        public void EventRegedit()
        {
            try
            {
                this.btnOK.Click += btnOK_Click;
                this.Closing +=UploadWindow_Closing;
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

        #region 开始上传

        void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.BeginUploadCallBack != null)
                {
                    this.BeginUploadCallBack();
                }
                if (this.uploadEntityList.Count > 0)
                {
                    this.btnOK.Visibility = vy.Hidden;
                }

                foreach (var item in this.uploadEntityList)
                {
                    item.UploadState = "进行中";
                    item.UploadVisibility = vy.Visible;
                    new Thread((o) =>
         {
             if (!isStopUpload)
             {
                 this.Resource_Upload(item.FolderID, item.FilePath,new Action(()=>
                     {
                         
                         item.UploadVisibility = vy.Collapsed;
                         item.UploadState = "已完成";

                         uploadCompleateCount++;
                         if (uploadCompleateCount == this.uploadEntityList.Count)
                         {
                             
                             string parameters2 = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.GetCollection, item.FolderID);

                             if (AllUploadCompleateCallBack != null)
                             {
                                 AllUploadCompleateCallBack(parameters2, 0, null);
                             }
                         }
                     }));
                
             }
         }) { IsBackground = true }.Start();
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

        #region 移除上传

        private void ItemDelste_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Object selectedItem = this.uploadDataGrid.SelectedItem;
                if (selectedItem is UploadEntity)
                {
                    UploadEntity uploadEntity = selectedItem as UploadEntity;
                    this.uploadDataGrid.Items.Remove(uploadEntity);
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

        #region 加载上传文件

        public void Items_Add(UploadEntity uploadEntity)
        {
            try
            {
                this.uploadDataGrid.Items.Add(uploadEntity);

                this.uploadEntityList.Add(uploadEntity);
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

        #region 文件上载

        protected void Resource_Upload(int ItemID, string file_Name,Action compleateCallBack)
        {
            try
            {
                //声明一个缓冲区
                byte[] documentStream = null;
                string fileName = System.IO.Path.GetFileName(file_Name);

                using (FileStream strea_M = new FileStream(file_Name, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    //创建一个缓冲区
                    documentStream = new byte[strea_M.Length];
                    //将流写入指定缓冲区
                    strea_M.Read(documentStream, 0, documentStream.Length);

                    Dictionary<string, string> dir = new Dictionary<string, string>() { { SpaceCodeEnterEntity.PFileName, fileName } };
                    string parameters = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.UploadFile, ItemID, dir);
                    ModelManage.Space_Service.FileUpload(SpaceCodeEnterEntity.UploadFile, parameters,
                                 SpaceCodeEnterEntity.LoginUserName,
                                 SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, documentStream, new Action<Dictionary<string, object>>((dicResult) =>
                                 {
                                     //跨线程（异步委托）
                                     this.Dispatcher.BeginInvoke(new Action(() =>
                                     {
                                         if(compleateCallBack!= null)
                                         {
                                             compleateCallBack();
                                         }
                                     }));
                                 }));
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

        #region 窗体关闭

        void UploadWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                uploadWindow = null;               
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
