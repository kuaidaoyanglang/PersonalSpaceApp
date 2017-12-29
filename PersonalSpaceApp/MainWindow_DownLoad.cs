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

namespace PersonalSpaceApp
{
    public partial class MainWindow : WindowBase
    {
        #region 事件回调

        /// 上传所有文件完成回调
        /// </summary>
        internal Action<string, int, string> AllDownLoadCompleateCallBack = null;

        /// <summary>
        /// 开始上传回调
        /// </summary>
        internal Action BeginDownLoadCallBack = null;

        #endregion

        #region 绑定属性

        int downLoad_CompleateCount = 0;
        /// <summary>
        /// 已完成的下载数量
        /// </summary>
        public int DownLoad_CompleateCount
        {
            get { return downLoad_CompleateCount; }
            set
            {
                if (value != downLoad_CompleateCount)
                {
                    downLoad_CompleateCount = value;
                    this.OnPropertyChanged("DownLoad_CompleateCount");
                }
            }
        }

        string downLoad_Title_Tip = "文件下载";
        /// <summary>
        /// 下载标题提示
        /// </summary>
        public string DownLoad_Title_Tip
        {
            get { return downLoad_Title_Tip; }
            set
            {
                if (value != downLoad_Title_Tip)
                {
                    downLoad_Title_Tip = value;
                    this.OnPropertyChanged("DownLoad_Title_Tip");
                }
            }
        }

        int downLoad_AllCount;
        /// <summary>
        /// 需要下载的总数量
        /// </summary>
        public int DownLoad_AllCount
        {
            get { return downLoad_AllCount; }
            set
            {
                if (value != downLoad_AllCount)
                {
                    downLoad_AllCount = value;
                    this.OnPropertyChanged("DownLoad_AllCount");
                }
            }
        }

        bool isAllDownLoadCompleate = true;
        /// <summary>
        /// 是否全部下载完成
        /// </summary>
        public bool IsAllDownLoadCompleate
        {
            get { return isAllDownLoadCompleate; }
            set
            {
                if (value != isAllDownLoadCompleate)
                {
                    isAllDownLoadCompleate = value;
                    this.OnPropertyChanged("IsAllDownLoadCompleate");
                }
            }
        }

        ObservableCollection<DownLoadItem> downLoadItemCollection = new ObservableCollection<DownLoadItem>()
        {

        };
        /// <summary>
        /// 下载子项
        /// </summary>
        public ObservableCollection<DownLoadItem> DownLoadItemCollection
        {
            get { return downLoadItemCollection; }
            set { downLoadItemCollection = value; }
        }

        DownLoadItem selected_FileDownLoadItem;
        /// <summary>
        /// 上传子项选择
        /// </summary>
        public DownLoadItem Selected_FileDownLoadItem
        {
            get { return selected_FileDownLoadItem; }
            set
            {
                if (value != selected_FileDownLoadItem)
                {
                    selected_FileDownLoadItem = value;
                    this.OnPropertyChanged("Selected_FileDownLoadItem");
                }
            }
        }

        vy downLoad_Failed_TipVisibility = vy.Collapsed;
        /// <summary>
        /// 上传失败提示
        /// </summary>
        public vy DownLoad_Failed_TipVisibility
        {
            get { return downLoad_Failed_TipVisibility; }
            set
            {
                if (value != downLoad_Failed_TipVisibility)
                {
                    downLoad_Failed_TipVisibility = value;
                    this.OnPropertyChanged("DownLoad_Failed_TipVisibility");
                }
            }
        }

        long all_DownloadNowValue = 0;
        /// <summary>
        /// 当前总进度
        /// </summary>
        public long All_DownloadNowValue
        {
            get { return all_DownloadNowValue; }
            set
            {
                if (value != all_DownloadNowValue)
                {
                    all_DownloadNowValue = value;
                    this.OnPropertyChanged("All_DownloadNowValue");
                }
            }
        }


        long all_DownloadMaxValue = 0;
        /// <summary>
        /// 当前所要完成的所有长度
        /// </summary>
        public long All_DownloadMaxValue
        {
            get { return all_DownloadMaxValue; }
            set
            {
                if (value != all_DownloadMaxValue)
                {
                    all_DownloadMaxValue = value;
                    this.OnPropertyChanged("All_DownloadMaxValue");
                }
            }
        }


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

        #endregion

        #region 开始下载文件

        public void Begin_Resource_DownLoad(string root)
        {
            try
            {
                if (this.BeginDownLoadCallBack != null)
                {
                    this.BeginDownLoadCallBack();
                }
                if (this.DownLoadItemCollection.Count > 0)
                {
                    this.DownLoad_AllCount = this.DownLoadItemCollection.Count;
                }
                this.DownloadWindowVisibility = vy.Visible;
                if (IsAllDownLoadCompleate)
                {
                    this.All_DownloadMaxValue = 0;
                    this.All_DownloadNowValue = 0;                   
                }
                foreach (var item in this.DownLoadItemCollection)
                {
                    if (!item.IsCompleate_DownLoad && !item.IsDownLoading_Now)
                    {
                        this.All_DownloadMaxValue += item.Length;
                        this.IsAllDownLoadCompleate = false;
                        item.IsDownLoading_Now = true;
                        item.WebClientHelper.FileDown(item.FileUri, root + "\\" + item.FileName, SpaceCodeEnterEntity.LoginUserName,
                                SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, new Action<int, long>((intProcess, recieve_Bytes) =>
                                {
                                    this.Dispatcher.BeginInvoke(new Action(() =>
                                        {
                                            item.Progress = intProcess;
                                            item.Recieve_Bytes += recieve_Bytes;
                                            //int int_process = intProcess + this.DownLoad_CompleateCount * 100;
                                            this.All_DownloadNowValue += recieve_Bytes;
                                            item.CompletePercent = string.Format(Percent_Formart, intProcess);
                                        }));

                                }), new Action<Exception, bool>((ex, IsSuccessed) =>
                                {
                                    this.Dispatcher.BeginInvoke(new Action(() =>
                                       {
                                           item.Progress = item.AllSize;
                                           item.CompletePercent = Percent_flg;
                                           item.IsDownLoading_Now = false;
                                           item.IsCompleate_DownLoad = true;
                                           this.DownLoad_CompleateCount++;
                                           if (this.DownLoad_CompleateCount == this.DownLoadItemCollection.Count)
                                           {
                                               this.DownLoad_Title_Tip = "下载完成";
                                               this.IsAllDownLoadCompleate = true;
                                           }
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

        #endregion


        #region 移除下载

        private void DownLoad_Delete_Click_CallBack(DownLoadItem obj)
        {
            try
            {
                if (this.DownLoadItemCollection.Contains(obj))
                {
                    this.DownLoadItemCollection.Remove(obj);

                    this.DownLoad_AllCount--;
                    if (obj.IsCompleate_DownLoad)
                    {
                        this.DownLoad_CompleateCount--;
                    }

                    obj.WebClientHelper.Dispose();

                    if (obj.IsDownLoading_Now)
                    {                      
                        this.All_DownloadNowValue -= obj.Recieve_Bytes;
                    }
                    else if(obj.IsCompleate_DownLoad)
                    {
                        this.All_DownloadNowValue -= obj.Length;
                    }

                    this.All_DownloadMaxValue -= obj.Length;
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

        public void DownLoad_Items_Add(DownLoadItem downLoadEntity)
        {
            try
            {
                //this.downLoadDataGrid.Items.Add(downLoadEntity);

                //this.downLoadEntityList.Add(downLoadEntity);

                this.DownLoadItemCollection.Add(downLoadEntity);
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
        /// 下载视图窗体弹出、关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void foldingDownLoadWindow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (SpaceCodeEnterEntity.DownLoad_View_IsDisplay)
                {
                    Storyboard_FoldingDownloadWindow.Begin();
                }
                else
                {
                    Storyboard_ExpandDownloadWindow.Begin();
                }

                SpaceCodeEnterEntity.DownLoad_View_IsDisplay = !SpaceCodeEnterEntity.DownLoad_View_IsDisplay;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        public void Open_DownLoad_View()
        {
            try
            {
                Storyboard_ExpandDownloadWindow.Begin();
                SpaceCodeEnterEntity.DownLoad_View_IsDisplay = true;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        public void Close_DownLoad_View()
        {
            try
            {
                Storyboard_FoldingDownloadWindow.Begin();
                SpaceCodeEnterEntity.DownLoad_View_IsDisplay = false;
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
        private void btn_DownLoad_OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DownloadWindowVisibility = vy.Collapsed;
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
