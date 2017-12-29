using ConferenceCommon.LogHelper;
using ConferenceModel;
using ConferenceModel.Common;
using PersonalSpaceApp.Common;
using PersonalSpaceApp.Control;
using PersonalSpaceApp.WPFHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using vy = System.Windows.Visibility;

namespace PersonalSpaceApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowBase
    {
        #region 绑定属性

        double share_View_Opacity = 1;
        /// <summary>
        /// 分享视图显示设置
        /// </summary>
        public double Share_View_Opacity
        {
            get { return share_View_Opacity; }
            set
            {
                if (value != share_View_Opacity)
                {
                    share_View_Opacity = value;
                    this.OnPropertyChanged("Share_View_Opacity");
                }
            }
        }
        vy share_View_Visibility = vy.Collapsed;
        /// <summary>
        /// 分享视图显示设置
        /// </summary>
        public vy Share_View_Visibility
        {
            get { return share_View_Visibility; }
            set
            {
                if (value != share_View_Visibility)
                {
                    share_View_Visibility = value;
                    this.OnPropertyChanged("share_View_Visibility");
                }
            }
        }


        #endregion

        #region 一般属性

        ObservableCollection<Ad_ListViewItem> ad_ListViewCollection = new ObservableCollection<Ad_ListViewItem>()
        {

        };
        /// <summary>
        /// 以选择子项
        /// </summary>
        public ObservableCollection<Ad_ListViewItem> Ad_ListViewCollection
        {
            get { return ad_ListViewCollection; }
            set { ad_ListViewCollection = value; }
        }

        #endregion

        #region 显示控制


        /// <summary>
        /// 打开共享视图
        /// </summary>
        public void Share_View_Changed_TO_Open()
        {
            try
            {
                Storyboard_ShareFileWindowOpen.Begin();
                //SpaceCodeEnterEntity.Share_View_IsDisplay = true  ;
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

        #region 根级目录初始化

        public void RootTreeInit()
        {
            try
            {
                this.Ad_ListViewCollection.Clear();
                this.TreeChildInit(this.Ad_Root);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        public void TreeChildInit(Ad_TreeViewItem ad_TreeViewItem)
        {
            try
            {
                if (string.IsNullOrEmpty(ad_TreeViewItem.path)) return;

                string dicParameters = SpaceHelper.GetParameters_Path(SpaceType, SpaceCodeEnterEntity.GetAd_Tree, ad_TreeViewItem.path);
                ModelManage.Space_Service.Function_Invoke(SpaceCodeEnterEntity.GetAd_Tree, dicParameters,
                    SpaceCodeEnterEntity.LoginUserName, SpaceCodeEnterEntity.WebLoginPassword,
                    SpaceCodeEnterEntity.UserDomain, new Action<Dictionary<string, object>>((dicResult) =>
                {
                    this.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        string json = Convert.ToString(dicResult[SpaceCodeEnterEntity.Collection]);
                        List<Ad_TreeViewItem> adEntityList = JsonManage.JsonToEntity<Ad_TreeViewItem>(json, ',');
                        if (adEntityList.Count > 0)
                        {
                            ad_TreeViewItem.Ad_Children = new ObservableCollection<Ad_TreeViewItem>();
                            foreach (var item in adEntityList)
                            {
                                item.Style = Share_Item_Style;
                                //(this.Ad_Root as TreeViewItem).Items.Add(item);
                                ad_TreeViewItem.Ad_Children.Add(item);

                                string loginName = item.loginname;
                                if (!string.IsNullOrEmpty(loginName) && loginName.Contains("\\"))
                                {
                                    item.loginname = loginName.Substring(loginName.LastIndexOf("\\") + 1);
                                }

                                item.Ad_Parent = ad_TreeViewItem;
                                TreeChildInit(item);
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

        #region 选择、取消

        private void UnCheck_CallBack(Ad_TreeViewItem obj)
        {
            try
            {
                this.UnCheck_Item_ByShare(obj);

                this.Cancel_Selected_Parent(obj);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        void Cancel_Selected_Parent(Ad_TreeViewItem obj)
        {
            try
            {
                Ad_TreeViewItem Ad_Parent = obj.Ad_Parent;
                if (Ad_Parent != null)
                {
                    Ad_Parent.Is_Checked = false;
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

        void UnCheck_Item_ByShare(Ad_TreeViewItem obj)
        {
            try
            {
                if (obj.ad == "cn")
                {

                    Ad_ListViewItem ad_ListViewItem = obj.Ad_ListViewItem;
                    if (ad_ListViewItem != null)
                    {
                        if (Ad_ListViewCollection.Contains(ad_ListViewItem))
                        {
                            Ad_ListViewCollection.Remove(ad_ListViewItem);
                        }
                    }
                }
                else
                {
                    foreach (var item in obj.Ad_Children)
                    {
                        UnCheck_Item_ByShare(item);
                        item.Is_Checked = false;
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

        private void Check_CallBack(Ad_TreeViewItem obj)
        {
            try
            {
                this.Check_Item_ByShare(obj);

                this.Selected_Parent(obj);
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        void Selected_Parent(Ad_TreeViewItem obj)
        {
            try
            {
                Ad_TreeViewItem Ad_Parent = obj.Ad_Parent;
                if (Ad_Parent != null)
                {
                    bool isAllChecked = true;
                    foreach (var item in Ad_Parent.Ad_Children)
                    {
                        if (!item.Is_Checked)
                        {
                            isAllChecked = false;
                        }
                    }
                    if (isAllChecked)
                    {
                        Ad_Parent.Is_Checked = true;
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

        void Check_Item_ByShare(Ad_TreeViewItem obj)
        {
            try
            {
                if (obj.ad == "cn")
                {
                    if (obj.Ad_ListViewItem != null && Ad_ListViewCollection.Contains(obj.Ad_ListViewItem))
                    {
                        return;
                    }
                    else
                    {
                        Ad_ListViewItem item = new Ad_ListViewItem();
                        item.name = obj.name;
                        item.path = obj.path;
                        item.Style = Ad_ListViewItem_Style;
                        item.loginname = obj.loginname;


                        obj.Ad_ListViewItem = item;
                        item.Ad_TreeViewItem = obj;
                        Ad_ListViewCollection.Add(item);
                    }
                }
                else if (obj.ad == "ou")
                {
                    foreach (var item in obj.Ad_Children)
                    {
                        Check_Item_ByShare(item);
                        item.Is_Checked = true;
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

        #region 选择、取消（右侧）

        private void Item_Cancel_CallBack(Ad_ListViewItem obj)
        {
            try
            {
                if (Ad_ListViewCollection.Contains(obj))
                {
                    Ad_ListViewCollection.Remove(obj);
                }
                Ad_TreeViewItem ad_TreeViewItem = obj.Ad_TreeViewItem;
                if (ad_TreeViewItem != null)
                {
                    ad_TreeViewItem.Is_Checked = false;
                    this.Cancel_Selected_Parent(ad_TreeViewItem);
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

        #region 确认分享，取消分享

        private void gridShareFileWindow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                foreach (var item in this.Ad_ListViewCollection)
                {
                    string dicParameters = string.Empty;
                    switch (this.SpaceType)
                    {
                        case SpaceType.PersonSpace:

                            string caml = "<Where><Eq><FieldRef Name='FileLeafRef' /><Value Type='Text'>" + item.loginname + "</Value></Eq></Where>";
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
                                    string id_Root_str = sPItemEntity_List[0].ID;
                                    int id_Root = Convert.ToInt32(sPItemEntity_List[0].ID);

                                    Share_Foreach_Helper(id_Root, this.Selected_PageViewBase.SpaceListViewItemList);                                  
                                }));
                            }
                        }
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

        private void Share_Foreach_Helper(int id_Root, List<SpaceListViewItem> SpaceListViewItemList)
        {
              try
            {
                int shareCount =0;
                foreach (var _Item in SpaceListViewItemList)
                {
                    int id = 0;
                    string file_folder_name = null;
                    switch (_Item.BookType)
                    {
                        case BookType.File:
                            id = _Item.Self_File.ID;
                            file_folder_name = _Item.Title;
                            break;
                        case BookType.Folder:
                            id = _Item.Self_Folder.ID;
                            file_folder_name = _Item.Title;
                            break;
                        default:
                            break;
                    }
                    string parameters = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.CreateFolder, id_Root, SpaceCodeEnterEntity.SelfName + "__分享区域");
                    ModelManage.Space_Service.Function_Invoke(SpaceCodeEnterEntity.CreateFolder, parameters, SpaceCodeEnterEntity.LoginUserName, SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, new Action<Dictionary<string, object>>((dicResult2) =>
                    {
                        if (dicResult2.Count > 0)
                        {
                            int User_ID = Convert.ToInt32(dicResult2[SpaceCodeEnterEntity.NewFolderID]);


                            string parametersBottom = SpaceHelper.GetParameters(this.SpaceType, SpaceCodeEnterEntity.Copy_Item, SpaceCodeEnterEntity.SP_ListName_Value, id, SpaceCodeEnterEntity.SP_ListName_Value, User_ID);
                            ModelManage.Space_Service.Function_Invoke(SpaceCodeEnterEntity.Copy_Item, parametersBottom, SpaceCodeEnterEntity.LoginUserName, SpaceCodeEnterEntity.WebLoginPassword, SpaceCodeEnterEntity.UserDomain, new Action<Dictionary<string, object>>((dicResult3) =>
                            {
                                if (dicResult3.Count > 0)
                                {
                                    shareCount++;

                                   if(shareCount ==SpaceListViewItemList.Count)
                                   {
                                       MessageBox.Show("分享成功");
                                   }
                                }
                            }));
                        }
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

        private void gridShareFileWindow_PreviewMouseLeftButtonDown2(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Storyboard_ShareFileWindowClose.Begin();
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
