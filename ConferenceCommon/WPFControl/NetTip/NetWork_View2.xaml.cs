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

namespace ConferenceCommon.WPFControl.NetTip
{
    /// <summary>
    /// NetWork_View2.xaml 的交互逻辑
    /// </summary>
    public partial class NetWork_View2 : UserControl
    {
        public NetWork_View2()
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

        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="message">信息内容</param>
        public void SetMessage(string message)
        {
            try
            {
                this.txtMessage.Text = message;
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
