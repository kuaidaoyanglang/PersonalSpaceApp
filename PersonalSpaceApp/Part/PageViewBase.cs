using ConferenceCommon.JsonHelper;
using ConferenceCommon.LogHelper;
using PersonalSpaceApp.WPFControl;
using ConferenceModel;
using PersonalSpaceApp.Common;
using PersonalSpaceApp.Control;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using vy = System.Windows.Visibility;
using fileType = PersonalSpaceApp.WPFControl.FileType;
using IOPath = System.IO.Path;
using System.Threading;
using ConferenceCommon.FileDownAndUp;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using viewType = PersonalSpaceApp.WPFHelper.ViewType;
using PersonalSpaceApp.WPFHelper;
using System.ComponentModel;
using PersonalSpaceApp.Window;
using ConferenceCommon.FileHelper;

namespace PersonalSpaceApp.Part
{
    public class PageViewBase : UserControl, INotifyPropertyChanged
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

        #region 自定义事件

        /// <summary>
        /// 控制面板操作回调
        /// </summary>
        protected Action<SpaceListViewItem, CallBackType> OperationCallBack = null;

        #endregion

        #region 静态字段

        /// <summary>
        /// 参数
        /// </summary>
        public const long buffer = 1024;

        /// <summary>
        /// 参数1
        /// </summary>
        public const long buffer1 = 1024 * 1024;

        /// <summary>
        /// 参数2
        /// </summary>
        public const long buffer2 = 1024 * 1024 / 10;

        /// <summary>
        /// kb标示
        /// </summary>
        public const string KB_Flg = "0.00 KB";

        /// <summary>
        /// MB标示
        /// </summary>
        public const string MB_Flg = "0.00 MB";

        /// <summary>
        /// 文件标示
        /// </summary>
        public const string File_Flg = "文档";

        /// <summary>
        /// 文件夹标示
        /// </summary>
        public const string Folder_Flg = "文件夹";

        /// <summary>
        /// 根参数
        /// </summary>
        public static string dicParameters_Root = null;

        /// <summary>
        /// 根ID
        /// </summary>
        public static int id_Root = 0;

        /// <summary>
        /// 根名称
        /// </summary>
        public static string ParentLeafName_Root = null;

        /// <summary>
        /// 根ID
        /// </summary>
        public static string id_Root_str = null;

        #endregion

        #region 内部字段

        /// <summary>
        /// 空间类型
        /// </summary>
        protected SpaceType SpaceType { get; set; }

        /// <summary>
        /// 当前目录(自定义)
        /// </summary>
        protected BreadLine currentBreadLine = null;

        /// <summary>
        /// 子项集合
        /// </summary>
        public List<SpaceListViewItem> SpaceListViewItemList = new List<SpaceListViewItem>();

        /// <summary>
        /// 面包线
        /// </summary>
        protected BreadLine Bread_LineRoot = null;

        /// <summary>
        /// 视图类型
        /// </summary>
        public static ViewType ViewType = ViewType.ListView;

        /// <summary>
        /// 文件名标示
        /// </summary>
        public static string Defalut_FileTitlFlg = "文件名";

        #endregion

        #region 绑定属性

        vy displayPanelVisibility = vy.Visible;
        /// <summary>
        /// 列标题面板显示
        /// </summary>
        public vy DisplayPanelVisibility
        {
            get { return displayPanelVisibility; }
            set
            {
                if (this.displayPanelVisibility != value)
                {
                    this.displayPanelVisibility = value;
                    this.OnPropertyChanged("DisplayPanelVisibility");
                }
            }
        }

        string firstColumnTitle = Defalut_FileTitlFlg;

        public string FirstColumnTitle
        {
            get { return firstColumnTitle; }
            set
            {
                if (this.firstColumnTitle != value)
                {
                    this.firstColumnTitle = value;
                    this.OnPropertyChanged("FirstColumnTitle");
                }
            }
        }

        #endregion

        #region 资源

        ObservableCollection<SpaceListViewItem> listViewCollection = new ObservableCollection<SpaceListViewItem>();
        /// <summary>
        /// listview资源
        /// </summary>
        public ObservableCollection<SpaceListViewItem> ListViewCollection
        {
            get { return listViewCollection; }
            set { listViewCollection = value; }
        }

        #endregion

        #region 构造函数

        public PageViewBase()
        {
            try
            {
                this.DataContext = this;
                this.SpaceListViewItemList.Clear();
                SpaceCodeEnterEntity.fileOpenManage = new FileOpenManage(SpaceCodeEnterEntity.WebLoginUserName, SpaceCodeEnterEntity.WebLoginPassword,
           SpaceCodeEnterEntity.LocalTemp, SpaceCodeEnterEntity.LoginUserName, SpaceCodeEnterEntity.UserDomain, false);
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

        #region 打开文件（包括文件夹）

        /// <summary>
        /// 打开文件（包括文件夹）
        /// </summary>
        /// <param name="spaceListViewItem"></param>
        protected void book_OpenFileEvent(SpaceListViewItem spaceListViewItem, Action<string, int, string> compleateCallBack)
        {
            try
            {
                switch (spaceListViewItem.BookType)
                {
                    case BookType.File:

                        if (spaceListViewItem.Self_File != null)
                        {
                            if (string.IsNullOrEmpty(spaceListViewItem.Uri))
                            {
                                string parameters = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.DisplayItem, spaceListViewItem.Self_File.ID);
                                ModelManage.Space_Service.Function_Invoke(SpaceCodeEnterEntity.DisplayItem, parameters, SpaceCodeEnterEntity.LoginUserName,
                                    SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, new Action<Dictionary<string, object>>((dicResult) =>
                                    {
                                        SPItemEntity sPItemEntity = JsonManage.DictionaryToEntity<SPItemEntity>(dicResult, ',');
                                        string uri = SpaceCodeEnterEntity.SPSiteAddressFront + sPItemEntity.FileRef;
                                        //跨线程（异步委托）
                                        this.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            spaceListViewItem.Self_File.ServerRelativeUrl = uri;
                                            spaceListViewItem.Uri = uri;
                                            //根据文件的类型使用相应的方式打开
                                            SpaceCodeEnterEntity.fileOpenManage.FileOpenByExtension((fileType)spaceListViewItem.FileType, spaceListViewItem.Uri);
                                        }));
                                    }));
                            }
                            else
                            {
                                //根据文件的类型使用相应的方式打开
                                SpaceCodeEnterEntity.fileOpenManage.FileOpenByExtension((fileType)spaceListViewItem.FileType, spaceListViewItem.Uri);
                            }
                        }
                        break;
                    case BookType.Folder:
                        //打开文件夹
                        this.FolderOpen(spaceListViewItem, compleateCallBack);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }


        /// <summary>
        /// 打开文件夹
        /// </summary>
        protected void FolderOpen(SpaceListViewItem spaceListViewItem, Action<string, int, string> compleateCallBack)
        {
            try
            {

                if (this.currentBreadLine != null && spaceListViewItem.BreadLine != null)
                {
                    ////设置当前面包线的子节点（子面包线）
                    //this.currentBreadLine.BreadLineChild = null;

                    BreadLine line = spaceListViewItem.BreadLine;

                    //DependencyObject line_Parent = line.Parent;
                    //if (line_Parent != null && line_Parent is BreadLine)
                    //{
                    //    BreadLine lineParent = line_Parent as BreadLine;
                    //    lineParent.BreadLineChild = null;
                    //}

                    if (this.currentBreadLine != null)
                    {
                        //设置当前面包线的子节点（子面包线）
                        //this.Bread_LineRoot.BreadLineChild = line;

                        this.Bread_LineRoot.SettingLastLine(line);

                        //设置当前面包线
                        this.currentBreadLine = line;

                        if (this.currentBreadLine.Folder != null && this.currentBreadLine.Folder.ID > 0)
                        {
                            int strItemID = this.currentBreadLine.Folder.ID;
                            string parameters = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.GetCollection, strItemID);

                            if (compleateCallBack != null)
                            {
                                compleateCallBack(parameters, 0, null);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 返回加载视图

        /// <summary>
        /// 返回加载视图
        /// </summary>
        /// <param name="element">返回的元素</param>
        protected void LoadUICallBack(Watch_ViewBase element)
        {
            try
            {
                ContentWindow window = new ContentWindow();
                window.ContentAdd(element);
                window.Title = element.Title;
                window.Window_Show();
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

        #region 新建文件夹

        protected void Resource_CreateFolder(Action beginCallBack, Action<string, int, string> compleateCallBack)
        {
            try
            {
                if (this.currentBreadLine != null && this.currentBreadLine.Folder != null)
                {
                    //文本输入窗体                  
                    InputMessageWindow._InputMessageWindow.Window_Show();
                    InputMessageWindow._InputMessageWindow.OkEventCallBack = new Action<string>((folderName) =>
                    {
                        ////等待提示
                        //this.ShowTip();
                        if (beginCallBack != null)
                        {
                            beginCallBack();
                        }
                        //ThreadPool.QueueUserWorkItem((o) =>
                        //{
                        string parameters = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.CreateFolder, this.currentBreadLine.Folder.ID, folderName);
                        ModelManage.Space_Service.Function_Invoke(SpaceCodeEnterEntity.CreateFolder, parameters,
                            SpaceCodeEnterEntity.LoginUserName,
                            SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, new Action<Dictionary<string, object>>((dicResult) =>
                            {
                                //跨线程（异步委托）
                                this.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    if (this.currentBreadLine.Folder != null && this.currentBreadLine.Folder.ID > 0)
                                    {
                                        int strItemID = this.currentBreadLine.Folder.ID;
                                        string parameters2 = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.GetCollection, strItemID);

                                        if (compleateCallBack != null)
                                        {
                                            compleateCallBack(parameters2, 0, null);
                                        }
                                    }
                                }));
                            }));
                        //});
                    });

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

        #region 文件删除

        protected void Resource_Delete(Action beginCallBack, Action<string, int, string> compleateCallBack)
        {
            try
            {
                List<int> idList = new List<int>();

                foreach (var item in this.SpaceListViewItemList)
                {

                    switch (item.BookType)
                    {
                        case BookType.File:
                            if (item.Self_File != null)
                            {
                                idList.Add(item.Self_File.ID);
                            }
                            break;
                        case BookType.Folder:
                            if (item.Self_Folder != null)
                            {
                                idList.Add(item.Self_Folder.ID);
                            }
                            break;
                    }
                }
                if (idList.Count > 0)
                {
                    if (beginCallBack != null)
                    {
                        beginCallBack();
                    }

                    string parameters = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.DeleteFile, idList);
                    ModelManage.Space_Service.Function_Invoke(SpaceCodeEnterEntity.DeleteFile, parameters,
                        SpaceCodeEnterEntity.LoginUserName,
                        SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, new Action<Dictionary<string, object>>((dicResult) =>
                        {
                            //跨线程（异步委托）
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (this.currentBreadLine != null && this.currentBreadLine.Folder != null && this.currentBreadLine.Folder.ID > 0)
                                {
                                    int strItemID = this.currentBreadLine.Folder.ID;
                                    string parameters2 = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.GetCollection, strItemID);

                                    if (compleateCallBack != null)
                                    {
                                        compleateCallBack(parameters2, 0, null);
                                    }
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

        protected void Resource_Delete(SpaceListViewItem item, Action beginCallBack, Action<string, int, string> compleateCallBack)
        {
            try
            {
                int intItemID = 0;


                switch (item.BookType)
                {
                    case BookType.File:
                        if (item.Self_File != null)
                        {
                            intItemID = item.Self_File.ID;
                        }
                        break;
                    case BookType.Folder:
                        if (item.Self_Folder != null)
                        {
                            intItemID = item.Self_Folder.ID;
                        }
                        break;
                }

                if (beginCallBack != null)
                {
                    beginCallBack();
                }
                if (intItemID > 0)
                {
                    string parameters = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.DeleteFile, intItemID);
                    ModelManage.Space_Service.Function_Invoke(SpaceCodeEnterEntity.DeleteFile, parameters,
                        SpaceCodeEnterEntity.LoginUserName,
                        SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, new Action<Dictionary<string, object>>((dicResult) =>
                        {
                            //跨线程（异步委托）
                            this.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                if (this.currentBreadLine.Folder != null && this.currentBreadLine.Folder.ID > 0)
                                {
                                    int strItemID = this.currentBreadLine.Folder.ID;
                                    string parameters2 = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.GetCollection, strItemID);

                                    if (compleateCallBack != null)
                                    {
                                        compleateCallBack(parameters2, 0, null);
                                    }
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

        #region 资源下载

        /// <summary>
        /// 资源下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Resource_DownLoad()
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //bool downloadWindowisShow = false;
                    foreach (var item in this.SpaceListViewItemList)
                    {
                        if (item.BookType == BookType.File)
                        {
                            MainWindow._MainWindow.Open_DownLoad_View();

                            string fileNameAboutExtention = item.Title + "." + item.FileType;

                            DownLoadItem downLoadEntity = new DownLoadItem()
                            {
                                FileUri = item.Uri,
                                FileName = fileNameAboutExtention,
                                WebClientHelper = new WebClientManage(),
                                AllSize = 100,
                                AllSize_MB = item.SizeDisplay,
                                Style = MainWindow.DownLoad_Item_Style,
                                Length = item.Self_File.Length
                            };

                            MainWindow._MainWindow.DownLoad_Items_Add(downLoadEntity);
                        }
                    }
                    MainWindow._MainWindow.Begin_Resource_DownLoad(dialog.SelectedPath);
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

        /// <summary>
        /// 资源下载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Resource_DownLoad(SpaceListViewItem item)
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();

                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (item.BookType == BookType.File)
                    {
                        MainWindow._MainWindow.Open_DownLoad_View();
                        string fileNameAboutExtention = item.Title + "." + item.FileType;

                        DownLoadItem downLoadEntity = new DownLoadItem()
                        {
                            FileUri = item.Uri,
                            FileName = fileNameAboutExtention,
                            WebClientHelper = new WebClientManage(),
                            AllSize = 100,
                            AllSize_MB = item.SizeDisplay,
                            Style = MainWindow.DownLoad_Item_Style,
                            Length = item.Self_File.Length
                        };

                        MainWindow._MainWindow.DownLoad_Items_Add(downLoadEntity);
                        MainWindow._MainWindow.Begin_Resource_DownLoad(dialog.SelectedPath);
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

        #region 资源上传

        protected void Resource_Upload(Action beginCallBack, Action<string, int, string> compleateCallBack)
        {
            try
            {
                if (this.currentBreadLine != null && this.currentBreadLine.Folder != null)
                {
                    //使用打开对话框
                    System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
                    dialog.Multiselect = true;
                    //点击确定
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (dialog.FileNames.Count() > 0)
                        {
                            MainWindow._MainWindow.BeginUploadCallBack = beginCallBack;
                            MainWindow._MainWindow.AllUploadCompleateCallBack = compleateCallBack;
                            MainWindow._MainWindow.Upload_Title_Tip = "文件上传";
                            MainWindow._MainWindow.Open_Upload_View();


                            foreach (var item in dialog.FileNames)
                            {
                                FileInfo fileInfo = new FileInfo(item);
                                FileUploadItem entity = new FileUploadItem()
                                {
                                    AllSize = fileInfo.Length,
                                    FolderID = currentBreadLine.Folder.ID,
                                    File_Size = FileManage.GetFileSize_MB_KB_Display(fileInfo.Length),
                                    FileName = fileInfo.Name,
                                    FilePath = item,
                                    Style = MainWindow.Upload_Item_Style,

                                };
                                MainWindow._MainWindow.UploadItems_Add(entity);

                            }

                            MainWindow._MainWindow.Begin_Resource_Upload();
                        }
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

        protected void Resource_Upload(List<string> filesList, Action beginCallBack, Action<string, int, string> compleateCallBack)
        {
            try
            {
                if (this.currentBreadLine != null && this.currentBreadLine.Folder != null)
                {
                    MainWindow._MainWindow.BeginUploadCallBack = beginCallBack;
                    MainWindow._MainWindow.AllUploadCompleateCallBack = compleateCallBack;
                    MainWindow._MainWindow.Upload_Title_Tip = "文件上传";
                    MainWindow._MainWindow.Open_Upload_View();

                    foreach (var item in filesList)
                    {
                        FileInfo fileInfo = new FileInfo(item);
                        FileUploadItem entity = new FileUploadItem()
                        {
                            AllSize = fileInfo.Length,
                            FolderID = currentBreadLine.Folder.ID,
                            File_Size = FileManage.GetFileSize_MB_KB_Display(fileInfo.Length),
                            FileName = fileInfo.Name,
                            FilePath = item,
                            Style = MainWindow.Upload_Item_Style,
                        };
                        MainWindow._MainWindow.UploadItems_Add(entity);
                    }

                    MainWindow._MainWindow.Begin_Resource_Upload();
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

        /// <summary>
        /// 单个上传（对话框）
        /// </summary>
        /// <param name="beginCallBack"></param>
        /// <param name="compleateCallBack"></param>
        protected void Resource_Upload_Single(Action beginCallBack, Action<string, int, string> compleateCallBack)
        {
            try
            {
                if (this.currentBreadLine != null && this.currentBreadLine.Folder != null)
                {
                    int ItemID = this.currentBreadLine.Folder.ID;

                    //使用打开对话框
                    System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();
                    //声明一个缓冲区
                    byte[] documentStream = null;

                    //文件名称
                    string documentName = null;

                    //点击确定
                    if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        //this.ShowTip();
                        if (beginCallBack != null)
                        {
                            beginCallBack();
                        }
                        //获取指定名称
                        documentName = dialog.SafeFileName;

                        string fileName = System.IO.Path.GetFileName(documentName);
                        //获取文件流
                        Stream stream = dialog.OpenFile();

                        //创建一个缓冲区
                        documentStream = new byte[stream.Length];
                        //将流写入指定缓冲区
                        stream.Read(documentStream, 0, documentStream.Length);

                        Resource_UploadHelper(compleateCallBack, ItemID, documentStream, fileName);
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

        protected void Resource_Upload(string file_Name, Action beginCallBack, Action<string, int, string> compleateCallBack)
        {
            try
            {
                if (this.currentBreadLine != null && this.currentBreadLine.Folder != null)
                {
                    int ItemID = this.currentBreadLine.Folder.ID;

                    //声明一个缓冲区
                    byte[] documentStream = null;

                    //this.ShowTip();
                    if (beginCallBack != null)
                    {
                        beginCallBack();
                    }

                    string fileName = System.IO.Path.GetFileName(file_Name);

                    using (FileStream strea_M = new FileStream(file_Name, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        //创建一个缓冲区
                        documentStream = new byte[strea_M.Length];
                        //将流写入指定缓冲区
                        strea_M.Read(documentStream, 0, documentStream.Length);

                        Resource_UploadHelper(compleateCallBack, ItemID, documentStream, fileName);
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

        private void Resource_UploadHelper(Action<string, int, string> compleateCallBack, int ItemID, byte[] documentStream, string fileName)
        {
            try
            {
                Dictionary<string, string> dir = new Dictionary<string, string>() { { SpaceCodeEnterEntity.PFileName, fileName } };
                string parameters = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.UploadFile, ItemID, dir);
                ModelManage.Space_Service.FileUpload(SpaceCodeEnterEntity.UploadFile, parameters,
                             SpaceCodeEnterEntity.LoginUserName,
                             SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, documentStream, new Action<Dictionary<string, object>>((dicResult) =>
                             {
                                 //跨线程（异步委托）
                                 this.Dispatcher.BeginInvoke(new Action(() =>
                                 {
                                     if (this.currentBreadLine.Folder != null && this.currentBreadLine.Folder.ID > 0)
                                     {
                                         int strItemID = this.currentBreadLine.Folder.ID;
                                         string parameters2 = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.GetCollection, strItemID);

                                         if (compleateCallBack != null)
                                         {
                                             compleateCallBack(parameters2, 0, null);
                                         }
                                     }
                                 }));
                             }));
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

        #region 全选、取消全选

        //protected void checkAll_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        foreach (var item in ListViewCollection)
        //        {
        //            item.IsItemChecked = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManage.WriteLog(this.GetType(), ex);
        //    }
        //    finally
        //    {
        //    }
        //}

        //protected void checkAll_Checked(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {

        //        foreach (var item in ListViewCollection)
        //        {
        //            item.IsItemChecked = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogManage.WriteLog(this.GetType(), ex);
        //    }
        //    finally
        //    {
        //    }
        //}

        protected void checkAll_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is CheckBox)
            {
                CheckBox checkAll = sender as CheckBox;
                if (checkAll.IsChecked == true)
                {
                    foreach (var item in ListViewCollection)
                    {
                        item.IsItemChecked = false;
                    }
                    checkAll.IsChecked = true;
                }
                else
                {
                    foreach (var item in ListViewCollection)
                    {
                        item.IsItemChecked = true;
                    }
                    checkAll.IsChecked = false;
                }
            }
        }

        #endregion

        #region 资源共享

        protected void Resource_Share()
        {
            try
            {
                MainWindow._MainWindow.Share_View_Changed_TO_Open();

                MainWindow._MainWindow.RootTreeInit();
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

        #region 显示控制面板

        /// <summary>
        /// 选择器控制中心
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="callBackType"></param>
        protected void ItemsSelectedEventCallBack(NavicateListView listView, SpaceListViewItem Item, CallBackType callBackType)
        {
            try
            {
                if (!listView.Items.Contains(Item)) return;

                switch (callBackType)
                {
                    case CallBackType.OpenItem:
                        this.OpenItemWith(Item, callBackType);
                        break;
                    case CallBackType.One_Selected:
                        this.SelectedDealWith(Item);
                        break;
                    case CallBackType.One_UnSelected:
                        this.UnselectedDealWith(Item);
                        break;
                    case CallBackType.KeDown:
                        this.KedownDealWidth(Item);
                        break;
                    case CallBackType.Share:
                        this.ShareDealWidth(Item);
                        break;
                    case CallBackType.Download:
                        this.DownloadWidth(Item);

                        break;
                    case CallBackType.Delete:
                        this.DeleteWidth(Item, callBackType);
                        break;
                    case CallBackType.ReName:
                        this.ReNameWidth(Item);
                        break;
                    default:
                        break;
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

        /// <summary>
        /// 获取uri地址
        /// </summary>
        /// <param name="Item"></param>
        protected void GetItemUri(SpaceListViewItem Item, Action callBack)
        {
            try
            {
                if (Item.BookType == Common.BookType.File && Item.Self_File != null)
                {
                    if (string.IsNullOrEmpty(Item.Uri))
                    {
                        string parameters = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.DisplayItem, Item.Self_File.ID);
                        ModelManage.Space_Service.Function_Invoke(SpaceCodeEnterEntity.DisplayItem, parameters, SpaceCodeEnterEntity.LoginUserName,
                            SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, new Action<Dictionary<string, object>>((dicResult) =>
                            {
                                SPItemEntity sPItemEntity = JsonManage.DictionaryToEntity<SPItemEntity>(dicResult, ',');
                                string uri = SpaceCodeEnterEntity.SPSiteAddressFront + sPItemEntity.FileRef;
                                //跨线程（异步委托）
                                this.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    Item.Self_File.ServerRelativeUrl = uri;
                                    Item.Uri = uri;
                                    if (callBack != null)
                                    {
                                        callBack();
                                    }
                                }));
                            }));
                    }
                    else
                    {
                        if (callBack != null)
                        {
                            callBack();
                        }
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

        /// <summary>
        /// 获取uri地址
        /// </summary>
        /// <param name="Item"></param>
        protected void GetItemUri(SpaceListViewItem Item)
        {
            try
            {
                if (Item.BookType == Common.BookType.File && Item.Self_File != null)
                {
                    if (string.IsNullOrEmpty(Item.Uri))
                    {
                        string parameters = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.DisplayItem, Item.Self_File.ID);
                        ModelManage.Space_Service.Function_Invoke(SpaceCodeEnterEntity.DisplayItem, parameters, SpaceCodeEnterEntity.LoginUserName,
                            SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, new Action<Dictionary<string, object>>((dicResult) =>
                            {
                                SPItemEntity sPItemEntity = JsonManage.DictionaryToEntity<SPItemEntity>(dicResult, ',');
                                string uri = SpaceCodeEnterEntity.SPSiteAddressFront + sPItemEntity.FileRef;
                                //跨线程（异步委托）
                                this.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    Item.Self_File.ServerRelativeUrl = uri;
                                    Item.Uri = uri;
                                }));
                            }));
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

        /// <summary>
        /// 打开子项
        /// </summary>
        /// <param name="Item"></param>
        protected void OpenItemWith(SpaceListViewItem Item, CallBackType callBackType)
        {
            try
            {
                if (this.OperationCallBack != null)
                {
                    this.OperationCallBack(Item, callBackType);
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

        /// <summary>
        /// 选择
        /// </summary>
        /// <param name="Item"></param>
        protected void SelectedDealWith(SpaceListViewItem Item)
        {
            try
            {
                this.GetItemUri(Item);

                if (!this.SpaceListViewItemList.Contains(Item))
                {
                    this.SpaceListViewItemList.Add(Item);
                    if (this.SpaceListViewItemList.Count == this.ListViewCollection.Count)
                    {
                        this.OperationCallBack(Item, CallBackType.All_Selected);
                    }
                    if (this.OperationCallBack != null)
                    {
                        this.OperationCallBack(Item, CallBackType.One_Selected);
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

        /// <summary>
        /// 取消选择
        /// </summary>
        /// <param name="Item"></param>
        protected void UnselectedDealWith(SpaceListViewItem Item)
        {
            try
            {
                if (this.SpaceListViewItemList.Contains(Item))
                {
                    this.SpaceListViewItemList.Remove(Item);
                    if (this.SpaceListViewItemList.Count == 0)
                    {
                        if (this.OperationCallBack != null)
                        {
                            this.OperationCallBack(Item, CallBackType.All_UnSelected);
                        }
                    }
                    if (this.OperationCallBack != null)
                    {
                        this.OperationCallBack(Item, CallBackType.One_UnSelected);
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

        /// <summary>
        /// 确认键按下（重命名）
        /// </summary>
        /// <param name="Item"></param>
        protected void KedownDealWidth(SpaceListViewItem Item)
        {
            try
            {
                int ItemID = 0;
                switch (Item.BookType)
                {
                    case BookType.File:
                        if (Item.Self_File != null)
                        {
                            ItemID = Item.Self_File.ID;
                        }
                        break;
                    case BookType.Folder:
                        if (Item.Self_Folder != null)
                        {
                            ItemID = Item.Self_Folder.ID;
                        }
                        break;
                    default:
                        break;
                }
                string parameters2 = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.ReName_Item, ItemID, new Dictionary<string, string>() { { SpaceCodeEnterEntity.NewName, Item.Title } });
                ModelManage.Space_Service.Function_Invoke(SpaceCodeEnterEntity.ReName_Item, parameters2, SpaceCodeEnterEntity.LoginUserName,
                            SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, new Action<Dictionary<string, object>>((dicResult) =>
                            {
                                //跨线程（异步委托）
                                this.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    Item.IsRenameState = false;
                                }));
                            }));
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 共享
        /// </summary>
        /// <param name="Item"></param>
        protected void ShareDealWidth(SpaceListViewItem Item)
        {
            try
            {
                Item.IsItemChecked = true;
                this.Resource_Share();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 下载子项
        /// </summary>
        /// <param name="Item"></param>
        protected void DownloadWidth(SpaceListViewItem Item)
        {
            try
            {
                this.GetItemUri(Item, new Action(() =>
                          {
                              this.Resource_DownLoad(Item);
                          }));
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        /// <summary>
        /// 删除子项
        /// </summary>
        /// <param name="Item"></param>
        protected void DeleteWidth(SpaceListViewItem Item, CallBackType callBackType)
        {
            try
            {
                if (this.OperationCallBack != null)
                {
                    this.OperationCallBack(Item, callBackType);
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

        /// <summary>
        /// 重命名
        /// </summary>
        /// <param name="Item"></param>
        protected void ReNameWidth(SpaceListViewItem Item)
        {
            try
            {
                Item.IsItemChecked = true;
                Item.IsRenameState = true;
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

        #region 视图更改

        protected void ViewSetting(viewType view_Type, NavicateListView listView)
        {
            try
            {
                ViewType = view_Type;
                switch (view_Type)
                {
                    case viewType.PictureView:
                        listView.Orientation = Orientation.Horizontal;
                        foreach (var item in this.ListViewCollection)
                        {
                            item.SettingStyle(ViewType);
                        }
                        if (this.SpaceListViewItemList.Count <= 0)
                        {
                        }
                        this.DisplayPanelVisibility = vy.Collapsed;
                        this.FirstColumnTitle = string.Empty;
                        break;
                    case viewType.ListView:

                        listView.Orientation = Orientation.Vertical;
                        foreach (var item in this.ListViewCollection)
                        {
                            item.SettingStyle(ViewType);
                        }

                        this.DisplayPanelVisibility = vy.Visible;
                        this.FirstColumnTitle = Defalut_FileTitlFlg;
                        break;
                    default:
                        break;
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
    }


}
