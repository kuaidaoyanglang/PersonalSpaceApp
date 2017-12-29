using ConferenceCommon.LogHelper;
using PersonalSpaceApp.WPFHelper;
using PersonalSpaceApp.ControlBase;
using PersonalSpaceApp.Control;
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
using PersonalSpaceApp.Common;
using ConferenceModel;
using ConferenceCommon.JsonHelper;
using System.Threading;
using ConferenceCommon.WebHelper;
using ConferenceCommon.TimerHelper;
using vy = System.Windows.Visibility;
using fileType = PersonalSpaceApp.WPFControl.FileType;
using IOPath = System.IO.Path;
using PersonalSpaceApp.WPFControl;
using ConferenceCommon.FileDownAndUp;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace PersonalSpaceApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowBase
    {
        #region 内部字段

        /// <summary>
        /// 上传停止
        /// </summary>
        //bool isStopUpload = false;

        #endregion

        #region 一般属性

        /// <summary>
        /// 空间类型
        /// </summary>
        protected SpaceType SpaceType { get; set; }

        #endregion

        #region 绑定属性

        int upload_CompleateCount = 0;
        /// <summary>
        /// 已完成的上传数量
        /// </summary>
        public int Upload_CompleateCount
        {
            get { return upload_CompleateCount; }
            set
            {
                if (value != upload_CompleateCount)
                {
                    upload_CompleateCount = value;
                    this.OnPropertyChanged("Upload_CompleateCount");
                }
            }
        }

        string upload_Title_Tip = "文件上传";
        /// <summary>
        /// 上传标题提示
        /// </summary>
        public string Upload_Title_Tip
        {
            get { return upload_Title_Tip; }
            set
            {
                if (value != upload_Title_Tip)
                {
                    upload_Title_Tip = value;
                    this.OnPropertyChanged("Upload_Title_Tip");
                }
            }
        }

        int upload_AllCount;
        /// <summary>
        /// 需要上传的总数量
        /// </summary>
        public int Upload_AllCount
        {
            get { return upload_AllCount; }
            set
            {
                if (value != upload_AllCount)
                {
                    upload_AllCount = value;
                    this.OnPropertyChanged("Upload_AllCount");
                }
            }
        }

        bool isAllUploadCompleate = true;
        /// <summary>
        /// 是否全部上传完成
        /// </summary>
        public bool IsAllUploadCompleate
        {
            get { return isAllUploadCompleate; }
            set
            {
                if (value != isAllUploadCompleate)
                {
                    isAllUploadCompleate = value;
                    this.OnPropertyChanged("IsAllUploadCompleate");
                }
            }
        }

        ObservableCollection<FileUploadItem> uploadItemCollection = new ObservableCollection<FileUploadItem>()
        {

        };
        /// <summary>
        /// 上传子项
        /// </summary>
        public ObservableCollection<FileUploadItem> UploadItemCollection
        {
            get { return uploadItemCollection; }
            set { uploadItemCollection = value; }
        }

        FileUploadItem selected_FileUploadItem;
        /// <summary>
        /// 上传子项选择
        /// </summary>
        public FileUploadItem Selected_FileUploadItem
        {
            get { return selected_FileUploadItem; }
            set
            {
                if (value != selected_FileUploadItem)
                {
                    selected_FileUploadItem = value;
                    this.OnPropertyChanged("Selected_FileUploadItem");
                }
            }
        }

        vy upload_Failed_TipVisibility = vy.Collapsed;
        /// <summary>
        /// 上传失败提示
        /// </summary>
        public vy Upload_Failed_TipVisibility
        {
            get { return upload_Failed_TipVisibility; }
            set
            {
                if (value != upload_Failed_TipVisibility)
                {
                    upload_Failed_TipVisibility = value;
                    this.OnPropertyChanged("Upload_Failed_TipVisibility");
                }
            }
        }

        #endregion

        #region 事件回调

        /// 上传所有文件完成回调
        /// </summary>
        internal Action<string, int, string> AllUploadCompleateCallBack = null;

        /// <summary>
        /// 开始上传回调
        /// </summary>
        internal Action BeginUploadCallBack = null;

        #endregion

        #region 添加上传子项

        public void UploadItems_Add(FileUploadItem fileUploadItem)
        {
            try
            {
                this.UploadItemCollection.Add(fileUploadItem);
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

        //int intCC = 0;

        public void Begin_Resource_Upload()
        {
            try
            {
                if (this.BeginUploadCallBack != null)
                {
                    this.BeginUploadCallBack();
                }
                if (this.UploadItemCollection.Count > 0)
                {
                    this.Upload_AllCount = this.UploadItemCollection.Count;
                }
                int i = 0;
                foreach (var item in this.UploadItemCollection)
                {
                    item.Nummber = i;
                    i++;
                }

                this.UploadWindowVisibility = vy.Visible;
                if (IsAllUploadCompleate)
                {
                    //intCC = 0;
                    UploadItem_Helper(this.UploadItemCollection[0]);
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

        public void UploadItem_Helper(FileUploadItem beginItem)
        {
            try
            {

                if (!beginItem.IsCompleate_Upload && !beginItem.IsUploading_Now)
                {
                    TimerJob.StartRun(new Action(() =>
                    {
                        beginItem.ImgStateType = ImgStateType.uploading;
                        beginItem.Upload_State = "进行中";
                    }), 10);
                    beginItem.IsUploading_Now = true;
                    beginItem.CancelVisibility = vy.Collapsed;
                    this.IsAllUploadCompleate = false;


                    new Thread((o) =>
                                    {
                                        TimerJob.StartRun(new Action(() =>
                   {

                       beginItem.Item_Thread = Thread.CurrentThread;
                   }), 10);
                                        this.Resource_Upload(beginItem.FolderID, beginItem.FilePath, new Action(() =>
                                        {
                                            if (!beginItem.Is_Removed)
                                            {
                                                beginItem.Upload_State = "已完成";
                                                beginItem.CancelVisibility = vy.Visible;
                                                beginItem.IsUploading_Now = false;
                                                beginItem.ImgStateType = ImgStateType.successed;

                                                beginItem.IsCompleate_Upload = true;
                                                this.Upload_CompleateCount++;
                                                beginItem.Item_Thread = null;
                                                if (this.Upload_CompleateCount == this.UploadItemCollection.Count)
                                                {
                                                    this.Upload_Compleate_Helper(beginItem);
                                                }
                                                else
                                                {
                                                    int id = beginItem.Nummber + 1;
                                                    if (id < this.UploadItemCollection.Count)
                                                    {
                                                        UploadItem_Helper(this.UploadItemCollection[id]);
                                                    }
                                                }
                                            }
                                        }));
                                    }) { IsBackground = true }.Start();
                }
                else
                {
                    int id = beginItem.Nummber + 1;
                    if (id < this.UploadItemCollection.Count)
                    {
                        UploadItem_Helper(this.UploadItemCollection[id]);
                    }
                }
                //}
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


        void Upload_Compleate_Helper(FileUploadItem item)
        {
            try
            {
                this.Upload_Title_Tip = "上传完成";
                this.IsAllUploadCompleate = true;
                string parameters2 = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.GetCollection, item.FolderID);

                if (AllUploadCompleateCallBack != null)
                {
                    AllUploadCompleateCallBack(parameters2, 0, null);
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

        #region 文件上载

        protected void Resource_Upload(int ItemID, string file_Name, Action compleateCallBack)
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
                    ModelManage.Space_Service.FileUpload_Synchronous(SpaceCodeEnterEntity.UploadFile, parameters,
                                 SpaceCodeEnterEntity.LoginUserName,
                                 SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, documentStream, new Action<Dictionary<string, object>>((dicResult) =>
                                 {
                                     //跨线程（异步委托）
                                     this.Dispatcher.BeginInvoke(new Action(() =>
                                     {
                                         if (compleateCallBack != null)
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

        #region 移除上传

        private void UpLoad_Delete_Click_CallBack(FileUploadItem obj)
        {
            try
            {
                if (this.UploadItemCollection.Contains(obj))
                {
                    //标志为移除
                    obj.Is_Removed = true;


                    int objNumber = obj.Nummber;

                    int lastObjNumber = this.UploadItemCollection[this.UploadItemCollection.Count - 1].Nummber;

                    //移除子项
                    this.UploadItemCollection.Remove(obj);
                    //将所有排序下标降1
                    for (int i = 0; i < this.UploadItemCollection.Count; i++)
                    {
                        this.UploadItemCollection[i].Nummber = i;
                    }

                    if (obj.IsUploading_Now)
                    {
                        //intCC--;

                        if (objNumber == lastObjNumber)
                        {
                            this.Upload_Compleate_Helper(obj);
                        }
                        else
                        {
                            if (obj.Item_Thread != null)
                            {
                                obj.Item_Thread.Interrupt();
                                obj.Item_Thread = null;

                                if (obj.Nummber < this.UploadItemCollection.Count)
                                {
                                    UploadItem_Helper(this.UploadItemCollection[obj.Nummber]);
                                }
                            }
                        }
                    }

                    //总项降1
                    this.Upload_AllCount--;
                    //若为已完成项，则已完成项数量降1
                    if (obj.IsCompleate_Upload)
                    {
                        this.Upload_CompleateCount--;
                    }
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

        #region 显示控制

        /// <summary>
        /// 上传视图窗体弹出、关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void foldingUploadWindow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (SpaceCodeEnterEntity.Upload_View_IsDisplay)
                {
                    Storyboard_FoldingUploadWindow.Begin();
                }
                else
                {
                    Storyboard_ExpandUploadWindow.Begin();
                }

                SpaceCodeEnterEntity.Upload_View_IsDisplay = !SpaceCodeEnterEntity.Upload_View_IsDisplay;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        public void Open_Upload_View()
        {
            try
            {
                Storyboard_ExpandUploadWindow.Begin();
                SpaceCodeEnterEntity.Upload_View_IsDisplay = true;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        public void Close_Upload_View()
        {
            try
            {
                Storyboard_FoldingUploadWindow.Begin();
                SpaceCodeEnterEntity.Upload_View_IsDisplay = false;
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

        #region 按钮点击

        /// <summary>
        /// 完成下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Upload_OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.UploadWindowVisibility = vy.Collapsed;
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
