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
using PersonalSpaceApp.Part;
using ConferenceCommon.VersionHelper;
using PersonalSpaceApp.Window;
using ConferenceCommon.ProcessHelper;

using form = System.Windows.Forms;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;

namespace PersonalSpaceApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowBase
    {
        #region 静态资源

        /// <summary>
        /// 折叠(上传)
        /// </summary>
        static Storyboard Storyboard_FoldingUploadWindow = null;

        /// <summary>
        /// 展开（上传）
        /// </summary>
        static Storyboard Storyboard_ExpandUploadWindow = null;

        /// <summary>
        /// 折叠（共享）
        /// </summary>
        static Storyboard Storyboard_ShareFileWindowOpen = null;

        /// <summary>
        /// 展开（共享）
        /// </summary>
        static Storyboard Storyboard_ShareFileWindowClose = null;

         /// <summary>
        /// 折叠(下载)
        /// </summary>
        static Storyboard Storyboard_FoldingDownloadWindow = null;

        /// <summary>
        /// 展开（下载）
        /// </summary>
        static Storyboard Storyboard_ExpandDownloadWindow = null;
        

        /// <summary>
        /// 自身
        /// </summary>
        public static MainWindow _MainWindow = null;

        /// <summary>
        /// 上传子项样式
        /// </summary>
        public static Style Upload_Item_Style = null;

        /// <summary>
        /// 下载子项样式
        /// </summary>
        public static Style DownLoad_Item_Style = null;  

        /// <summary>
        /// 共享子项样式
        /// </summary>
        public static Style Share_Item_Style = null;

        /// <summary>
        /// 共享子项样式
        /// </summary>
        public static Style Ad_ListViewItem_Style = null;


        #endregion

        #region 一般属性

        static WebCredentialManage webCManage = null;
        /// <summary>
        /// web凭据验证管理模型
        /// </summary>
        public static WebCredentialManage WebCManage
        {
            get { return webCManage; }
            set { webCManage = value; }
        }

        #endregion

        #region 绑定属性

        vy controlPanelVisibility = vy.Collapsed;
        /// <summary>
        /// 控制面板显示
        /// </summary>
        public vy ControlPanelVisibility
        {
            get { return controlPanelVisibility; }
            set
            {
                if (value != controlPanelVisibility)
                {
                    controlPanelVisibility = value;
                    this.OnPropertyChanged("ControlPanelVisibility");
                }
            }
        }

        bool controlPanelEnable = false;
        /// <summary>
        /// 控制面板显示
        /// </summary>
        public bool ControlPanelEnable
        {
            get { return controlPanelEnable; }
            set
            {
                if (value != controlPanelEnable)
                {
                    controlPanelEnable = value;
                    this.OnPropertyChanged("ControlPanelEnable");
                }
            }
        }

        double uploadWindowOpacity = 1;
        /// <summary>
        /// 下载视图显示
        /// </summary>
        public double UploadWindowOpacity
        {
            get { return uploadWindowOpacity; }
            set
            {
                if (value != uploadWindowOpacity)
                {
                    uploadWindowOpacity = value;
                    this.OnPropertyChanged("UploadWindowOpacity");
                }
            }
        }

        double downloadWindowOpacity = 1;
        /// <summary>
        /// 下载视图显示
        /// </summary>
        public double DownloadWindowOpacity
        {
            get { return downloadWindowOpacity; }
            set
            {
                if (value != downloadWindowOpacity)
                {
                    downloadWindowOpacity = value;
                    this.OnPropertyChanged("DownloadWindowOpacity");
                }
            }
        }

        vy uploadWindowVisibility = vy.Collapsed;
        /// <summary>
        /// 下载视图显示
        /// </summary>
        public vy UploadWindowVisibility
        {
            get { return uploadWindowVisibility; }
            set
            {
                if (value != uploadWindowVisibility)
                {
                    uploadWindowVisibility = value;
                    this.OnPropertyChanged("UploadWindowVisibility");
                }
            }
        }

        vy downloadWindowVisibility = vy.Collapsed;
        /// <summary>
        /// 上载视图显示
        /// </summary>
        public vy DownloadWindowVisibility
        {
            get { return downloadWindowVisibility; }
            set
            {
                if (value != downloadWindowVisibility)
                {
                    downloadWindowVisibility = value;
                    this.OnPropertyChanged("DownloadWindowVisibility");
                }
            }
        }

        #endregion

        #region 内部字段

        /// <summary>
        /// 选择的页面
        /// </summary>
        PageViewBase Selected_PageViewBase = null;

        #endregion

        #region 构造函数

        public MainWindow()
        {
            try
            {
                InitializeComponent();

                this.Resource_Collection();

                this.EventRegedit();

                this.ParametersInit();
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

        #region 资源收集

        void Resource_Collection()
        {
            try
            {
                _MainWindow = this;

                if (this.Resources.Contains("Storyboard_FoldingUploadWindow"))
                {
                    Storyboard_FoldingUploadWindow = this.Resources["Storyboard_FoldingUploadWindow"] as Storyboard;
                }

                if (this.Resources.Contains("Storyboard_ExpandUploadWindow"))
                {
                    Storyboard_ExpandUploadWindow = this.Resources["Storyboard_ExpandUploadWindow"] as Storyboard;
                }

                if (this.Resources.Contains("Storyboard_ShareFileWindowOpen"))
                {
                    Storyboard_ShareFileWindowOpen = this.Resources["Storyboard_ShareFileWindowOpen"] as Storyboard;
                }

                if (this.Resources.Contains("Storyboard_ShareFileWindowClose"))
                {
                    Storyboard_ShareFileWindowClose = this.Resources["Storyboard_ShareFileWindowClose"] as Storyboard;
                }

                 if (this.Resources.Contains("Storyboard_FoldingDownloadWindow"))
                {
                    Storyboard_FoldingDownloadWindow = this.Resources["Storyboard_FoldingDownloadWindow"] as Storyboard;
                }

                 if (this.Resources.Contains("Storyboard_ExpandDownloadWindow"))
                {
                    Storyboard_ExpandDownloadWindow = this.Resources["Storyboard_ExpandDownloadWindow"] as Storyboard;
                }
                



                ResourceDictionary resourceDictionary = Application.Current.Resources;

                if (resourceDictionary.Contains("Upload_Item_Style"))
                {
                    Upload_Item_Style = resourceDictionary["Upload_Item_Style"] as Style;
                }

                if (resourceDictionary.Contains("DownLoad_Item_Style"))
                {
                    DownLoad_Item_Style = resourceDictionary["DownLoad_Item_Style"] as Style;
                }
                
                if (resourceDictionary.Contains("Share_Item_Style"))
                {
                    Share_Item_Style = resourceDictionary["Share_Item_Style"] as Style;
                }

                if (resourceDictionary.Contains("Ad_ListViewItem_Style"))
                {
                    Ad_ListViewItem_Style = resourceDictionary["Ad_ListViewItem_Style"] as Style;
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

        #region 初始化

        public void ParametersInit()
        {
            try
            {
                this.NavicateListView1.SelectedIndex = 0;

                string dicParameters = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.GetCurrentDisPlayName);
                ModelManage.Space_Service.Function_Invoke(SpaceCodeEnterEntity.GetCurrentDisPlayName, dicParameters, SpaceCodeEnterEntity.LoginUserName, SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, new Action<Dictionary<string, object>>((dicResult) =>
                     {
                         if (dicResult.Count > 0)
                         {
                          SpaceCodeEnterEntity.SelfName =   Convert.ToString( dicResult[SpaceCodeEnterEntity.DisplayName]);

                         }
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

        #region 关闭事件

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Application.Current.Shutdown(0);
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
                this.NavicateListView1.Naivcate_SelectionChanged += NavicateListView1_Naivcate_SelectionChanged;

                FileUploadItem.UpLoad_Delete_Click_CallBack = UpLoad_Delete_Click_CallBack;

                DownLoadItem.DownLoad_Delete_Click_CallBack = DownLoad_Delete_Click_CallBack;

                Ad_TreeViewItem.Check_CallBack = Check_CallBack;

                Ad_TreeViewItem.UnCheck_CallBack = UnCheck_CallBack;

                Ad_ListViewItem.Item_Cancel_CallBack = Item_Cancel_CallBack;

                this.Closing += MainWindow_Closing;
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

        #region 导航区域选择

        void NavicateListView1_Naivcate_SelectionChanged(NavicateListView listView, NavicateListViewItem listViewItem)
        {
            try
            {
                PageMode pageMode = null;

                switch (listViewItem.NavicateType)
                {
                    case NavicateType.None:
                        return;
                    case NavicateType.MainNavicate_All:
                        pageMode = new PageMode();
                        pageMode.SpaceInit();
                        this.borContent.Child = pageMode;
                        this.Selected_PageViewBase = pageMode;
                        return;
                    case NavicateType.MainNavicate_Recycle:
                        this.borContent.Child = null;
                        return;
                    default:
                        Page_FileMode page_FileMode = new Page_FileMode(listViewItem.NavicateType);
                        page_FileMode.SpaceInit();
                        this.borContent.Child = page_FileMode;
                        this.Selected_PageViewBase = page_FileMode;
                        return;
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

        private void gridCancelButton_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("是否注销", "操作提示", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown(0);
                    ProcessManage.OpenFileByLocalAddress(SpaceCodeEnterEntity.StartApplication);
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

        private void WindowBase_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 在此处添加事件处理程序实现。
            //this.gridCancelWindow.Visibility = Visibility.Collapsed;
        }

        //private void gridCloseButton_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    // 在此处添加事件处理程序实现。
        //    this.Close();
        //}

        #endregion

        #region UI控制区域

        /// <summary>
        /// 窗体最小化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridMinimizeButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        /// <summary>
        /// 状态切换（最大化、正常）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridMaximizingButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Normal)
            {
                this.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                this.WindowState = System.Windows.WindowState.Normal;
            }
        }

        /// <summary>
        /// 状态切换（最大化、正常）,点击标题栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaxState)
                {
                    //       form.Screen scr = form.Screen.PrimaryScreen;
                    //scr.WorkingArea
                    //        IsMaxState = true;
                    this.WindowState = System.Windows.WindowState.Maximized;
                }
                else
                {
                    this.WindowState = System.Windows.WindowState.Normal;
                    this.IsMaxState = false;
                }
            }
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridCloseButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("是否退出", "操作提示", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown(0);
            }
        }

        #endregion

       

       
    }
}
