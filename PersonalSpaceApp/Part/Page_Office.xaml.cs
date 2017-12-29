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
using ConferenceCommon.FileHelper;
namespace PersonalSpaceApp.Part
{
    /// <summary>
    /// Page_Office.xaml 的交互逻辑
    /// </summary>
    public partial class Page_FileMode : PageViewBase
    {
        #region 内部字段

        /// <summary>
        /// 部分caml语句 （OR）
        /// </summary>
        string partCaml = null;

        #endregion

        #region 构造函数

        public Page_FileMode(NavicateType navicateType)
        {
            try
            {
                InitializeComponent();

                this.CollectionCaml(navicateType);

                this.ParametersInit();

                this.EventRegedit();
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
                this.NavicateListView3.Naivcate_SelectionChanged += NavicateListView1_Naivcate_SelectionChanged;
                SpaceCodeEnterEntity.fileOpenManage.LoadUICallBack = this.LoadUICallBack;
                SpaceListViewItem.ItemsSelectedEventCallBack = ItemsSelectedEventCallBack;
                this.OperationCallBack = Operation_CallBack;
                this.Loaded += Page_FileMode_Loaded;
                this.checkAll.PreviewMouseLeftButtonDown += checkAll_PreviewMouseLeftButtonDown;
                this.NavicateListView4.Naivcate_SelectionChanged += NavicateListView1_Naivcate_SelectionChanged;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
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

        void Page_FileMode_Loaded(object sender, RoutedEventArgs e)
        {
            this.listView.Width = this.ActualWidth;
            this.NavicateListView4.SelectedIndex = (int)ViewType;
        }

        #endregion

        #region 智存空间初始化

        public void SpaceInit()
        {
            try
            {
                //等待提示
                this.ShowTip();

                this.SpaceListViewItemList.Clear();
                this.NavicateListView3.IsEnabled = false;
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
                                    id_Root = Convert.ToInt32(id_Root_str);
                                    dicParameters_Root = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.GetCollection, id_Root);
                                    //ParentLeafName_Root = sPItemEntity_List[0].LinkFilename;
                                    //string caml = "<Where><Or>" + partCaml + "</Or></Where>";

                                    //string caml = "<Where><Or><Eq><FieldRef Name='DocIcon' /><Values><Value Type='Text'>" + "txt" + "</Value>"+"<Value Type='Text'>" + "doc" + "</Value></Eq></Or></Where>";
                                    dicParameters = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.GetFileByExtention, this.partCaml, new Dictionary<string, string>() 
                                    {
                                    {
                                        SpaceCodeEnterEntity.ViewAttributes, "Scope=\"Recursive\"" }, 
                                        { SpaceCodeEnterEntity.SP_ItemID, sPItemEntity_List[0].ID }
                                    });

                                    this.LoadFolderCenter(dicParameters, id_Root, ParentLeafName_Root);
                                }));
                            }
                        }
                    }));
                }
                else
                {

                    string dicParameters2 = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.GetFileByExtention, this.partCaml, new Dictionary<string, string>() 
                                    {
                                    {
                                        SpaceCodeEnterEntity.ViewAttributes, "Scope=\"Recursive\"" }, 
                                        { SpaceCodeEnterEntity.SP_ItemID, id_Root_str }
                                    });
                    this.LoadFolderCenter(dicParameters2, id_Root, ParentLeafName_Root);
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
            ModelManage.Space_Service.Function_Invoke(SpaceCodeEnterEntity.GetFileByExtention, dicParameters, SpaceCodeEnterEntity.LoginUserName, SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, new Action<Dictionary<string, object>>((dicResult) =>
            {
                try
                {
                    string json = Convert.ToString(dicResult[SpaceCodeEnterEntity.Collection]);
                    List<SPItemEntity> sPItemEntity_List = JsonManage.JsonToEntity<SPItemEntity>(json, ',');
                    this.Dispatcher.BeginInvoke(new Action(() =>
                              {
                                  foreach (var item in sPItemEntity_List)
                                  {
                                      if (item.ContentType.Equals(File_Flg))
                                      {

                                          fileType fileExtension = default(fileType);
                                          bool isTransferSuccessed = Enum.TryParse<fileType>(item.DocIcon, out fileExtension);
                                          if (isTransferSuccessed)
                                          {
                                              this.DataLoad_File(item);
                                          }
                                      }
                                  }
                                  this.HidenTip();
                              }));
                }
                catch (Exception ex)
                {
                    LogManage.WriteLog(this.GetType(), ex);
                }
                finally
                {
                }
            }));
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

                    case NavicateType.Resource_Upload:
                        this.Resource_Upload(this.ShowTip, this.LoadFolderCenter);
                        this.SpaceInit();
                        break;
                    case NavicateType.Resource_Share:
                        this.Resource_Share();
                        break;
                    case NavicateType.Resource_DownLoad:
                        this.Resource_DownLoad();
                        break;
                    case NavicateType.Resource_Delete:
                        this.Resource_Delete(this.ShowTip, null);
                        this.SpaceInit();
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

        #region 生成文件

        /// <summary>
        /// 生成文件
        /// </summary>    
        private void DataLoad_File(SPItemEntity sPVirtualFile)
        {
            try
            {
                //等待提示
                //this.ShowTip();
                SPVirtualFile file = new SPVirtualFile()
                {
                    ID = Convert.ToInt32(sPVirtualFile.ID),
                    Name = sPVirtualFile.LinkFilename,
                };
                long length;
                bool isSuccessed = long.TryParse(sPVirtualFile.FileSizeDisplay, out length);
                if (isSuccessed)
                {
                    file.Length = length;
                }

                //文件类型
                fileType file_Type = default(fileType);

                if (file.Name.Contains("."))
                {
                    //文件类型
                    string extention = IOPath.GetExtension(file.Name);
                    //去掉点
                    extention = extention.Replace(".", string.Empty);

                    //文件名称
                    string fileName = IOPath.GetFileNameWithoutExtension(file.Name);

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
                        BookType = BookType.File,
                        FileType = file_Type,
                        Self_File = file,
                        ParentView = this.listView,

                    };
                    listViewItem.SetSmallImg(imageUri_Small);
                    listViewItem.SizeDisplay = FileManage.GetFileSize_MB_KB_Display(file.Length);

                    DateTime dateTime;
                    bool isSuccessed_1 = DateTime.TryParse(sPVirtualFile.Modified, out dateTime);
                    if (isSuccessed_1)
                    {
                        listViewItem.UpdateTime = dateTime;
                    }

                    //添加书
                    this.ItemsAdd(listViewItem);
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
                //if (this.Loading.Visibility == vy.Collapsed)
                //{
                //    //等待提示
                //    this.Loading.Visibility = vy.Visible;
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

        #region 获取caml语句

        public void CollectionCaml(NavicateType navicateType)
        {
            try
            {
                partCaml = null;
                switch (navicateType)
                {
                    case NavicateType.MainNavicate_Video:
                        partCaml = "<Where><Or>"
                        + "<Eq><FieldRef Name='DocIcon' /><Value Type='Text'>mp4</Value></Eq>"
                        + "<Or><Eq><FieldRef Name='DocIcon' /><Value Type='Text'>avi</Value></Eq><Eq><FieldRef Name='DocIcon' /><Value Type='Text'>rmvb</Value></Eq></Or>"
                        + "</Or></Where>";
                        break;
                    case NavicateType.MainNavicate_Audio:

                        partCaml = "<Where>"
                          + "<Eq><FieldRef Name='DocIcon' /><Value Type='Text'>mp3</Value></Eq>"
                          + "</Where>";
                        break;
                    case NavicateType.MainNavicate_Picture:

                        partCaml = "<Where><Or>"
                           + "<Eq><FieldRef Name='DocIcon' /><Value Type='Text'>ico</Value></Eq>"
                           + "<Or><Or><Eq><FieldRef Name='DocIcon' /><Value Type='Text'>png</Value></Eq><Eq><FieldRef Name='DocIcon' /><Value Type='Text'>Jpg</Value></Eq></Or>"
                           + "<Or><Eq><FieldRef Name='DocIcon' /><Value Type='Text'>bmp</Value></Eq><Eq><FieldRef Name='DocIcon' /><Value Type='Text'>gif</Value></Eq></Or>"
                           + "</Or></Or></Where>";
                        break;
                    case NavicateType.MainNavicate_Office:
                        partCaml = "<Where><Or><Or>"
                            + "<Eq><FieldRef Name='DocIcon' /><Value Type='Text'>txt</Value></Eq>"
                            + "<Or><Eq><FieldRef Name='DocIcon' /><Value Type='Text'>ppt</Value></Eq><Eq><FieldRef Name='DocIcon' /><Value Type='Text'>pptx</Value></Eq></Or></Or>"
                            + "<Or><Or><Eq><FieldRef Name='DocIcon' /><Value Type='Text'>docx</Value></Eq><Eq><FieldRef Name='DocIcon' /><Value Type='Text'>doc</Value></Eq></Or>"
                            + "<Or><Eq><FieldRef Name='DocIcon' /><Value Type='Text'>xls</Value></Eq><Eq><FieldRef Name='DocIcon' /><Value Type='Text'>xlsx</Value></Eq></Or>"
                            + "</Or></Or></Where>";
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
