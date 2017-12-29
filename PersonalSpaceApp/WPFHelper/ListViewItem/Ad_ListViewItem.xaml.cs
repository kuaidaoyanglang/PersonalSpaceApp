using ConferenceCommon.LogHelper;
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

namespace PersonalSpaceApp.WPFHelper
{
    /// <summary>
    /// Ad_ListViewItem.xaml 的交互逻辑
    /// </summary>
    public partial class Ad_ListViewItem : ListViewItem
    {
        #region 事件回调

        public static Action<Ad_ListViewItem> Item_Cancel_CallBack = null;

        #endregion

        #region 注册依赖项属性

        public static readonly DependencyProperty nameProperty = DependencyProperty.Register("name", typeof(string), typeof(Ad_ListViewItem), new FrameworkPropertyMetadata(string.Empty));
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
        #endregion

        #region 构造函数

        public Ad_ListViewItem()
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

        static Ad_ListViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Ad_ListViewItem), new FrameworkPropertyMetadata(typeof(Ad_ListViewItem)));
        }

        #endregion

        #region 一般属性
    
        /// <summary>
        /// 路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 未选择前的原型
        /// </summary>
        public Ad_TreeViewItem Ad_TreeViewItem { get; set; }

        /// <summary>
        /// 登录名称
        /// </summary>
        public string loginname { get; set; }

        #endregion

        #region 绑定事件

        public void Item_Click()
        {
             try
            {
               if(Item_Cancel_CallBack!= null)
               {
                   Item_Cancel_CallBack(this);
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
