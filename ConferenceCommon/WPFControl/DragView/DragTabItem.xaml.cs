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

namespace ConferenceCommon.WPFControl.DragView
{
    /// <summary>
    /// DragTabItem.xaml 的交互逻辑
    /// </summary>
    public partial class DragTabItem : UserControl
    {
        private FrameworkElement element;

        public FrameworkElement Element
        {
            get { return element; }
            set
            {
                if (value != null && value is FrameworkElement)
                {
                    value.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    value.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
                    this.borderMain.Child = value;
                    element = value;
                }
            }
        }

        string tittle;
        /// <summary>
        /// 设置标题
        /// </summary>
        public string Tittle
        {
            get { return tittle; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this.txtTitle.Text = value;
                    tittle = value;
                }
            }
        }

        public DragTabItem()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }

        private void lbl_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                lbl.Visibility = System.Windows.Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                LogManage.WriteLog(this.GetType(), ex);
            }
            finally
            {
            }
        }
    }
}
