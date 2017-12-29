using ConferenceCommon.IconHelper;
using ConferenceCommon.LogHelper;
using PersonalSpaceApp.WPFHelper;
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
using System.Windows.Shapes;


namespace PersonalSpaceApp.Control
{
    /// <summary>
    /// ContentWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ContentWindow : WindowOperationBase
    {        
        #region 构造函数

        public ContentWindow()
        {
            try
            {
                InitializeComponent();            
                this.Window_Seize(0.9, 0.9);
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

        #region 加载内容

        public void ContentAdd(FrameworkElement element)
        {
            try
            {
                this.Content = element;
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
