using ConferenceCommon.JsonHelper;
using ConferenceCommon.LogHelper;
using PersonalSpaceApp.WPFControl;
using PersonalSpaceApp.WPFHelper;
using ConferenceModel;
using PersonalSpaceApp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PersonalSpaceApp.Control
{
    public partial class SpaceListViewItem : ListViewItem
    {
        #region 静态回调

        /// <summary>
        /// 是否包含子项
        /// </summary>
        public static Action<NavicateListView, SpaceListViewItem, CallBackType> ItemsSelectedEventCallBack = null;

        #endregion

        #region 静态资源

        /// <summary>
        /// 样式
        /// </summary>
        static Style SpaceListViewItemStyle1 = null;

        /// <summary>
        /// 样式
        /// </summary>
        static Style SpaceListViewItemStyle2 = null;

        #endregion

        #region 一般属性

        string date;
        /// <summary>
        /// 书本的发布日期
        /// </summary>
        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        BookType bookType = BookType.Folder;
        /// <summary>
        /// 书本类型【文件、文件夹】
        /// </summary>
        public BookType BookType
        {
            get { return bookType; }
            set
            {
                bookType = value;
            }
        }

        FileType fileType;
        /// <summary>
        /// 文件类型
        /// </summary>
        public FileType FileType
        {
            get { return fileType; }
            set
            {
                fileType = value;
            }
        }

        string uri;
        /// <summary>
        /// 路径
        /// </summary>
        public string Uri
        {
            get { return uri; }
            set { uri = value; }
        }

        BreadLine breadLine = null;
        /// <summary>
        /// 书本关联的面包线（属于文件夹的时候才有）
        /// </summary>
        public BreadLine BreadLine
        {
            get { return breadLine; }
            set { breadLine = value; }
        }

        /// <summary>
        /// 绑定SharePoint文件
        /// </summary>
        internal SPVirtualFile Self_File { get; set; }

        /// <summary>
        /// 绑定SharePoint文件夹
        /// </summary>
        internal SPVirtualFolder Self_Folder { get; set; }

        /// <summary>
        /// 空间类型
        /// </summary>
        public SpaceType SpaceType { get; set; }

        /// <summary>
        /// 父类
        /// </summary>
        public NavicateListView ParentView { get; set; }

        #endregion

        #region 构造函数

        /// <summary>
        /// 书本的构造函数(普通元素构造者)
        /// </summary>
        /// <param name="title">书本的标题</param>             
        /// <param name="imageUri">书的背景图</param>
        public SpaceListViewItem(string imageUri, ViewType viewType)
            : this()
        {
            try
            {
                this.ParametersInit(imageUri);

                this.SettingStyle(viewType);
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

        public void ParametersInit(string imageUri)
        {
            try
            {
                Uri uri = default(Uri);

                if (imageUri.Contains("http://"))
                {
                    uri = new Uri(imageUri);
                }
                else
                {
                    uri = new Uri(imageUri, UriKind.RelativeOrAbsolute);
                }
                BitmapImage imageSource = new BitmapImage(uri);
                this.Img_Brush = imageSource;

                #region oldsolution

              


                #endregion
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


        #region 确定样式

        public void SettingStyle(ViewType viewType)
        {
             try
            {
                switch (viewType)
                {
                    case ViewType.PictureView:
                        if (SpaceListViewItemStyle1 == null)
                        {
                            if (Application.Current.Resources.Contains("SpaceListViewItemStyle1"))
                            {
                                SpaceListViewItemStyle1 = Application.Current.Resources["SpaceListViewItemStyle1"] as Style;
                            }
                        }
                        this.Style = SpaceListViewItemStyle1;
                        break;
                    case ViewType.ListView:
                        if (SpaceListViewItemStyle2 == null)
                        {
                            if (Application.Current.Resources.Contains("SpaceListViewItemStyle2"))
                            {
                                SpaceListViewItemStyle2 = Application.Current.Resources["SpaceListViewItemStyle2"] as Style;
                            }
                        }
                        this.Style = SpaceListViewItemStyle2;
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

        public void SetSmallImg(string imageUri)
        {
            try
            {
                Uri uri = default(Uri);

                if (imageUri.Contains("http://"))
                {
                    uri = new Uri(imageUri);
                }
                else
                {
                    uri = new Uri(imageUri, UriKind.RelativeOrAbsolute);
                }
                BitmapImage imageSource = new BitmapImage(uri);
                this.Img_Small_Brush = imageSource;
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

        #region 打开子项操作

        public void OpenItem()
        {
            try
            {
                if (!this.IsRenameState)
                {
                    if (ItemsSelectedEventCallBack != null && this.ParentView != null)
                    {
                        ItemsSelectedEventCallBack(this.ParentView,this, CallBackType.OpenItem);
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

        #region 取消重命名样式

        public void Cancel_ReName()
        {
             try
            {
                this.IsRenameState = false;
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

        #region 子项选择

        /// <summary>
        /// 子项添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Check_Selected()
        {
            try
            {
                if (ItemsSelectedEventCallBack != null&& this.ParentView!=null)
                {
                    ItemsSelectedEventCallBack(this.ParentView,this, CallBackType.One_Selected);
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
        /// 子项移除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Check_UnSelected()
        {
            try
            {
                if (ItemsSelectedEventCallBack != null&& this.ParentView!=null)
                {
                    ItemsSelectedEventCallBack(this.ParentView,this, CallBackType.One_UnSelected);
                }

                this.IsRenameState = false;
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

        #region key键入事件

        public void Rename_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == System.Windows.Input.Key.Return)
                {
                    if (sender is TextBox)
                    {
                        TextBox txt = sender as TextBox;
                        this.Title = txt.Text;
                        if (ItemsSelectedEventCallBack != null&& this.ParentView!=null)
                        {
                            ItemsSelectedEventCallBack(this.ParentView,this, CallBackType.KeDown);
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

        #endregion

        #region 展开操作面板

        public void Open_Operation_Panel()
        {
            try
            {               
                this.Is_Open = true;
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

        #region 分享、下载、删除

        public void Share_Click()
        {
            try
            {
                if (ItemsSelectedEventCallBack != null&& this.ParentView!=null)
                {
                    ItemsSelectedEventCallBack(this.ParentView,this, CallBackType.Share);
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

        public void Download_Click()
        {
            try
            {
                if (ItemsSelectedEventCallBack != null&& this.ParentView!=null)
                {
                    ItemsSelectedEventCallBack(this.ParentView,this, CallBackType.Download);
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

       

        public void Delete_Click()
        {
              try
            {
                if (ItemsSelectedEventCallBack != null && this.ParentView != null)
                {
                    ItemsSelectedEventCallBack(this.ParentView, this, CallBackType.Delete);
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

        #region 重命名

        public void ReName_Click()
        {
              try
            {
                if (ItemsSelectedEventCallBack != null && this.ParentView != null)
                {
                    ItemsSelectedEventCallBack(this.ParentView, this, CallBackType.ReName);
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

        #region 移动到

        public void Move_Click()
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
    }


    public enum CallBackType
    {
        OpenItem,
        One_Selected, 
        All_Selected,
        One_UnSelected,
        All_UnSelected,
        KeDown,
        Share,
        Download,
        Delete,
        ReName
    }
}
