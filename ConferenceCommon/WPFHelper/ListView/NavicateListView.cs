using ConferenceCommon.LogHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ConferenceCommon.WPFHelper
{
    public class NavicateListView : ListView
    {
        #region 一般属性

        public AreaType AreaType { get; set; }

        #endregion

        #region 注册依赖项属性

        public static readonly DependencyProperty ItemWidthProperty = DependencyProperty.Register("ItemWidth", typeof(double), typeof(NavicateListView), new FrameworkPropertyMetadata((double)0));
        public double ItemWidth
        {
            get
            {
                return (double)this.GetValue(ItemWidthProperty);
            }
            set
            {
                this.SetValue(ItemWidthProperty, value);
            }
        }

        public static readonly DependencyProperty ItemHeightProperty = DependencyProperty.Register("ItemHeight", typeof(double), typeof(NavicateListView), new FrameworkPropertyMetadata((double)0));
        public double ItemHeight
        {
            get
            {
                return (double)this.GetValue(ItemHeightProperty);
            }
            set
            {
                this.SetValue(ItemHeightProperty, value);
            }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation", typeof(Orientation), typeof(NavicateListView), new FrameworkPropertyMetadata(Orientation.Vertical));
        public Orientation Orientation
        {
            get
            {
                return (Orientation)this.GetValue(OrientationProperty);
            }
            set
            {

                this.SetValue(OrientationProperty, value);
            }
        }

        //public static readonly DependencyProperty IsItemAllCheckedProperty = DependencyProperty.Register("IsItemAllChecked", typeof(bool), typeof(NavicateListView), new FrameworkPropertyMetadata(false));
        //public bool IsItemAllChecked
        //{
        //    get
        //    {
        //        return (bool)this.GetValue(IsItemAllCheckedProperty);
        //    }
        //    set
        //    {
        //        this.SetValue(IsItemAllCheckedProperty, value);
        //    }
        //}


        #endregion

        #region 自定义路由事件(模拟,节省性能)


        public event Action<NavicateListView, NavicateListViewItem> Naivcate_SelectionChanged;

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            try
            {
                base.OnSelectionChanged(e);
                if (Naivcate_SelectionChanged != null)
                {
                    if (this.SelectedItem != null && this.SelectedItem is NavicateListViewItem)
                    {
                        NavicateListViewItem item = this.SelectedItem as NavicateListViewItem;
                        this.Naivcate_SelectionChanged(this, item);
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

        #region 构建

        static NavicateListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavicateListView), new FrameworkPropertyMetadata(typeof(NavicateListView)));
        }

        #endregion


        #region 构造函数

        public NavicateListView()
        {
            this.DataContext = this;
           
        }

        #endregion
    }

    public enum AreaType
    {
        NormalButton,
        NavicateType,
    }
}
