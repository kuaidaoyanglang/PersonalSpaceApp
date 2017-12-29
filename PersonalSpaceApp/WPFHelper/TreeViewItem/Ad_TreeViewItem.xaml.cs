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
using System.Collections.ObjectModel;
using ConferenceCommon.LogHelper;

namespace PersonalSpaceApp.WPFHelper
{
    /// <summary>
    /// Ad_TreeViewItem.xaml 的交互逻辑
    /// </summary>
    public partial class Ad_TreeViewItem : TreeViewItem
    {
        #region 事件回调

        /// <summary>
        /// 选择回调
        /// </summary>
        public static Action<Ad_TreeViewItem> Check_CallBack = null;

        /// <summary>
        /// 取消回调
        /// </summary>
        public static Action<Ad_TreeViewItem> UnCheck_CallBack = null;

        #endregion

        #region 注册依赖项属性

        public static readonly DependencyProperty nameProperty = DependencyProperty.Register("name", typeof(string), typeof(Ad_TreeViewItem), new FrameworkPropertyMetadata(string.Empty));
        public string name
        {
            get
            {
                return (string)this.GetValue(nameProperty);
            }
            set
            {
                this.SetValue(nameProperty, value);
            }
        }

        public static readonly DependencyProperty Check_VisibilityProperty = DependencyProperty.Register("Check_Visibility", typeof(Visibility), typeof(Ad_TreeViewItem), new FrameworkPropertyMetadata(Visibility.Visible));
        public Visibility Check_Visibility
        {
            get
            {
                return (Visibility)this.GetValue(Check_VisibilityProperty);
            }
            set
            {
                this.SetValue(Check_VisibilityProperty, value);
            }
        }

        //private ObservableCollection<Ad_TreeViewItem> ad_Children;
        ///// <summary>
        ///// 子项集合
        ///// </summary>
        //public ObservableCollection<Ad_TreeViewItem> Ad_Children
        //{
        //    get { return ad_Children; }
        //    set { ad_Children = value; }
        //}

        public static readonly DependencyProperty Ad_ChildrenProperty = DependencyProperty.Register("Ad_Children", typeof(ObservableCollection<Ad_TreeViewItem>), typeof(Ad_TreeViewItem), new FrameworkPropertyMetadata(null));
        public ObservableCollection<Ad_TreeViewItem> Ad_Children
        {
            get
            {
                return (ObservableCollection<Ad_TreeViewItem>)this.GetValue(Ad_ChildrenProperty);
            }
            set
            {
                this.SetValue(Ad_ChildrenProperty, value);
            }
        }

        public static readonly DependencyProperty adProperty = DependencyProperty.Register("ad", typeof(string), typeof(Ad_TreeViewItem), new FrameworkPropertyMetadata(string.Empty));
        public string ad
        {
            get
            {
                return (string)this.GetValue(adProperty);
            }
            set
            {
                this.SetValue(adProperty, value);
            }
        }

        public static readonly DependencyProperty Is_CheckedProperty = DependencyProperty.Register("Is_Checked", typeof(bool), typeof(Ad_TreeViewItem), new FrameworkPropertyMetadata(false));
        public bool Is_Checked
        {
            get
            {
                return (bool)this.GetValue(Is_CheckedProperty);
            }
            set
            {
                this.SetValue(Is_CheckedProperty, value);
            }
        }


        #endregion

        #region 一般属性

        /// <summary>
        /// 登录名称
        /// </summary>
        public string loginname { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 父项
        /// </summary>
        public Ad_TreeViewItem Ad_Parent { get; set; }


        /// <summary>
        /// 已选择的项（UI变更）
        /// </summary>
        public Ad_ListViewItem Ad_ListViewItem { get; set; }

        #endregion

        #region 构造函数

        public Ad_TreeViewItem()
        {
            try
            {
                this.DataContext = this;
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

        #region 构建

        static Ad_TreeViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Ad_TreeViewItem), new FrameworkPropertyMetadata(typeof(Ad_TreeViewItem)));
        }

        #endregion

        #region 绑定事件

        public void Checked()
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

        public void Unchecked()
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

        public void Check_Click()
        {
            try
            {
                if (this.Is_Checked)
                {
                    if (Check_CallBack != null)
                    {
                        Check_CallBack(this);
                    }
                }
                else
                {
                    if (UnCheck_CallBack != null)
                    {
                        UnCheck_CallBack(this);
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
    }
}
