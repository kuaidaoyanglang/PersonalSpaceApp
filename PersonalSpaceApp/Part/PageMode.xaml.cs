using ConferenceCommon.LogHelper;
using PersonalSpaceApp.WPFControl;
using PersonalSpaceApp.WPFHelper;
using ConferenceModel;
//using ConferenceModel.Common;
using PersonalSpaceApp.Common;
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
using vy = System.Windows.Visibility;
using fileType = PersonalSpaceApp.WPFControl.FileType;
using IOPath = System.IO.Path;
using ConferenceCommon.TimerHelper;
using System.Threading;
using ConferenceCommon.JsonHelper;
using System.Collections.ObjectModel;
using viewType = PersonalSpaceApp.WPFHelper.ViewType;
using System.ComponentModel;
using ConferenceCommon.FileHelper;
using System.IO;


namespace PersonalSpaceApp.Part
{
    /// <summary>
    /// PageMode.xaml 的交互逻辑
    /// </summary>
    public partial class PageMode : PageViewBase
    {

        #region 注册依赖项属性

        public static readonly DependencyProperty BreadLineRootTitleProperty = DependencyProperty.Register("BreadLineRootTitle", typeof(string), typeof(PageMode), new FrameworkPropertyMetadata(null));
        /// <summary>
        /// 面板线标题
        /// </summary>
        public string BreadLineRootTitle
        {
            get
            {
                return (string)this.GetValue(BreadLineRootTitleProperty);
            }
            set
            {
                this.SetValue(BreadLineRootTitleProperty, value);
            }
        }

        #endregion

        #region 构造函数

        public PageMode()
        {
            try
            {

                InitializeComponent();
                this.DataContext = this;


                this.ParametersInit();

                this.EventRegedit();


                //this.SpaceInit();
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

        public void ParametersInit()
        {
            try
            {
                this.DataContext = this;

                this.listView.Items.Clear();

                this.BreadLineRoot.Title = "个人空间";

                //this.NavicateListView4.SelectedIndex = (int)ViewType;
                //this.ViewSetting(ViewType);
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
                this.NavicateListView2.Naivcate_SelectionChanged += NavicateListView1_Naivcate_SelectionChanged;
                this.NavicateListView3.Naivcate_SelectionChanged += NavicateListView1_Naivcate_SelectionChanged;
                SpaceCodeEnterEntity.fileOpenManage.LoadUICallBack = this.LoadUICallBack;
                SpaceListViewItem.ItemsSelectedEventCallBack = base.ItemsSelectedEventCallBack;
                this.OperationCallBack = Operation_CallBack;
                this.Loaded += PageMode_Loaded;
                this.NavicateListView4.Naivcate_SelectionChanged += NavicateListView1_Naivcate_SelectionChanged;

                this.checkAll.PreviewMouseLeftButtonDown += checkAll_PreviewMouseLeftButtonDown;

                this.listView.DragOver += listView_DragOver;
                this.listView.Drop += listView_Drop;
                this.SizeChanged += PageMode_SizeChanged;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        void PageMode_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.listView.Width = this.ActualWidth;

        }

        void PageMode_Loaded(object sender, RoutedEventArgs e)
        {
            this.NavicateListView4.SelectedIndex = (int)ViewType;
        }

        private void Operation_CallBack(SpaceListViewItem Item, CallBackType callBackType)
        {
            try
            {
                switch (callBackType)
                {
                    case CallBackType.OpenItem:
                        this.book_OpenFileEvent(Item, this.LoadFolderCenter);
                        break;
                    case CallBackType.One_Selected:
                        this.NavicateListView3.IsEnabled = true;
                        break;
                    case CallBackType.All_Selected:
                        this.checkAll.IsChecked = true;

                        break;
                    case CallBackType.One_UnSelected:
                        this.checkAll.IsChecked = false;
                        break;

                    case CallBackType.All_UnSelected:
                        this.NavicateListView3.IsEnabled = false;
                        break;
                    case CallBackType.Delete:
                        this.Resource_Delete(Item, this.ShowTip, this.LoadFolderCenter);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(typeof(App), ex);
            }
            finally
            {
            }
        }

        #endregion

        #region 加载面包线根节点

        public void BreadLine_Load()
        {
            try
            {

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

        #region 事件处理中心

        void NavicateListView1_Naivcate_SelectionChanged(NavicateListView listView, NavicateListViewItem listViewItem)
        {
            try
            {
                if (listView.SelectedIndex == -1)
                    return;

                switch (listViewItem.NavicateType)
                {
                    case NavicateType.None:
                        break;
                    case NavicateType.Resource_Upload:
                        this.Resource_Upload(this.ShowTip, this.LoadFolderCenter);
                        break;
                    case NavicateType.Resource_CreateFolder:
                        this.Resource_CreateFolder(this.ShowTip, this.LoadFolderCenter);
                        break;
                    case NavicateType.Resource_Share:
                        this.Resource_Share();
                        break;
                    case NavicateType.Resource_DownLoad:
                        this.Resource_DownLoad();
                        break;
                    case NavicateType.Resource_Delete:
                        this.Resource_Delete(this.ShowTip, this.LoadFolderCenter);

                        break;
                    case NavicateType.Resource_Remove:
                        break;
                    case NavicateType.Resource_Rename:

                        if (SpaceListViewItemList.Count > 0)
                        {
                            SpaceListViewItem item = SpaceListViewItemList[SpaceListViewItemList.Count - 1];
                            item.IsRenameState = true;
                        }
                        break;

                    case NavicateType.List_View:
                        this.Item_ListView.BG_Brush = this.Item_ListView.BG_SelectBrush;
                        this.Item_PcitureView.BG_Brush = this.Item_PcitureView.BG_UnSelectBrush;
                        this.ViewSetting(viewType.ListView, this.listView);
                        break;
                    case NavicateType.Picture_View:
                        this.Item_PcitureView.BG_Brush = this.Item_PcitureView.BG_SelectBrush;
                        this.Item_ListView.BG_Brush = this.Item_ListView.BG_UnSelectBrush;
                        this.ViewSetting(viewType.PictureView, this.listView);
                        break;
                    default:
                        break;
                }

                switch (listView.AreaType)
                {
                    case AreaType.NormalButton:
                        listView.SelectedIndex = -1;
                        break;
                    case AreaType.NavicateType:

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

        #region 智存空间初始化

        public void SpaceInit()
        {
            try
            {
                //等待提示
                this.ShowTip();

                this.listView.Items.Clear();
                if (dicParameters_Root == null)
                {
                    string dicParameters = string.Empty;
                    switch (this.SpaceType)
                    {
                        case SpaceType.PersonSpace:

                            string caml = "<Where><Eq><FieldRef Name='FileLeafRef' /><Value Type='Text'>" + SpaceCodeEnterEntity.LoginUserName + "</Value></Eq></Where>";
                            dicParameters = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.GetCollection, caml);
                            break;
                        case SpaceType.MeetSpace:
                            //string caml2 = "<Where><Eq><FieldRef Name='FileLeafRef' /><Value Type='Text'>" + SpaceCodeEnterEntity.ConferenceName + "</Value></Eq></Where>";
                            dicParameters = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.GetCollection);
                            break;
                        case SpaceType.TopicSpace:
                            break;
                        default:
                            break;
                    }

                    ModelManage.Space_Service.Function_Invoke(SpaceCodeEnterEntity.GetCollection, dicParameters, SpaceCodeEnterEntity.LoginUserName, SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, new Action<Dictionary<string, object>>((dicResult) =>
                    {
                        if (dicResult.Count > 0)
                        {
                            string json = Convert.ToString(dicResult[SpaceCodeEnterEntity.Collection]);
                            List<SPItemEntity> sPItemEntity_List = JsonManage.JsonToEntity<SPItemEntity>(json, ',');

                            if (sPItemEntity_List.Count > 0)
                            {
                                //跨线程（异步委托）
                                this.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    id_Root_str = sPItemEntity_List[0].ID;
                                    id_Root = Convert.ToInt32(sPItemEntity_List[0].ID);
                                    dicParameters_Root = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.GetCollection, id_Root);
                                    ParentLeafName_Root = sPItemEntity_List[0].LinkFilename;
                                    this.LoadFolderCenter(dicParameters_Root, id_Root, ParentLeafName_Root);
                                }));
                            }
                        }
                    }));
                }
                else
                {
                    this.LoadFolderCenter(dicParameters_Root, id_Root, ParentLeafName_Root);
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


        #region 数据加载中心

        public void LoadFolderCenter(string dicParameters, int itemID, string itemName)
        {
            ListViewCollection.Clear();
            ModelManage.Space_Service.Function_Invoke(SpaceCodeEnterEntity.GetCollection, dicParameters, SpaceCodeEnterEntity.LoginUserName, SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, new Action<Dictionary<string, object>>((dicResult) =>
            {
                //跨线程（异步委托）
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        this.SpaceListViewItemList.Clear();
                        this.NavicateListView3.IsEnabled = false;

                        string json = Convert.ToString(dicResult[SpaceCodeEnterEntity.Collection]);
                        List<SPItemEntity> sPItemEntity_List = JsonManage.JsonToEntity<SPItemEntity>(json, ',');
                        if (this.currentBreadLine == null || this.currentBreadLine.Folder == null)
                        {
                            SPVirtualFolder folder = new SPVirtualFolder();
                            //folder.Name = "文档";
                            if (itemID > 0)
                            {
                                folder.ID = itemID;
                            }
                            if (!string.IsNullOrEmpty(itemName))
                            {
                                folder.Name = itemName;
                            }
                            SPVirtualItemInit(folder, sPItemEntity_List);

                            //设置根目录(第一层面包线,文件夹为第一层文件夹【会议文件夹】)
                            this.BreadLineRoot.Folder = folder;
                            //设置当前所处的目录
                            this.currentBreadLine = this.BreadLineRoot;

                            base.Bread_LineRoot = this.BreadLineRoot;
                            //清除之前的那根线【第一个面包线是不需要那根线的】
                            this.BreadLineRoot.ClearBeforeLine();
                            //面包线点击事件
                            this.BreadLineRoot.LineClickEventCallBack = breadLine_LineClickEvent;
                        }
                        else
                        {
                            SPVirtualItemInit(this.currentBreadLine.Folder, sPItemEntity_List);
                        }
                        //刷新当前页面
                        this.RefleshSpaceData(this.currentBreadLine);
                    }
                    catch (Exception ex)
                    {
                        LogManage.WriteLog(this.GetType(), ex);
                    }
                    finally
                    {
                    }
                }));
            }));
        }

        #endregion

        #region 智存空间服务提供


        public void SPVirtualItemInit(SPVirtualFolder folder, List<SPItemEntity> sPItemEntity_List)
        {
            try
            {
                folder.Files.Clear();
                folder.Folders.Clear();
                foreach (var item in sPItemEntity_List)
                {
                    if (item.ContentType.Equals(File_Flg))
                    {
                        SPVirtualFile file = new SPVirtualFile()
                        {
                            ID = Convert.ToInt32(item.ID),
                            Name = item.LinkFilename,

                        };
                        long length;
                        bool isSuccessed = long.TryParse(item.FileSizeDisplay, out length);
                        if (isSuccessed)
                        {
                            file.Length = length;
                        }

                        DateTime dateTime;
                        bool isSuccessed_1 = DateTime.TryParse(item.Modified, out dateTime);
                        if (isSuccessed_1)
                        {
                            file.UpdateTime = dateTime;
                        }

                        folder.Files.Add(file);
                    }
                    else
                    {
                        SPVirtualFolder fold = new SPVirtualFolder()
                        {
                            ID = Convert.ToInt32(item.ID),
                            Name = item.LinkFilename,
                        };

                        DateTime dateTime;
                        bool isSuccessed_1 = DateTime.TryParse(item.Modified, out dateTime);
                        if (isSuccessed_1)
                        {
                            fold.UpdateTime = dateTime;
                        }
                        folder.Folders.Add(fold);
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

        #region 面包线点击

        /// <summary>
        /// 根面包线点击事件
        /// </summary>
        /// <param name="breadLine"></param>
        void breadLine_LineClickEvent(BreadLine breadLine)
        {
            try
            {
                  //设置当前面包线
                this.currentBreadLine = null;

                //清空面包线之后的子项
                breadLine.BreadLineChild = null;
                //设置当前的目录
                this.currentBreadLine = breadLine;

                if (this.currentBreadLine.Folder != null)
                {
                    int ItemID = this.currentBreadLine.Folder.ID;
                    string parameters = string.Empty;
                    if (ItemID > 0)
                    {
                        parameters = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.GetCollection, ItemID);
                    }
                    else
                    {
                        parameters = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.GetCollection);
                    }

                    this.LoadFolderCenter(parameters, 0, null);

                }

                //刷新当前页面
                //this.RefleshSpaceData(this.currentBreadLine);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        #endregion

        #region 数据刷新

        /// <summary>
        /// 数据刷新
        /// </summary>
        /// <param name="folder"></param>
        public void RefleshSpaceData(BreadLine bread_Line)
        {
            try
            {
                //面包线是否为null
                if (bread_Line == null)
                {
                    return;
                }
                if (bread_Line.Folder != null)
                {
                    //等待提示
                    this.ShowTip();
                    this.listView.Items.Clear();
                    //获取文件夹
                    SPVirtualFolder folder = bread_Line.Folder;

                    TimerJob.StartRun_Sync(new Action(() =>
                    {
                        DataLoad_All(folder.Folders, folder.Files);
                    }));
                }
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
        }

        /// <summary>
        /// 数据加载
        /// </summary>
        /// <param name="folderList"></param>
        /// <param name="fileList"></param>
        public void DataLoad_All(List<SPVirtualFolder> folderList, List<SPVirtualFile> fileList)
        {
            #region 数据加载

            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                try
                {
                    try
                    {
                        //生成文件夹
                        this.DataLoad_Directory(folderList);
                        //生成文件
                        this.DataLoad_File(fileList);
                        ////整体书架刷新
                        //this.ReFlush_BookShell();
                        //等待提示
                        this.HidenTip();
                    }
                    catch (Exception ex)
                    {
                        LogManage.WriteLog(this.GetType(), ex);
                    };
                }
                catch (Exception ex)
                {
                    LogManage.WriteLog(this.GetType(), ex);
                }
                finally
                {
                }
            }));


            #endregion
        }

        /// <summary>
        /// 生成文件(realDisplay)
        /// </summary>
        /// <param name="folderList">文件夹目录</param>
        /// <param name="row1Height">行1高</param>
        /// <param name="fileHeigth">文件高</param>
        /// <param name="fileWidth">文件宽</param>
        private void DataLoad_File(List<SPVirtualFile> fileList)
        {
            try
            {
                //文件类型
                fileType file_Type = default(fileType);
                //生成文件（根据文件类型去匹配）
                foreach (var item in fileList)
                {
                    if (item.Name.Contains("."))
                    {
                        //文件类型
                        string extention = IOPath.GetExtension(item.Name);
                        //去掉点
                        extention = extention.Replace(".", string.Empty);

                        //文件名称
                        string fileName = IOPath.GetFileNameWithoutExtension(item.Name);

                        //转换为枚举
                        Enum.TryParse(extention, true, out file_Type);
                        //图片
                        string imageUri = null;
                        imageUri = SpaceCodeEnterEntity.extetionImageFolderName + extention + SpaceCodeEnterEntity.pictureExtension;

                        string imageUri_Small = null;
                        imageUri_Small = SpaceCodeEnterEntity.extetionImageFolderName_Small + extention + SpaceCodeEnterEntity.pictureExtension;

                        SpaceListViewItem listViewItem = new SpaceListViewItem(imageUri, ViewType)
                        {
                            Title = fileName,
                            SpaceType = this.SpaceType,
                            UpdateTime = item.UpdateTime,
                            BookType = BookType.File,
                            FileType = file_Type,
                            Self_File = item,
                            ParentView = this.listView,
                        };
                        listViewItem.SetSmallImg(imageUri_Small);
                        listViewItem.SizeDisplay = FileManage.GetFileSize_MB_KB_Display(item.Length); ;
                        //添加书
                        this.ItemsAdd(listViewItem);
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
        /// 生成文件夹
        /// </summary>
        /// <param name="folderList">文件夹目录</param>
        /// <param name="row1Height">行1高</param>
        /// <param name="fileHeigth">文件高</param>
        /// <param name="fileWidth">文件宽</param>
        private void DataLoad_Directory(List<SPVirtualFolder> folderList)
        {
            try
            {
                string folder_Img_Small = null;
                folder_Img_Small = SpaceCodeEnterEntity.folderPngUriPart_Small;

                string folder_Img = null;
                folder_Img = SpaceCodeEnterEntity.folderPngUriPart;



                //生成书本形式的文件夹
                foreach (var folderItem in folderList)
                {

                    SpaceListViewItem listViewItem = new SpaceListViewItem(folder_Img, ViewType)
                    {
                        Title = folderItem.Name,
                        SpaceType = this.SpaceType,
                        BookType = BookType.Folder,
                        UpdateTime = folderItem.UpdateTime,
                        Self_Folder = folderItem,
                        SizeDisplay = "-",
                        IsShowOperationIcon = false,
                        ParentView = this.listView,
                    };
                    this.ItemsAdd(listViewItem);

                    listViewItem.SetSmallImg(folder_Img_Small);

                    //创建一个面包线（目前在这里创建和根面包线【书架自带】）
                    BreadLine breadLine = new BreadLine() { Folder = folderItem, Title = folderItem.Name };
                    //关联指定生成的面包线（）
                    listViewItem.BreadLine = breadLine;
                    //面包线点击事件
                    breadLine.LineClickEventCallBack = breadLine_LineClickEvent;
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



        #region 添加子项（先决条件：定位）

        /// <summary>
        /// 添加子项（先决条件：定位）
        /// </summary>
        public void ItemsAdd(SpaceListViewItem listViewItem)
        {
            try
            {
                this.listView.Items.Add(listViewItem);
                this.ListViewCollection.Add(listViewItem);
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

        #region 显示隐藏等待提示

        /// <summary>
        /// 显示提示
        /// </summary>
        public void ShowTip()
        {
            try
            {
                //this.Dispatcher.BeginInvoke(new Action(() =>
                //{
                //if (this.Loading.Visibility == vy.Collapsed)
                //{
                //    //等待提示
                //    this.Loading.Visibility = vy.Visible;
                //}
                if (this.BreadLineRoot.IsEnabled)
                {
                    this.BreadLineRoot.IsEnabled = false;
                }
                //}));

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
        /// 隐藏提示
        /// </summary>
        public void HidenTip()
        {
            try
            {
                //if (this.Loading.Visibility == vy.Visible)
                //{
                //    //等待提示
                //    this.Loading.Visibility = vy.Collapsed;
                //    //this.Loading.Visibility = vy.Collapsed;
                //}
                if (!this.BreadLineRoot.IsEnabled)
                {
                    this.BreadLineRoot.IsEnabled = true;
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

        #region 文件拖拽

        private void listView_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effects = DragDropEffects.All;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }


                e.Handled = false;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }
        private void listView_Drop(object sender, DragEventArgs e)
        {
            try
            {
                var dropObject = ((System.Array)e.Data.GetData(DataFormats.FileDrop));
                List<string> filesList = new List<string>();
                for (int i = 0; i < dropObject.Length; i++)
                {
                    string fileName = dropObject.GetValue(i).ToString();

                    if (File.Exists(fileName))
                    {
                        filesList.Add(fileName);
                    }
                }

                if (filesList.Count > 0)
                {
                    this.Resource_Upload(filesList, this.ShowTip, this.LoadFolderCenter);
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
