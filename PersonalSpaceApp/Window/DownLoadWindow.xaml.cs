
using ConferenceCommon.LogHelper;
using PersonalSpaceApp.WPFHelper;
using Microsoft.Win32;
using PersonalSpaceApp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace PersonalSpaceApp.Control
{
    /// <summary>
    /// DownLoadWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DownLoadWindow : WindowOperationBase
    {
        #region 内部字段

        /// <summary>
        /// 下载列表
        /// </summary>
        List<DownLoadEntity> downLoadEntityList = new List<DownLoadEntity>();

        #endregion

        #region 静态字段

        /// <summary>
        /// 百分比格式
        /// </summary>
        static string Percent_Formart = "{0}%";

        /// <summary>
        /// 百分比标示
        /// </summary>
        static string Percent_flg = "100%";

       
        private static DownLoadWindow downLoadWindow = null;
        /// <summary>
        /// 下载窗体(单例循环模式)
        /// </summary>
        public static DownLoadWindow _DownLoadWindow
        {
            get
            {
                if (downLoadWindow == null)
                {
                    downLoadWindow = new DownLoadWindow();                  
                }
                
                return DownLoadWindow.downLoadWindow;
            }          
        }

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

        #region 构造函数

        public DownLoadWindow()
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
                //this.btnClose.Click += btnClose_Click;
                this.Closing += DownLoadWindow_Closing;
                this.btnPathChanged.Click += btnPathChanged_Click;
                this.btnOK.Click += btnOK_Click;
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

        #region 开始下载

        void btnOK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.downLoadEntityList.Count > 0)
                {
                    this.btnOK.Visibility = vy.Hidden;
                    this.btnPathChanged.Visibility = vy.Hidden;
                }
                foreach (var item in this.downLoadEntityList)
                {
                    //long allSize = item.AllSize
                    item.WebClientHelper.FileDown(item.FileUri, this.SourceRoot + "\\" + item.FileName, SpaceCodeEnterEntity.LoginUserName,
                        SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, new Action<int>((intProcess) =>
                    {
                        item.Progress = intProcess;
                        item.CompletePercent = string.Format(Percent_Formart, intProcess);

                    }), new Action<Exception, bool>((ex, IsSuccessed) =>
                    {
                        item.Progress = item.AllSize;
                        item.CompletePercent = Percent_flg;
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

        #region 更改文件下载路径

        void btnPathChanged_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.SelectedPath = this.SourceRoot;
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.SourceRoot = dialog.SelectedPath;
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

        void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        void DownLoadWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                downLoadWindow = null;
                foreach (var item in this.downLoadEntityList)
                {
                    item.WebClientHelper.Dispose();
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

        #region 移除下载

        private void ItemDelste_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Object selectedItem = this.downLoadDataGrid.SelectedItem;
                if (selectedItem is DownLoadEntity)
                {
                    DownLoadEntity downLoadEntity = selectedItem as DownLoadEntity;
                    downLoadEntity.WebClientHelper.Dispose();
                    this.downLoadDataGrid.Items.Remove(downLoadEntity);
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

        #region 加载下载文件

        public void Items_Add(DownLoadEntity downLoadEntity)
        {
            try
            {
                this.downLoadDataGrid.Items.Add(downLoadEntity);

                this.downLoadEntityList.Add(downLoadEntity);
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
